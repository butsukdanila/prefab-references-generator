using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

using Object = UnityEngine.Object;

namespace PrefabRefsGenerator
{
	public abstract class FixedScriptableSingleton<T> : ScriptableObject
		where T : FixedScriptableSingleton<T>
	{
		private static T s_instance;
		private SerializedObject m_serialized;

		public static T instance
		{
			get
			{
				if (s_instance != null || Load() || Create()) return s_instance;
				throw new System.Exception($"{typeof(T).Name} wasn't loaded or created");
			}
		}

		public SerializedObject serializedObject
		{
			get
			{
				if (m_serialized != null || Serialize()) return m_serialized;
				throw new System.Exception($"{typeof(T).Name} wasn't loaded or created");
			}
		}

		public virtual bool saveAsText => true;

		private static bool Load()
		{
			var filePath = GetFilePath();
			if (string.IsNullOrEmpty(filePath)) return false;

			var result = InternalEditorUtility.LoadSerializedFileAndForget(filePath);
			if (result.Length == 0) return false;

			s_instance = result[0] as T;
			return s_instance != null;
		}

		private static bool Create()
		{
			s_instance = CreateInstance<T>();
			s_instance.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;
			return s_instance != null;
		}

		private bool Serialize()
		{
			m_serialized = new SerializedObject(this);
			return m_serialized != null;
		}

		public void Save()
		{
			if (s_instance == null) throw new System.Exception($"Can't save {typeof(T).Name} there is no instance");

			var filePath = GetFilePath();
			if (string.IsNullOrEmpty(filePath)) return;

			var directory = Path.GetDirectoryName(filePath);
			if (string.IsNullOrEmpty(directory)) return;
			Directory.CreateDirectory(directory);

			InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { s_instance }, filePath, saveAsText);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		private static string GetFilePath()
		{
			foreach (var customAttribute in typeof(T).GetCustomAttributes(true))
			{
				if (customAttribute is not FilePathAttribute filePathAttr) continue;

				var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				var filePathGet = filePathAttr.GetType().GetProperty("filepath", bindings).GetGetMethod(true);
				if (filePathGet.Invoke(filePathAttr, null) is not string result) return string.Empty;

				return result;
			}
			return string.Empty;
		}
	}
}
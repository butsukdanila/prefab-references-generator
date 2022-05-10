using System;
using System.Collections.Generic;

using PrefabRefsGenerator.Utilities;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace PrefabRefsGenerator
{
	[FilePath("Assets/Editor/PrefabRefsGeneratorState.asset", FilePathAttribute.Location.ProjectFolder)]
	public class PrefabRefsGeneratorState : Utilities.Editor.ScriptableSingleton<PrefabRefsGeneratorState>
	{
		[SerializeField] private string m_generationFolder;
		[SerializeField] private TagToTypeMap m_tagToType;
		[SerializeField] private List<PrefabRecord> m_records;

		private string m_defaultFolder;

		public string generationFolder
		{
			get
			{
				if (string.IsNullOrEmpty(m_generationFolder))
					m_generationFolder = m_defaultFolder;
				return m_generationFolder;
			}
		}

		private void OnEnable()
		{
			m_defaultFolder = Application.dataPath.UnifyPath();
		}

		public void Add(PrefabRecord target)
		{
			if (Contains(target)) return;
			m_records.Add(target);
			Save();
		}

		public bool Remove(PrefabRecord target)
		{
			if (!Contains(target)) return false;
			if (!m_records.Remove(target)) return false;
			Save();
			return true;
		}

		private bool Contains(PrefabRecord target)
		{
			var targetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target.prefab);
			foreach (var record in m_records)
			{
				if (record.prefab == target.prefab) return true;

				var recordPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(record.prefab);
				if (recordPath == targetPath) return true;
			}
			return false;
		}

		public void CreateGUI(VisualElement parent)
		{
			CreateFolderSelector(parent);
			CreateTagToTypeMap(parent);
			CreateRecords(parent);
		}

		private void CreateFolderSelector(VisualElement parent)
		{
			var container = parent.Add<VisualElement>();

			var generationFolder = serializedObject.FindProperty(nameof(m_generationFolder));
			var fs = container.Add<FolderSelector>();
			fs.defaultFolder = m_defaultFolder;
			fs.label.text = generationFolder.displayName;
			fs.textField.BindProperty(generationFolder);
			fs.textField.TrackPropertyValue(generationFolder, _ => Save());
		}

		private void CreateTagToTypeMap(VisualElement parent)
		{
			var container = parent.Add<VisualElement>();

			var pf = container.Add<PropertyField>();
			pf.BindProperty(serializedObject.FindProperty(nameof(m_tagToType)));
			pf.Bind(serializedObject);
		}

		private void CreateRecords(VisualElement parent)
		{
			var container = parent.Add<VisualElement>();
			container.style.WithMargin(1, 5, 5, 1);

			var records = serializedObject.FindProperty(nameof(m_records));
			var lv = container.Add<ListView>();
			lv.headerTitle = records.displayName;
			lv.showFoldoutHeader = true;
			lv.showBoundCollectionSize = false;
			lv.showBorder = true;
			lv.showAddRemoveFooter = true;

			lv.itemsSource = m_records;
			lv.makeItem = new(() => { return new PrefabRecord.Visual(); });
			lv.bindItem = new((e, i) => (e as PrefabRecord.Visual).record = m_records[i]);
			lv.itemsAdded += _ => Save();
			lv.itemsRemoved += _ => Save();
			lv.fixedItemHeight = 20;
		}
	}

	[Serializable]
	public sealed class PrefabRecord
	{
		[SerializeField] public GameObject m_prefab;
		[SerializeField] public MonoScript m_refs;

		public GameObject prefab => m_prefab;
		public MonoScript refs => m_refs;

		public sealed class Visual : VisualElement
		{
			private readonly ObjectField m_prefab = new();
			private readonly ObjectField m_refs = new();

			public PrefabRecord record { get; set; }

			public Visual()
			{
				Add(m_prefab);
				Add(m_refs);

				style.flexDirection = FlexDirection.Row;

				m_prefab.style.flexGrow = 1;
				m_refs.style.flexGrow = 1;

				m_refs.SetEnabled(false);

				m_prefab.objectType = typeof(GameObject);
				m_prefab.allowSceneObjects = false;
			}
		}
	}

	[Serializable]
	public sealed class TagToTypeMap : SerializedDictionary<string, SerializedType> { }

	public static class VisualElementEx
	{
		public static TElement Add<TElement>(this VisualElement target)
			where TElement : VisualElement, new()
		{
			var newElement = new TElement();
			target.Add(newElement);
			return newElement;
		}
	}

	public static class UIElementsStyleBuilderEx
	{
		public static IStyle WithPadding(this IStyle target, float bottom, float left, float right, float top)
		{
			target.paddingBottom = bottom;
			target.paddingLeft = left;
			target.paddingRight = right;
			target.paddingTop = top;
			return target;
		}

		public static IStyle WithPadding(this IStyle target, float padding)
		{
			return target.WithPadding(padding, padding, padding, padding);
		}

		public static IStyle WithBorder(this IStyle target, float width, float radius, Color color)
		{
			target.borderBottomWidth = width;
			target.borderLeftWidth = width;
			target.borderRightWidth = width;
			target.borderTopWidth = width;
			target.borderBottomLeftRadius = radius;
			target.borderBottomRightRadius = radius;
			target.borderTopLeftRadius = radius;
			target.borderTopRightRadius = radius;
			target.borderBottomColor = color;
			target.borderLeftColor = color;
			target.borderRightColor = color;
			target.borderTopColor = color;
			return target;
		}

		public static IStyle WithMargin(this IStyle target, float bottom, float left, float right, float top)
		{
			target.marginBottom = bottom;
			target.marginLeft = left;
			target.marginRight = right;
			target.marginTop = top;
			return target;
		}

		public static IStyle WithMargin(this IStyle target, float margin)
		{
			return target.WithMargin(margin, margin, margin, margin);
		}

		public static IStyle WithBackground(this IStyle target, Color color)
		{
			target.backgroundColor = color;
			return target;
		}
	}
}
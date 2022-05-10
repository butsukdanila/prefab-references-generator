using System;
using System.Collections.Generic;
using System.Linq;

using PrefabRefsGenerator.Utilities;

using UnityEditor;

using UnityEngine;

namespace PrefabRefsGenerator
{
	public partial class RefsClassGenerator
	{
		private readonly string m_directory;
		private readonly string m_classNamespace;
		private readonly string m_className;
		private readonly string[] m_excludedPaths;
		private readonly RefInfo[] m_refInfos;

		public RefsClassGenerator(InitInfo initInfo)
		{
			m_directory = initInfo.directory;
			if (string.IsNullOrEmpty(m_directory)) throw new ArgumentException("Directory cannot be null or empty");

			m_className = initInfo.className;
			if (string.IsNullOrEmpty(m_className)) throw new ArgumentException("Class name cannot be null or empty");

			m_classNamespace = initInfo.classNamespace;
			if (string.IsNullOrEmpty(m_classNamespace)) throw new ArgumentException("Class namespace cannot be null or empty");

			m_excludedPaths = PopulateExcludedPaths(initInfo.excluded);

			m_refInfos = PopulateRefInfos(initInfo.target, initInfo.tagToType);
			if (m_refInfos.Length == 0) throw new Exception("Ref infos are empty. Nothing to generate");
		}

		private string[] PopulateExcludedPaths(IReadOnlyList<GameObject> objects)
		{
			if (objects == null) return Array.Empty<string>();
			return objects.Select(go => PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go)).ToArray();
		}

		private RefInfo[] PopulateRefInfos(GameObject target, IReadOnlyDictionary<string, Type> tagToType)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			if (tagToType?.Count == 0) throw new ArgumentException("Tag to type dictionary cannot be null or empty");

			var result = new List<RefInfo>();
			foreach (var child in target.ChildrenDepthFirstTraversal())
			{
				var childPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
				if (m_excludedPaths.Contains(childPath)) continue;

				if (!RefTagsParser.TryParse(child.name, out var tagless, out var tags)) continue;

				foreach (var tag in tags)
				{
					if (!tagToType.TryGetValue(tag, out var type))
					{
						Debug.LogWarning($"Tag to type map doesn't contain type for tag '{tag}'");
						continue;
					}

					if (!typeof(Component).IsAssignableFrom(type))
					{
						Debug.LogWarning($"'{type.Name}' doesn't derive from '{typeof(Component).Name}'");
						continue;
					}

					result.Add(new()
					{
						type = type,
						name = RefNameFormatter.Format(tagless, tag),
						owner = child.name
					});
				}
			}
			return result.ToArray();
		}

		public struct InitInfo
		{
			public string directory;
			public string classNamespace;
			public string className;
			public GameObject target;
			public IReadOnlyDictionary<string, Type> tagToType;
			public IReadOnlyList<GameObject> excluded;
		}

		private class RefInfo
		{
			public Type type;
			public string name;
			public string owner;
		}
	}
}

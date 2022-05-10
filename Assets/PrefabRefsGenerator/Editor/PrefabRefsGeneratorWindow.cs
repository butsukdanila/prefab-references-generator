using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

namespace PrefabRefsGenerator
{
	public class PrefabRefsGeneratorWindow : EditorWindow
	{
		private static PrefabRefsGeneratorState state => PrefabRefsGeneratorState.instance;

		[MenuItem("Tools/Prefab References Generator")]
		private static void Open()
		{
			GetWindow<PrefabRefsGeneratorWindow>("Prefab References Generator").Show();
		}

		private void OnEnable()
		{
			PrefabStage.prefabSaved += PrefabStage_prefabSaved;
			PrefabStage.prefabStageDirtied += PrefabStage_prefabStageDirtied;
			PrefabStage.prefabStageClosing += PrefabStage_prefabStageClosing;
		}

		public void CreateGUI()
		{
			state.CreateGUI(rootVisualElement);
		}

		private List<RefsClassGenerator> m_pendingGenerators = new();

		private void PrefabStage_prefabStageDirtied(PrefabStage obj)
		{
			//rootVisualElement.SetEnabled(false);
		}

		private void PrefabStage_prefabSaved(GameObject obj)
		{
			//var stage = PrefabStageUtility.GetPrefabStage(obj);
			//if (stage == null) return;

			// todo: search from selected prefabs
			//var selectPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(m_prefab); 
			//if (!string.Equals(stage.assetPath, selectPath)) return;

			//var selectedGenerator = new RefsClassGenerator(new()
			//{
			//	directory = m_folderSelector.current,
			//	classNamespace = "GeneratedCode",
			//	className = m_prefab.name + "Refs",
			//	tagToType = m_tagToType,
			//	target = m_prefab,
			//	excluded = m_otherPrefabs
			//});

			//todo: SHA- 256
			//var selected g.GenerateSHA256();
		}

		private void PrefabStage_prefabStageClosing(PrefabStage obj)
		{
			//var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(m_prefab);
			//if (!string.Equals(obj.assetPath, path)) return;

			//EditorApplication.delayCall += () =>
			//{
			//	AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
			//	EditorUtility.RequestScriptReload();
			//  rootVisualElement.SetEnabled(true);
			//};

			//rootVisualElement.SetEnabled(true);
		}

		public void Generate()
		{
			//var sg = new RefsClassGenerator(new()
			//{
			//	directory = m_selectedFolder,
			//	classNamespace = "GeneratedCode",
			//	className = m_prefab.name + "Refs",
			//	tagToType = m_tagToType,
			//	target = m_prefab,
			//	excluded = m_otherPrefabs
			//});

			//sg.Generate();

			//AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
			//EditorUtility.RequestScriptReload();
		}
	}
}
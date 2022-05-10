using System.IO;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace PrefabRefsGenerator.Utilities
{
	public class FolderSelector : VisualElement
	{
		private const string c_select_text = "Select";
		private const string c_reset_text = "Reset";

		private readonly Button m_selectButton = new();
		private readonly Button m_resetButton = new();
		public readonly Label label = new();
		public readonly TextField textField = new();

		public string defaultFolder { get; set; }

		public FolderSelector()
		{
			var container = this.Add<VisualElement>();
			container.style.flexDirection = FlexDirection.Row;
			container.style.WithPadding(0).WithMargin(1, 3, 3, 1);

			label = container.Add<Label>();
			label.style.unityTextAlign = TextAnchor.MiddleCenter;
			label.style.WithPadding(0).WithMargin(1, 3, 3, 1);

			textField = container.Add<TextField>();
			textField.style.flexGrow = 1;
			textField.style.flexShrink = 1;
			textField.isReadOnly = true;

			m_selectButton = container.Add<Button>();
			m_selectButton.text = c_select_text;
			m_selectButton.clicked += Select;

			m_resetButton = container.Add<Button>();
			m_resetButton.text = c_reset_text;
			m_resetButton.clicked += Reset;
		}

		private void Select()
		{
			var openFolder = textField.value == defaultFolder ?
				defaultFolder :
				Path.Combine(defaultFolder, textField.value).UnifyPath();

			var selected = EditorUtility.OpenFolderPanel(string.Empty, openFolder, string.Empty);
			if (string.IsNullOrEmpty(selected)) return;

			textField.value = selected == defaultFolder ?
				defaultFolder :
				selected.Replace(defaultFolder, string.Empty).UnifyPath();
		}

		private void Reset()
		{
			textField.value = defaultFolder;
		}
	}

	public static class StringPathEx
	{
		public static string UnifyPath(this string target)
		{
			return target.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}
	}
}


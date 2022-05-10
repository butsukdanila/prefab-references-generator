using UnityEditor;

using UnityEngine.UIElements;

namespace PrefabRefsGenerator.Utilities
{
	[CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
	public class SerializedDictionaryPropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var container = new VisualElement();
			return container;
		}
	}
}
using PrefabRefsGenerator.Utilities;

using UnityEngine;

namespace PrefabRefsGenerator
{
	public abstract class PrefabRefs : MonoBehaviour
	{
#if UNITY_EDITOR
		protected void ValidateRef<TComponent>(ref TComponent component, string parentName)
			where TComponent : Component
		{
			var parent = transform.FindChildRec(parentName);
			if (parent == null) return;
			component = parent.GetComponent<TComponent>();
		}
#endif
	}
}
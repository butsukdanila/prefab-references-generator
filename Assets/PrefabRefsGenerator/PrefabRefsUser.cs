using PrefabRefsGenerator.Utilities;

using UnityEngine;

namespace PrefabRefsGenerator
{
	public abstract class PrefabRefsUser<TRefs> : MonoBehaviour
		where TRefs : PrefabRefs
	{
		[field: SerializeField]
		protected TRefs refs { get; private set; }

#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			refs = refs == null ? gameObject.RequireComponent<TRefs>() : refs;
		}
#endif
	}
}


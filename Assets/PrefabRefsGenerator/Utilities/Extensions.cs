using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PrefabRefsGenerator.Utilities
{
	public static class Extensions
	{
		public static IEnumerable<GameObject> ChildrenDepthFirstTraversal(this GameObject target)
		{
			var stack = new Stack<IEnumerator>();
			var enumerator = target.transform.GetEnumerator();

			while (true)
			{
				if (enumerator.MoveNext())
				{
					var element = enumerator.Current as Transform;
					yield return element.gameObject;

					stack.Push(enumerator);
					enumerator = element.GetEnumerator();
					continue;
				}

				if (stack.Count > 0)
				{
					enumerator = stack.Pop();
					continue;
				}

				yield break;
			}
		}

		public static Transform FindChildRec(this Transform target, string name)
		{
			foreach (Transform child in target)
			{
				if (child.name == name)
					return child;

				var found = child.FindChildRec(name);
				if (found != null)
					return found;
			}
			return null;
		}

		public static TComponent RequireComponent<TComponent>(this GameObject target)
			where TComponent : Component
		{
			if (target.TryGetComponent<TComponent>(out var component)) return component;
			return target.AddComponent<TComponent>();
		}
	}
}

using System;

using UnityEngine;

namespace PrefabRefsGenerator.Utilities
{
	[Serializable]
	public class SerializedType : ISerializationCallbackReceiver
	{
		[SerializeField] private string m_assemblyQualifiedName;

		private Type m_type;

		public void OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(m_assemblyQualifiedName)) return;
			m_type = Type.GetType(m_assemblyQualifiedName);
		}

		public void OnBeforeSerialize()
		{
			if (m_type == null) return;
			m_assemblyQualifiedName = m_type.AssemblyQualifiedName;
		}

		public static implicit operator Type(SerializedType t)
		{
			return t?.m_type;
		}
	}
}
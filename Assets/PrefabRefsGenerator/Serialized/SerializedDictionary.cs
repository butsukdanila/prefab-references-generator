using System;
using System.Collections.Generic;

using UnityEngine;

namespace PrefabRefsGenerator.Utilities
{
	[Serializable]
	public class SerializedDictionary<K, V> : SerializedDictionary<K, V, K, V>
	{
		public override K SerializeKey(K key) { return key; }
		public override V SerializeValue(V value) { return value; }
		public override K DeserializeKey(K key) { return key; }
		public override V DeserializeValue(V value) { return value; }
	}

	[Serializable]
	public abstract class SerializedDictionary<K, V, SK, SV> : Dictionary<K, V>, ISerializationCallbackReceiver
	{
		[SerializeField] private List<SK> m_keys = new();
		[SerializeField] private List<SV> m_values = new();

		public abstract SK SerializeKey(K key);
		public abstract SV SerializeValue(V value);
		public abstract K DeserializeKey(SK serializedKey);
		public abstract V DeserializeValue(SV serializedValue);

		public void OnBeforeSerialize()
		{
			m_keys.Clear();
			m_values.Clear();

			foreach (var (key, value) in this)
			{
				m_keys.Add(SerializeKey(key));
				m_values.Add(SerializeValue(value));
			}
		}

		public void OnAfterDeserialize()
		{
			for (var i = 0; i < m_keys.Count; i++)
			{
				Add(DeserializeKey(m_keys[i]), DeserializeValue(m_values[i]));
			}

			m_keys.Clear();
			m_values.Clear();
		}
	}

}

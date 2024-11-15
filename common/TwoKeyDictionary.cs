using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.common
{
	public class TwoKeyDictionary<K1, K2, V> : Dictionary<K1, Dictionary<K2, V>>
	{

		public V this[K1 key1, K2 key2]
		{
			get
			{
				if (ContainsKey(key1, key2))
				{
					return base[key1][key2];
				}
				GD.PrintErr($"Warning: Key ({key1}, {key2}) not found in TwoKeyDictionary.");
				return default(V);
			}
			set
			{
				if (!ContainsKey(key1))
					this[key1] = new Dictionary<K2, V>();
				this[key1][key2] = value;
			}
		}

		public void Add(K1 key1, K2 key2, V value)
		{
			if (!ContainsKey(key1))
				this[key1] = new Dictionary<K2, V>();
			this[key1][key2] = value;
		}

		public bool ContainsKey(K1 key1, K2 key2)
		{
			return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
		}

		public new IEnumerable<V> Values
		{
			get
			{
				return from baseDict in base.Values
					   from baseKey in baseDict.Keys
					   select baseDict[baseKey];
			}

		}
	}
}

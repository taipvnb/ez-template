using System;
using System.Text;
using com.ez.engine.foundation;
using UnityEngine;

namespace com.ez.engine.loots.core
{
	[Serializable]
	public class LootParams
	{
		[SerializeField] private string _lootType;
		[SerializeField] private string[] _params;

		public string LootType => _lootType;

		public string[] Params => _params;

		public LootParams() { }

		public LootParams(string lootType, string[] param)
		{
			_lootType = lootType;
			_params = param;
		}

		public LootParams(string lootType, string rawData, char separator)
		{
			_lootType = lootType;
			_params = rawData.Split(separator);
		}

		public LootParams(string data, char separator = ';', int start = 0)
		{
			var splitData = data.Split(separator);
			_lootType = splitData[start];
			_params = splitData.Remove(splitData[start]);
		}

		public override string ToString()
		{
			var b = new StringBuilder("<b>--- Loot Params ---</b>\n");

			b.AppendFormat("<b>LootType:</b> {0}\n", _lootType);

			for (var i = 0; i < _params.Length; ++i)
			{
				b.AppendFormat("<b>Param:</b> {0} ", _params[i]);
			}

			b.AppendFormat("\n");
			return b.ToString();
		}
	}
}

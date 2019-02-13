using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	[System.Serializable]
	public class RuleConfiguration : ScriptableObject
	{
		private const string PROP_KEY = "blendrule_rule_config";

		[SerializeField] private BlendTile _tile;
		[SerializeField] private AdjBlendOption[] _adj_options;
		[SerializeField] private ushort[] _raw_adj_options;

		public void Init()
		{
			_tile = default(BlendTile);
			_adj_options = new AdjBlendOption[6];
			_raw_adj_options = new ushort[6];
			for (int d = 0; d < 6; d++)
			{
				this[d] = (AdjBlendOption)EditorPrefs.GetInt(PROP_KEY + d, 1);
			}
		}

		public ushort[] raw_options
		{
			get { return _raw_adj_options; }
		}

		public BlendTile tile
		{
			get { return _tile; }
			set
			{
				if (System.Enum.IsDefined(typeof(BlendTile), value))
				{
					_tile = value;
				}
				else
				{
					_tile = default(BlendTile);
				}
			}
		}

		public AdjBlendOption this[int index]
		{
			get
			{
				if (index < 0 || index > 5)
				{
					return AdjBlendOption.B;
				}
				return _adj_options[index];
			}
			set
			{
				if (index < 0 || index > 5)
				{
					return;
				}
				if (System.Enum.IsDefined(typeof(AdjBlendOption), value))
				{
					_adj_options[index] = value;
				}
				else
				{
					_adj_options[index] = AdjBlendOption.B;
				}
				_raw_adj_options[index] = (ushort)_adj_options[index];
			}
		}

		private void OnDestroy()
		{
			//save editor settings
			for (int d = 0; d < 6; d++)
			{
				EditorPrefs.SetInt(PROP_KEY + d, (int)_adj_options[d]);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendRule
{
	public class BlendRule : ScriptableObject
	{
		[SerializeField] private BlendTile _default_tile;
		[SerializeField] private BlendTile[] _blend_tiles;
		[SerializeField] private BlendRuleList[] _rule_lists;

		public void Init()
		{
			_default_tile = default(BlendTile);
			
			BlendTile[] tiles = (BlendTile[])System.Enum.GetValues(typeof(BlendTile));
			int index = 0;
			int count = Mathf.Max(tiles.Length - 1);

			_rule_lists = new BlendRuleList[count];
			_blend_tiles = new BlendTile[count];
			for (int k = 0;k < tiles.Length;k++)
			{
				if (tiles[k] != _default_tile)
				{
					_blend_tiles[index] = tiles[k];
					_rule_lists[index] = new BlendRuleList();
					index += 1;
				}
			}
		}

		public int BlendTileCount { get { return _blend_tiles.Length; } }
		public BlendTile DefaultTile { get { return _default_tile; } }
		public BlendTile[] BlendTiles { get { return _blend_tiles; } }

		public BlendTile ClassifyTileConfiguration(AdjBlendTile[] adj_config)
		{
			ushort[] generic_rule = ConvertAdjacentToBytes(adj_config);
			int tile_index = ClassifyTileConfiguration(generic_rule);
			if (tile_index < 0)
			{
				return _default_tile;
			}
			else
			{
				return _blend_tiles[tile_index];
			}
		}
		public int ClassifyTileConfiguration(ushort[] generic_rule)
		{
			for (int k = 0; k < _rule_lists.Length; k++)
			{
				if (_rule_lists[k].RuleExists(generic_rule))
				{
					return k;
				}
			}
			return -1;
		}

		public TileConfiguration GetTileConfiguration(AdjBlendTile[] adj_config)
		{
			ushort[] explicit_rule = ConvertAdjacentToBytes(adj_config);
			TileConfiguration config = new TileConfiguration();

			int tile_index = ClassifyTileConfiguration(explicit_rule);
			if (tile_index < 0)
			{
				config.tile = _default_tile;
			}
			else
			{
				config.tile = _blend_tiles[tile_index];
			}
			config.shift = 0;

			if (config.tile == _default_tile)
			{
				return config;
			}
			
			BlendRuleElement element = _rule_lists[tile_index].GetElement(explicit_rule);
			//calculate shift
			config.shift = BlendRuleElement.RulesMatch(element.rule_options, explicit_rule, config.tile.UpperSides());

			return config;
		}
		
		public int BlendTileRuleCount(int index)
		{
			return _rule_lists[index].RuleCount;
		}

		public BlendTile GetBlendTile(int tile_index)
		{
			if (tile_index < 0 || tile_index >= _blend_tiles.Length)
			{
				return _default_tile;
			}
			else
			{
				return _blend_tiles[tile_index];
			}
		}

		public string GetRuleString(int tile_index, int rule_index)
		{
			if (tile_index < 0 || 
				rule_index < 0 || 
				tile_index >= _blend_tiles.Length || 
				rule_index >= _rule_lists[tile_index].RuleCount)
			{
				return "";
			}
			return _rule_lists[tile_index].GetElement(rule_index).RuleString();
		}

		public int BlendTileKeyIndex(BlendTile tile)
		{
			for (int k = 0;k < _blend_tiles.Length;k++)
			{
				if (_blend_tiles[k] == tile)
				{
					return k;
				}
			}
			return -1;
		}

		public bool AddRule(int tile_index, ushort[] generic_rule)
		{
			if (generic_rule == null || generic_rule.Length != 6)
			{
				return false;
			}
			if (tile_index < 0 || tile_index >= _blend_tiles.Length)
			{
				return false;
			}
			if (RuleExists(generic_rule))
			{
				return false;
			}
			//add rule to list
			_rule_lists[tile_index].AddRuleElement(generic_rule);
			return true;
		}

		public bool DeleteRule(int tile_index, int rule_index)
		{
			if (tile_index < 0 || tile_index >= _blend_tiles.Length)
			{
				return false;
			}
			if (rule_index < 0 || rule_index >= _rule_lists[tile_index].RuleCount)
			{
				return false;
			}
			//Delete rule from list
			if (!_rule_lists[tile_index].DeleteRuleElement(rule_index))
			{
				return false;
			}
			return true;
		}

		public bool RuleExists(ushort[] generic_rule)
		{
			for (int k = 0;k < _rule_lists.Length;k++)
			{
				if (_rule_lists[k].RuleExists(generic_rule)) {
					return true;
				}
			}
			return false;
		}

		private ushort[] ConvertAdjacentToBytes(AdjBlendTile[] adj_config)
		{
			ushort[] adj_bytes = new ushort[6];
			for (int d = 0;d < 6;d++)
			{
				adj_bytes[d] = (ushort)adj_config[d];
			}
			return adj_bytes;
		}
	}
}


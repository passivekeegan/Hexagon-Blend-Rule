using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	[System.Serializable]
	public class AdjacentConfiguration : ScriptableObject
	{
		private const string PROP_KEY = "blendrule_adj_config";

		[SerializeField] private int _shift;
		[SerializeField] private BlendTile _tile;
		[SerializeField] private AdjBlendTile[] _adj_tiles;

		public void Init()
		{
			_shift = 0;
			_tile = default(BlendTile);
			_adj_tiles = new AdjBlendTile[] {
				AdjBlendTile.B,AdjBlendTile.B,AdjBlendTile.B,
				AdjBlendTile.B,AdjBlendTile.B,AdjBlendTile.B
			};
			for (int d = 0; d < 6; d++)
			{
				this[d] = (AdjBlendTile)EditorPrefs.GetInt(PROP_KEY + d, 1);
			}
		}

		public int shift
		{
			get { return _shift; }
			set
			{
				_shift = Mathf.Clamp(value, 0, 5);
			}
		}

		public BlendTile tile
		{
			get { return _tile; }
			set
			{
				if (System.Enum.IsDefined(typeof(BlendTile),value))
				{
					_tile = value;
				}
				else
				{
					_tile = default(BlendTile);
				}
			}
		}

		public AdjBlendTile this[int index]
		{
			get {
				if (index < 0 || index > 5)
				{
					return AdjBlendTile.B;
				} 
				return _adj_tiles[index];
			}
			set
			{
				if (index < 0 || index > 5)
				{
					return;
				}
				if (System.Enum.IsDefined(typeof(AdjBlendTile), value))
				{
					_adj_tiles[index] = value;
				}
				else
				{
					_adj_tiles[index] = AdjBlendTile.B;
				}
			}
		}

		public void ShiftForward()
		{
			AdjBlendTile storage = _adj_tiles[5];
			for (int d = 5; d > 0; d--)
			{
				_adj_tiles[d] = _adj_tiles[d - 1];
			}
			_adj_tiles[0] = storage;
		}

		public void ShiftReverse()
		{
			AdjBlendTile storage = _adj_tiles[0];
			for (int d = 0; d < 5; d++)
			{
				_adj_tiles[d] = _adj_tiles[d + 1];
			}
			_adj_tiles[5] = storage;
		}

		public void Randomize()
		{
			int rand;
			int high_count = 0;
			int[] level_set = new int[6];
			for (int d = 0; d < 6; d++)
			{
				rand = Random.Range(0, 100);
				if (rand < 20)//B 20%
				{
					level_set[d] = 0;
				}
				else if (rand < 50)//L 30%
				{
					level_set[d] = -1;
				}
				else if (rand < 90)//H 40%
				{
					level_set[d] = 1;
					high_count += 1;
				}
				else//X 10%
				{
					level_set[d] = 2;
				}
			}
			int max_height = Mathf.Min(4, Random.Range(1, high_count + 1));
			List<int> high_heights = new List<int>(high_count);
			for (int d = 0; d < high_count; d++)
			{
				if (d < max_height)
				{
					high_heights.Add(d + 1);
				}
				else
				{
					high_heights.Add(Random.Range(0, max_height) + 1);
				}
			}

			for (int d = 0; d < 6; d++)
			{
				switch (level_set[d])
				{
					case -1:
						rand = Random.Range(0, 100);
						if (rand < 10)
						{
							_adj_tiles[d] = AdjBlendTile.lc;
						}
						else if (rand < 40)
						{
							_adj_tiles[d] = AdjBlendTile.L;
						}
						else
						{
							_adj_tiles[d] = AdjBlendTile.LC;
						}
						break;
					case 1:
						rand = Random.Range(0, high_heights.Count);
						switch (high_heights[rand])
						{
							case 2:
								_adj_tiles[d] = AdjBlendTile.H2;
								break;
							case 3:
								_adj_tiles[d] = AdjBlendTile.H3;
								break;
							case 4:
								_adj_tiles[d] = AdjBlendTile.H4;
								break;
							default:
								_adj_tiles[d] = AdjBlendTile.H1;
								break;
						}
						high_heights.RemoveAt(rand);
						break;
					case 2:
						_adj_tiles[d] = AdjBlendTile.X;
						break;
					default:
						rand = Random.Range(0, 100);
						if (rand < 25)
						{
							_adj_tiles[d] = AdjBlendTile.bc;
						}
						else if (rand < 35)
						{
							_adj_tiles[d] = AdjBlendTile.BC;
						}
						else
						{
							_adj_tiles[d] = AdjBlendTile.B;
						}
						break;
				}
			}
		}

		private void OnDestroy()
		{
			//save editor settings
			for (int d = 0; d < 6; d++)
			{
				EditorPrefs.SetInt(PROP_KEY + d, (int)_adj_tiles[d]);
			}
		}
	}
}

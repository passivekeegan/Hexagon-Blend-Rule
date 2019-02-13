using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendRule
{
	public class AdjacentConfigurationWrapper : ScriptableObject
	{
		[SerializeField] public AdjacentConfiguration config;

		public void Init()
		{
			config = ScriptableObject.CreateInstance<AdjacentConfiguration>();
			config.Init();
		}

		public int shift
		{
			get { return config.shift; }
			set { config.shift = value; }
		}

		public BlendTile tile
		{
			get { return config.tile; }
			set { config.tile = value; }
		}

		public AdjBlendTile this[int index]
		{
			get
			{
				return config[index];
			}
			set
			{
				config[index] = value;
			}
		}

		public void ShiftForward()
		{
			config.ShiftForward();
		}

		public void ShiftReverse()
		{
			config.ShiftReverse();
		}

		public void Randomize()
		{
			config.Randomize();
		}

		private void OnDestroy()
		{
			DestroyImmediate(config);
		}
	}
}

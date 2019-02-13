using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendRule
{
	public class RuleConfigurationWrapper : ScriptableObject
	{
		[SerializeField] public RuleConfiguration config;

		public void Init()
		{
			config = ScriptableObject.CreateInstance<RuleConfiguration>();
			config.Init();
		}

		public BlendTile tile
		{
			get { return config.tile; }
			set { config.tile = value; }
		}

		public ushort[] raw_options
		{
			get { return config.raw_options; }
		}

		public AdjBlendOption this[int index]
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

		private void OnDestroy()
		{
			DestroyImmediate(config);
		}
	}
}

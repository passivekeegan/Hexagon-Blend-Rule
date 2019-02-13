using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlendRule
{
	[System.Serializable]
	public class BlendRuleElement
	{
		[SerializeField] private ushort[] _element_options;


		public BlendRuleElement(ushort option1, ushort option2, ushort option3, ushort option4, ushort option5, ushort option6)
		{
			_element_options = new ushort[] {
				option1,
				option2,
				option3,
				option4,
				option5,
				option6
			};
		}

		public BlendRuleElement(BlendRuleElement other)
		{
			_element_options = new ushort[6];
			_element_options[0] = other._element_options[0];
			_element_options[1] = other._element_options[1];
			_element_options[2] = other._element_options[2];
			_element_options[3] = other._element_options[3];
			_element_options[4] = other._element_options[4];
			_element_options[5] = other._element_options[5];
		}

		public ushort[] rule_options
		{
			get { return _element_options; }
		}

		public int Compare(BlendRuleElement a, BlendRuleElement b)
		{
			for (int d = 0; d < 6; d++)
			{
				if (a._element_options[d] < b._element_options[d])
				{
					return 1;
				}
				else if (a._element_options[d] > b._element_options[d])
				{
					return -1;
				}
			}
			return 0;
		}

		public string RuleString()
		{
			int option_width = AdjBlendOptionMethods.OPTION_WIDTH;
			string rule_string = "";
			for (int d = 0; d < 6; d++)
			{
				AdjBlendOption option = (AdjBlendOption)_element_options[d];
				rule_string += System.Enum.GetName(typeof(AdjBlendOption), option).PadRight(option_width);
				if (d < 5)
				{
					rule_string += "  ";
				}
			}
			return rule_string;
		}
		

		public static string GetRuleKey(ushort[] explicit_rule)
		{
			string key = "";
			for (int d = 0; d < 6; d++)
			{
				key += ((AdjBlendTile)explicit_rule[d]).Key();
			}
			return key;
		}

		public static int RulesMatch(ushort[] match_rule, ushort[] shift_rule)
		{
			return RulesMatch(match_rule, shift_rule, 0);
		}
		public static int RulesMatch(ushort[] match_rule, ushort[] shift_rule, int reverse_offset)
		{
			if (match_rule == null || match_rule.Length != 6)
			{
				return -1;
			}
			if (shift_rule == null || shift_rule.Length != 6)
			{
				return -1;
			}
			if (reverse_offset < 0 || reverse_offset > 5)
			{
				return -1;
			}
			for (int shift_index = 0; shift_index < 6; shift_index++)
			{
				int forward = shift_index;
				int backward = MeshGeneration.D[5,shift_index];
				for (int match_index = 0; match_index < 6 && (forward >= 0 || backward >= 0); match_index++)
				{
					if (forward >= 0)
					{
						if ((shift_rule[forward] & match_rule[match_index]) != 0)
						{
							if (match_index < 5)
							{
								forward = MeshGeneration.D[1,forward];
							}
							else
							{
								return shift_index;
							}
						}
						else
						{
							forward = -1;
						}
					}
					if (backward >= 0)
					{
						if ((shift_rule[backward] & match_rule[match_index]) != 0)
						{
							if (match_index < 5)
							{
								backward = MeshGeneration.D[5,backward];
							}
							else
							{
								return MeshGeneration.ND[reverse_offset, shift_index];
							}
						}
						else
						{
							backward = -1;
						}
					}
				}
			}
			return -1;
		}
	}
}


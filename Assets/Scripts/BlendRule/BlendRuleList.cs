using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendRule
{
	[System.Serializable]
	public class BlendRuleList
	{
		[SerializeField] private List<BlendRuleElement> _rules;

		public BlendRuleList()
		{
			_rules = new List<BlendRuleElement>();
		}

		public int RuleCount
		{
			get { return _rules.Count; }
		}

		public BlendRuleElement GetElement(int index)
		{
			return _rules[index];
		}

		public BlendRuleElement GetElement(ushort[] generic_rule)
		{
			for (int k = 0; k < _rules.Count; k++)
			{
				int shift = BlendRuleElement.RulesMatch(generic_rule, _rules[k].rule_options);
				if (shift >= 0)
				{
					return _rules[k];
				}
			}
			return null;
		}

		public void AddRuleElement(ushort[] generic_rule)
		{
			_rules.Add(new BlendRuleElement(
				generic_rule[0],
				generic_rule[1],
				generic_rule[2],
				generic_rule[3],
				generic_rule[4],
				generic_rule[5]
			));
		}

		public bool DeleteRuleElement(int rule_index)
		{
			if (rule_index < 0 || rule_index >= _rules.Count)
			{
				return false;
			}
			_rules.RemoveAt(rule_index);
			return true;
		}

		public bool RuleExists(ushort[] generic_rule)
		{
			return (GetElement(generic_rule) != null);
		}
	}
}


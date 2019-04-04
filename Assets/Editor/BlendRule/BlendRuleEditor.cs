using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	[CustomEditor(typeof(BlendRule))]
	public class BlendRuleEditor : Editor
	{
		private const string BT_INDEX_KEY = "blendrule_bt_index";
		private const string RL_INDEX_KEY = "blendrule_rl_index";

		private BlendRule _blend_rule;
		private int _bt_index;
		private int _rl_index;
		private RuleConfigurationWrapper _config;
		private SerializedObject _rule_obj;
		private SerializedProperty _rule_prop;

		private void OnEnable()
		{
			_blend_rule = (BlendRule)target;
			//get blend tile selection index
			_bt_index = EditorPrefs.GetInt(BT_INDEX_KEY, 0);
			//get rule selection index
			_rl_index = EditorPrefs.GetInt(RL_INDEX_KEY, -1);
			//get add blend rule object
			_config = ScriptableObject.CreateInstance<RuleConfigurationWrapper>();
			_config.Init();

			_rule_obj = new SerializedObject(_config);
			_rule_prop = _rule_obj.FindProperty("config");
		}

		private void OnDisable()
		{
			//set blend tile selection index
			EditorPrefs.SetInt(BT_INDEX_KEY, _bt_index);
			//set rule selection index
			EditorPrefs.SetInt(RL_INDEX_KEY, _rl_index);
			//set add blend rule object
			_rule_prop.Dispose();
			_rule_obj.Dispose();
			DestroyImmediate(_config);
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Space(10);
			DrawBTPanel();
			GUILayout.Space(30);
			DrawRuleListPanel();
			GUILayout.Space(30);
			//DrawPermutationsPanel();
			EditorGUILayout.EndVertical();
		}

		private void DrawBTPanel()
		{
			EditorGUILayout.LabelField("Blend Tiles", BREditorStyles.SectionLabel);
			GUILayout.Space(4);
			DrawBTSelction();
		}

		private void DrawBTSelction()
		{
			BlendTile[] bt_tiles = _blend_rule.BlendTiles;
			string[] bt_names = BlendTileNames(bt_tiles);

			EditorGUILayout.LabelField("Blend Tile Selection", BREditorStyles.MiddleLabel);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUI.BeginChangeCheck();
			_bt_index = GUILayout.SelectionGrid(_bt_index, bt_names, 3, GUI.skin.button);
			if (EditorGUI.EndChangeCheck())
			{
				_rl_index = -1;
			}
			if (_bt_index < 0 || _bt_index >= bt_tiles.Length)
			{
				_config.tile = BlendTile.None;
			}
			else
			{
				_config.tile = bt_tiles[_bt_index];
			}
			
			EditorGUILayout.EndVertical();
		}

		private void DrawRuleListPanel()
		{
			//Rule List
			if (_bt_index >= 0 && _bt_index < _blend_rule.BlendTileCount)
			{
				EditorGUILayout.LabelField("Rules", BREditorStyles.SectionLabel);
				GUILayout.Space(4);
				DrawRuleList();
				GUILayout.Space(4);
				DrawRuleControl();
			}
		}

		private void DrawRuleList()
		{
			EditorGUILayout.LabelField("Rule List");
			EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));
			int rule_count = _blend_rule.BlendTileRuleCount(_bt_index);
			string[] rule_texts = new string[rule_count];
			for (int k = 0; k < rule_count; k++)
			{
				rule_texts[k] = _blend_rule.GetRuleString(_bt_index, k);
			}
			_rl_index = GUILayout.SelectionGrid(_rl_index, rule_texts, 1);
			EditorGUILayout.EndVertical();
		}

		private void DrawRuleControl()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(_blend_rule.RuleExists(_config.raw_options));
			if (GUILayout.Button("Add"))
			{
				Undo.RecordObject(_blend_rule, "Add Rule");
				_blend_rule.AddRule(_bt_index, _config.raw_options);
				_rl_index = -1;
			}
			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginDisabledGroup(InvalidBTIndex() || InvalidRLIndex());
			if (GUILayout.Button("Delete"))
			{
				//TODO UNDO
				Undo.RecordObject(_blend_rule, "Delete Rule");
				_blend_rule.DeleteRule(_bt_index, _rl_index);
				_rl_index = -1;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			if (_rule_prop != null)
			{
				EditorGUILayout.PropertyField(_rule_prop);
			}
			if (EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(_blend_rule);
			}
		}

		private string[] BlendTileNames(BlendTile[] tiles)
		{
			string[] names = new string[tiles.Length];
			for (int k = 0;k < tiles.Length;k++)
			{
				names[k] = System.Enum.GetName(typeof(BlendTile), tiles[k]);
			}
			return names;
		}

		private bool InvalidBTIndex()
		{
			return (_bt_index < 0) || (_bt_index >= _blend_rule.BlendTileCount);
		}
		private bool InvalidRLIndex()
		{
			return (_rl_index < 0) || (_rl_index >= _blend_rule.BlendTileRuleCount(_bt_index));
		}
	}
}

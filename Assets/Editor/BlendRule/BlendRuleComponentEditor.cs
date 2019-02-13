using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	[CustomEditor(typeof(BlendRuleComponent))]
	public class BlendRuleComponentEditor : Editor
	{
		private BlendRuleComponent _rulecomp;
		private AdjacentConfigurationWrapper _adj_config;
		private SerializedObject _adj_obj;
		private SerializedProperty _adj_prop;

		private void OnEnable()
		{
			_rulecomp = target as BlendRuleComponent;

			_adj_config = ScriptableObject.CreateInstance<AdjacentConfigurationWrapper>();
			_adj_config.Init();

			_adj_obj = new SerializedObject(_adj_config);
			_adj_prop = _adj_obj.FindProperty("config");
			//Regenerate mesh
			GenerateMesh();
		}
		private void OnDisable()
		{
			_adj_prop.Dispose();
			_adj_obj.Dispose();
			DestroyImmediate(_adj_config);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();
			_rulecomp.rule = (BlendRule) EditorGUILayout.ObjectField(_rulecomp.rule, typeof(BlendRule), true);
			if (_adj_prop != null)
			{
				EditorGUILayout.PropertyField(_adj_prop);
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Rotate");
				
				if (GUILayout.Button(" < ", GUILayout.ExpandWidth(false)))
				{
					//reverse shift
					_adj_config.ShiftReverse();
				}
				if (GUILayout.Button(" > ", GUILayout.ExpandWidth(false)))
				{
					//forward shift
					_adj_config.ShiftForward();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Randomize Configuration");
				if (GUILayout.Button(" Randomize ", GUILayout.ExpandWidth(false)))
				{
					//randomize tile configuration
					_adj_config.Randomize();
				}
				GUILayout.EndHorizontal();
			}
			if (EditorGUI.EndChangeCheck())
			{
				GenerateMesh();
			}
			serializedObject.ApplyModifiedProperties();
		}

		private void GenerateMesh()
		{
			if (_rulecomp == null || _adj_config == null)
			{
				return;
			} 
			//load configuration into component
			for (int d = 0; d < 6; d++)
			{
				_rulecomp.adj_tiles[d] = _adj_config[d];
			}
			_rulecomp.GenerateMesh();
			_adj_config.tile = _rulecomp.tile;
			_adj_config.shift = _rulecomp.shift;
		}
	}
}

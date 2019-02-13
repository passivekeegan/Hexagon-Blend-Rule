using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	public class MakeBlendRule
	{
		[MenuItem("Assets/Create/Hexagon/Blend Rule")]
		public static void CreateBlendRule()
		{
			BlendRule rule = ScriptableObject.CreateInstance<BlendRule>();
			rule.Init();

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path.Length == 0)
			{
				path = "Assets";
			}
			else if (System.IO.Path.GetExtension(path).Length > 0)
			{
				path = path.Replace("/" + System.IO.Path.GetFileName(path), "");
			}

			path = AssetDatabase.GenerateUniqueAssetPath(path + "/blendrule.asset");
			AssetDatabase.CreateAsset(rule, path);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();

			Selection.activeObject = rule;
		}
	}
}

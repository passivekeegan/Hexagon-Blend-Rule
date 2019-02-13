using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	[CustomPropertyDrawer(typeof(AdjacentConfiguration))]
	public class AdjacentConfigurationDrawer : PropertyDrawer
	{
		private const float ADJ_LENGTH = 40f;
		private const float HORZ_FACTOR = 0.866f;
		private const float DIAG_FACTOR = 0.708f;
		private const float UNIT_LENGTH = 40f;
		private const float POINT_LENGTH = 10f;
		private const float HORZ_OFFSET = 1f;
		private const float VERT_OFFSET = 4f;

		private Vector2[] point_center = new Vector2[] {
			new Vector2(HORZ_FACTOR * UNIT_LENGTH, -UNIT_LENGTH / 2f),
			new Vector2(HORZ_FACTOR * UNIT_LENGTH, UNIT_LENGTH / 2f),
			new Vector2(0f, UNIT_LENGTH),
			new Vector2(-HORZ_FACTOR * UNIT_LENGTH, UNIT_LENGTH / 2f),
			new Vector2(-HORZ_FACTOR * UNIT_LENGTH, -UNIT_LENGTH / 2f),
			new Vector2(0f, -UNIT_LENGTH)
		};

		private Vector2[] adj_center = new Vector2[] {
			new Vector2(2 * HORZ_FACTOR * UNIT_LENGTH, 0f),
			new Vector2(HORZ_FACTOR * UNIT_LENGTH, 1.5f * UNIT_LENGTH),
			new Vector2(-HORZ_FACTOR * UNIT_LENGTH, 1.5f * UNIT_LENGTH),
			new Vector2(-2 * HORZ_FACTOR * UNIT_LENGTH, 0f),
			new Vector2(-HORZ_FACTOR * UNIT_LENGTH, -1.5f * UNIT_LENGTH),
			new Vector2(HORZ_FACTOR * UNIT_LENGTH, -1.5f * UNIT_LENGTH)
		};

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			AdjacentConfiguration obj = property.objectReferenceValue as AdjacentConfiguration;
			float center_y = position.y + position.height / 2f;
			float center_x = position.x + position.width / 2f;

			GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
			style.stretchHeight = true;
			style.stretchWidth = true;
			style.fixedHeight = 0;
			style.fixedWidth = 0;

			EditorGUI.BeginProperty(position, label, property);
			//draw hexagon blend tile orientation
			BlendTile tile = obj.tile;
			Rect point_rect = new Rect(0, 0, POINT_LENGTH, POINT_LENGTH);
			Rect adj_rect = new Rect(0, 0, ADJ_LENGTH, ADJ_LENGTH);
			for (int d = 0; d < 6; d++)
			{
				//calculate hexagon rect positions
				point_rect.x = center_x + point_center[d].x - (POINT_LENGTH / 2f);
				point_rect.y = center_y + point_center[d].y - (POINT_LENGTH / 2f);
				adj_rect.x = center_x + adj_center[d].x - (ADJ_LENGTH / 2f) + HORZ_OFFSET;
				adj_rect.y = center_y + adj_center[d].y - (ADJ_LENGTH / 2f) + VERT_OFFSET;
				//draw uninteractive toggle
				int hpoint_index = MeshGeneration.ND[obj.shift, d];
				EditorGUI.Toggle(point_rect, tile.IsHighPoint(hpoint_index), EditorStyles.radioButton);
				//draw adjacent blend option popup
				obj[d] = (AdjBlendTile)EditorGUI.EnumPopup(adj_rect, obj[d], style);
			}
			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (3 * UNIT_LENGTH) + ADJ_LENGTH + 10f;
		}
	}
}

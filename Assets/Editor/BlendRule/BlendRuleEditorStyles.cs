using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlendRule
{
	public static class BREditorStyles
	{
		public static Color TextColour { get; private set; }
		public static Color HeaderColour { get; private set; }
		public static Texture2D HeaderBackground { get; private set; }

		public static GUIStyle MainLabel { get; private set; }

		public static GUIStyle SectionLabel { get; private set; }
		public static GUIStyle SectionPanel { get; private set; }
		public static GUIStyle MiddleLabel { get; private set; }
		public static GUIStyle MiddleRightLabel { get; private set; }

		static BREditorStyles()
		{
			TextColour = new Color(38f / 255f, 38f / 255f, 38f / 255f, 1);
			HeaderColour = new Color(0.8f, 0.8f, 0.8f, 1f);
			HeaderBackground = ColourImage(new Color(0.2f, 0.2f, 0.2f, 1f));


			MainLabel = GetMainLabel();
			SectionLabel = GetSectionLabel();
			SectionPanel = GetSectionPanel();
			MiddleLabel = new GUIStyle(EditorStyles.label);
			MiddleLabel.alignment = TextAnchor.MiddleLeft;
			MiddleRightLabel = new GUIStyle(EditorStyles.label);
			MiddleRightLabel.alignment = TextAnchor.MiddleRight;
		}

		public static GUIStyle GetMainLabel()
		{
			GUIStyle style = new GUIStyle(EditorStyles.label);
			style.fontSize = 12;
			style.fixedHeight = 18;
			style.fontStyle = FontStyle.Bold;
			return style;
		}
		public static GUIStyle GetSectionLabel()
		{
			GUIStyle style = new GUIStyle(EditorStyles.largeLabel);
			style.fontStyle = FontStyle.Bold;
			return style;
		}
		public static GUIStyle GetSectionPanel()
		{
			GUIStyle style = new GUIStyle(EditorStyles.helpBox);
			return style;
		}




		public static GUIStyle GetLabel(TextAnchor anchor, Color colour, Color background)
		{
			return GetLabel(anchor, colour, ColourImage(background));
		}
		public static GUIStyle GetLabel(TextAnchor anchor, Color colour, Texture2D background)
		{
			GUIStyle style = new GUIStyle(EditorStyles.label);
			style.alignment = anchor;
			style.padding = new RectOffset(6, 6, 0, 0);

			ApplyAllTextColours(style, colour);
			ApplyAllBackgrounds(style, background);
			return style;
		}

		public static GUIStyle GetPanel(TextAnchor anchor, Color background)
		{
			return GetPanel(anchor, ColourImage(background));
		}
		public static GUIStyle GetPanel(TextAnchor anchor, Texture2D background)
		{
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleLeft;
			style.border = new RectOffset(0, 0, 0, 0);
			style.padding = new RectOffset(0, 0, 0, 0);
			style.margin = new RectOffset(0, 0, 0, 0);
			ApplyAllBackgrounds(style, background);
			return style;
		}

		

		public static Texture2D ColourImage(Color colour)
		{
			Texture2D texture = new Texture2D(2,2, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.alphaIsTransparency = true;
			texture.SetPixels32(new Color32[] { colour, colour, colour, colour});
			texture.Apply();
			return texture;
		}


		private static void ApplyAllTextColours(GUIStyle style, Color colour)
		{
			style.normal.textColor = colour;
			style.onNormal.textColor = colour;
			style.hover.textColor = colour;
			style.onHover.textColor = colour;
			style.focused.textColor = colour;
			style.onFocused.textColor = colour;
			style.active.textColor = colour;
			style.onActive.textColor = colour;
		}

		private static void ApplyAllBackgrounds(GUIStyle style, Texture2D background)
		{
			style.normal.background = background;
			style.onNormal.background = background;
			style.hover.background = background;
			style.onHover.background = background;
			style.focused.background = background;
			style.onFocused.background = background;
			style.active.background = background;
			style.onActive.background = background;
		}
	}
}

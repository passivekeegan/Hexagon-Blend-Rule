using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendRule
{
	public enum BlendTile
	{
		None = 0,
		Single = 1,
		Double = 2,
		Triple = 3
	}

	public static class BlendTileMethods
	{
		public static int UpperSides(this BlendTile tile)
		{
			switch (tile)
			{
				case BlendTile.Single:
					return 1;
				case BlendTile.Double:
					return 2;
				case BlendTile.Triple:
					return 3;
				default:
					return 0;
			}
		}
		public static bool IsHighPoint (this BlendTile tile, int index)
		{
			if (index < 0 || index > 5)
			{
				return false;
			}
			switch(tile)
			{
				case BlendTile.Single:
					return (index < 2);
				case BlendTile.Double:
					return (index < 3);
				case BlendTile.Triple:
					return (index < 4);
				default:
					return false;
			}
		}
	}

	public enum AdjBlendTile : ushort
	{
		B = 1,
		BC = 2,
		bc = 4,
		L = 8,
		LC = 16,
		lc = 32,
		H1 = 64,
		H2 = 128,
		H3 = 256,
		H4 = 512,
		X = 1024
	}

	public static class AdjBlendTileMethods
	{
		public static char Key(this AdjBlendTile rule_flag)
		{
			switch (rule_flag)
			{
				case AdjBlendTile.BC:
					return 'b';
				case AdjBlendTile.bc:
					return 'c';
				case AdjBlendTile.L:
					return 'd';
				case AdjBlendTile.LC:
					return 'e';
				case AdjBlendTile.lc:
					return 'f';
				case AdjBlendTile.H1:
					return 'g';
				case AdjBlendTile.H2:
					return 'h';
				case AdjBlendTile.H3:
					return 'i';
				case AdjBlendTile.H4:
					return 'j';
				case AdjBlendTile.X:
					return 'k';
				default:
					return 'a';
			}
		}
		public static string Translate (string char_code)
		{
			string text = "";
			for (int k = 0;k < char_code.Length;k++)
			{
				switch (char_code[k])
				{
					case 'b':
						text += "BC";
						break;
					case 'c':
						text += "U-BC";
						break;
					case 'd':
						text += "L";
						break;
					case 'e':
						text += "LC";
						break;
					case 'f':
						text += "U-LC";
						break;
					case 'g':
						text += "H1";
						break;
					case 'h':
						text += "H2";
						break;
					case 'i':
						text += "H3";
						break;
					case 'j':
						text += "H4";
						break;
					case 'k':
						text += "X";
						break;
					default:
						text += "B";
						break;
				}
			}
			return text;
		}
	}

	public enum AdjBlendOption : ushort
	{
		B = 1,//0000 0000 0000 0001
		BC = 2,//0000 0000 0000 0010
		BB = 3,//0000 0000 0000 0011
		bc = 4,//0000 0000 0000 0100
		L = 8,//0000 0000 0000 1000
		LC = 16,//0000 0000 0001 0000
		BBLC = 19,//0000 0000 0001 0011
		lc = 32,//0000 0000 0010 0000
		bclc = 36,//0000 0000 0010 0100
		H1 = 64,//0000 0000 0100 0000
		H2 = 128,//0000 0000 1000 0000
		H3 = 256,//0000 0001 0000 0000
		H4 = 512,//0000 0010 0000 0000
		HH = 960,//0000 0011 1100 0000
		X = 1024,//0000 0100 0000 0000
		GEH4 = 1536,//0000 0110 0000 0000
		GEH3 = 1792,//0000 0111 0000 0000
		GEH2 = 1920,//0000 0111 1000 0000
		HHX = 1984,//0000 0111 1100 0000
		NH4 = 65023,//1111 1101 1111 1111
		NH3 = 65279,//1111 1110 1111 1111
		NH2 = 65407,//1111 1111 0111 1111
		NH1 = 65471,//1111 1111 1011 1111
		A = 65535 //1111 1111 1111 1111
	}

	public static class AdjBlendOptionMethods
	{
		public const int OPTION_WIDTH = 4;
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexagon
{
	public static class HV
	{
		public static float SQRT3 { get; private set; }
		public static float SQRT3D2 { get; private set; }
		public static float SQRT3D3 { get; private set; }
		public static float SQRT3D6 { get; private set; }
		public static int[,] D { get; private set; }
		public static int[,] ND { get; private set; }

		static HV()
		{
			SQRT3 = Mathf.Sqrt(3f);
			SQRT3D2 = SQRT3 / 2f;
			SQRT3D3 = SQRT3 / 3f;
			SQRT3D6 = SQRT3 / 6f;
			D = new int[,] {
				{ 0, 1, 2, 3, 4, 5 },//+0
				{ 1, 2, 3, 4, 5, 0 },//+1
				{ 2, 3, 4, 5, 0, 1 },//+2
				{ 3, 4, 5, 0, 1, 2 },//+3
				{ 4, 5, 0, 1, 2, 3 },//+4
				{ 5, 0, 1, 2, 3, 4 } //+5
			};
			ND = new int[,] {
				{ 0, 1, 2, 3, 4, 5 },//-0
				{ 5, 0, 1, 2, 3, 4 },//-1
				{ 4, 5, 0, 1, 2, 3 },//-2
				{ 3, 4, 5, 0, 1, 2 },//-3
				{ 2, 3, 4, 5, 0, 1 },//-4
				{ 1, 2, 3, 4, 5, 0 } //-5
			};
		}
	}
}


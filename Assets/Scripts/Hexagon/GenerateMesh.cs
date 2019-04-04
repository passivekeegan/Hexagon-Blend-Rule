using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlendRule;

namespace Hexagon
{
	public static class GenerateMesh
	{
		//public
		public static int[] SideTriangles { get; private set; }
		public static Vector3[] SurfaceVertex { get; private set; }
		public static Vector3[] BaseVertex { get; private set; }
		public static Vector3[] AdjCenters { get; private set; }

		//private
		private const float INNER_WEIGHT_A = 0.8f;
		private const float INNER_WEIGHT_B = 0.15f;
		private const float INNER_WEIGHT_C = 0.05f;
		private const float INNER_SMOOTH_FACTOR = 0.6f;

		static GenerateMesh()
		{
			SurfaceVertex = new Vector3[] {
				new Vector3(0,0,0), //center vertex
				new Vector3(HV.SQRT3D3,0,0),//inner vertex
				new Vector3(HV.SQRT3D6,0,-0.5f),
				new Vector3(-HV.SQRT3D6,0,-0.5f),
				new Vector3(-HV.SQRT3D3,0,0),
				new Vector3(-HV.SQRT3D6,0,0.5f),
				new Vector3(HV.SQRT3D6,0,0.5f),
				new Vector3(HV.SQRT3D2,0,0.5f),//outer
				new Vector3(HV.SQRT3D2,0,-0.5f),
				new Vector3(0,0,-1),
				new Vector3(-HV.SQRT3D2,0,-0.5f),
				new Vector3(-HV.SQRT3D2,0,0.5f),
				new Vector3(0,0,1)
			};
			BaseVertex = new Vector3[] {
				SurfaceVertex[7],
				SurfaceVertex[8],
				SurfaceVertex[9],
				SurfaceVertex[10],
				SurfaceVertex[11],
				SurfaceVertex[12]
			};
			AdjCenters = new Vector3[]
			{
				new Vector3(HV.SQRT3,0,0),
				new Vector3(HV.SQRT3D2,0,-1.5f),
				new Vector3(-HV.SQRT3D2,0,-1.5f),
				new Vector3(-HV.SQRT3,0,0),
				new Vector3(-HV.SQRT3D2,0,1.5f),
				new Vector3(HV.SQRT3D2,0,1.5f)
			};
		}

		public static float CenterVertexHeight(float[] inner_heights)
		{
			if (inner_heights == null)
			{
				throw new System.ArgumentNullException("inner_heights");
			}
			if (inner_heights.Length != 6)
			{
				throw new System.ArgumentException("Float array length must be 6.", "inner_heights");
			}
			float height = 0;
			//calculate height
			for (int d = 0; d < 6; d++)
			{
				height += inner_heights[d];
			}
			return height / 6f;
		}
		public static float[] InnerVertexHeights(float[] outer_heights)
		{
			if (outer_heights == null)
			{
				throw new System.ArgumentNullException("outer_heights");
			}
			if (outer_heights.Length != 6)
			{
				throw new System.ArgumentException("Float array length must be 6.", "outer_heights");
			}
			float[] heights = new float[6];
			float[] heights_copy = new float[6];
			for (int d = 0; d < 6; d++)
			{
				//calculate height
				float a = INNER_WEIGHT_A * 0.5f * (outer_heights[d] + outer_heights[HV.D[1, d]]);
				float b = INNER_WEIGHT_B * 0.5f * (outer_heights[HV.D[2, d]] + outer_heights[HV.D[5, d]]);
				float c = INNER_WEIGHT_C * 0.5f * (outer_heights[HV.D[3, d]] + outer_heights[HV.D[4, d]]);
				heights[d] = a + b + c;
				heights_copy[d] = heights[d];
			}
			for (int d = 0; d < 6; d++)
			{
				float smoothed_height = (
					outer_heights[d] + outer_heights[HV.D[1, d]] + 
					heights_copy[HV.D[1, d]] + heights_copy[HV.D[5, d]]
				) / 4f;
				heights[d] = Mathf.Lerp(heights[d], smoothed_height, INNER_SMOOTH_FACTOR);
			}

			return heights;
		}
		public static float[] OuterVertexHeights(BlendTile tile, int shift, int adj_height)
		{
			return OuterVertexHeights(tile, shift, adj_height, adj_height);
		}
		public static float[] OuterVertexHeights(BlendTile tile, int shift, int adj_height_l, int adj_height_r)
		{
			if (shift < 0 || shift >= 6)
			{
				throw new System.ArgumentOutOfRangeException("shift", "Arguments range is [0-5].");
			}
			float[] heights = new float[] { 0, 0, 0, 0, 0, 0 };
			//calculate height
			switch (tile)
			{
				case BlendTile.Single:
					heights[shift] = adj_height_l;
					heights[HV.D[1, shift]] = adj_height_l;
					break;
				case BlendTile.Double:
					heights[shift] = adj_height_l;
					heights[HV.D[1, shift]] = adj_height_l;
					heights[HV.D[2, shift]] = adj_height_l;
					break;
				case BlendTile.Triple:
					heights[shift] = adj_height_l;
					heights[HV.D[1, shift]] = adj_height_l;
					heights[HV.D[2, shift]] = adj_height_r;
					heights[HV.D[3, shift]] = adj_height_r;
					break;
			}
			return heights;
		}
		public static Vector3[] CalculateSurfaceVertices(Vector3 center, int height, float[] outer_heights)
		{
			if (outer_heights == null)
			{
				throw new System.ArgumentNullException("outer_heights");
			}
			if (outer_heights.Length != 6)
			{
				throw new System.ArgumentException("Float array length must be 6.", "outer_heights");
			}
			Vector3[] vertices = (Vector3[])SurfaceVertex.Clone();
			//add center and apply heights to outer vertices
			for (int d = 0; d < 6; d++)
			{
				vertices[7 + d] = center + vertices[7 + d];
				vertices[7 + d].y = height + outer_heights[d];
			}
			//add center and apply heights to inner vertices
			float[] inner_heights = InnerVertexHeights(outer_heights);
			for (int d = 0; d < 6; d++)
			{
				vertices[1 + d] = center + vertices[1 + d];
				vertices[1 + d].y = height + inner_heights[d];
			}
			//add center and apply height to center vertex
			float center_height = CenterVertexHeight(inner_heights);
			vertices[0] = center + vertices[0];
			vertices[0].y = height + center_height;

			return vertices;
		}
		public static Vector3[] CalculateBaseVertices(Vector3 center, int height)
		{
			Vector3[] vertices = (Vector3[])BaseVertex.Clone();
			for (int d = 0; d < 6; d++)
			{
				vertices[d] = center + vertices[d];
				vertices[d].y = height;
			}
			return vertices;
		}
		public static void AddHexagon(Vector3[] surface_vertex, Vector3[] base_vertex, List<Vector3> vertices, List<int> triangles)
		{
			//add surface vertices
			int vertex_index = vertices.Count;
			//inner vertices
			for (int d = 0; d < 6; d++)
			{
				vertices.Add(surface_vertex[0]);
				vertices.Add(surface_vertex[1 + d]);
				vertices.Add(surface_vertex[1 + HV.D[1, d]]);
			}
			//corner vertices
			for (int d = 0; d < 6; d++)
			{
				vertices.Add(surface_vertex[1 + d]);
				vertices.Add(surface_vertex[7 + HV.D[1, d]]);
				vertices.Add(surface_vertex[1 + HV.D[1, d]]);
			}
			//outer vertices
			for (int d = 0; d < 6; d++)
			{
				vertices.Add(surface_vertex[1 + d]);
				vertices.Add(surface_vertex[7 + d]);
				vertices.Add(surface_vertex[7 + HV.D[1, d]]);
			}
			//add surface triangles
			for (int k = 0; k < 54; k++)
			{
				triangles.Add(vertex_index + k);
			}
			//add side vertices
			vertex_index = vertices.Count;
			for (int d = 0; d < 6; d++)
			{
				vertices.Add(surface_vertex[7 + d]);
				vertices.Add(surface_vertex[7 + HV.D[1, d]]);
				vertices.Add(base_vertex[d]);
				vertices.Add(base_vertex[HV.D[1, d]]);
			}
			//add side triangles
			for (int k = 0; k < SideTriangles.Length; k++)
			{
				triangles.Add(vertex_index + SideTriangles[k]);
			}
		}
	}
}


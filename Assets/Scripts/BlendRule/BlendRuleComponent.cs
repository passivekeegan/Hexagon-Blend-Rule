using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlendRule
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class BlendRuleComponent : MonoBehaviour
	{
		[SerializeField] public BlendRule rule;
		[SerializeField] public AdjBlendTile[] adj_tiles;

		[SerializeField] private int _shift;
		[SerializeField] private BlendTile _tile;

		public int shift
		{
			get { return _shift; }
		}
		public BlendTile tile
		{
			get { return _tile; }
		}

		private void Reset()
		{
			//Create component
			_tile = BlendTile.None;
			_shift = 0;
			adj_tiles = new AdjBlendTile[] {
				AdjBlendTile.B,
				AdjBlendTile.B,
				AdjBlendTile.B,
				AdjBlendTile.B,
				AdjBlendTile.B,
				AdjBlendTile.B
			};
			GenerateMesh();
		}

		public void GenerateMesh()
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();

			//add center hexagon
			int base_height = 2;
			int adj_height_l = 0;
			int adj_height_r = 0;
			_tile = BlendTile.None;
			_shift = 0;
			if (rule != null)
			{
				TileConfiguration config = rule.GetTileConfiguration(adj_tiles);
				_tile = config.tile;
				_shift = config.shift;
				adj_height_l = 1;
				adj_height_r = 1;
			}
			
			float[] corners = MeshGeneration.OuterVertexHeights(_tile, _shift, adj_height_l, adj_height_r);
			Vector3[] surface_vertex = MeshGeneration.CalculateSurfaceVertices(Vector3.zero, base_height, corners);
			Vector3[] base_vertex = MeshGeneration.CalculateBaseVertices(Vector3.zero, 0);
			MeshGeneration.AddHexagon(surface_vertex, base_vertex, vertices, triangles);

			//add adjacent hexagons
			for (int d = 0;d < 6;d++)
			{
				BlendTile adj_tile = ClassifyAdjacentTile(adj_tiles[d]);
				int adj_shift = CalculateAdjacentShift(adj_tile, adj_tiles[d], d);
				int adj_height = ClassifyAdjacentHeight(adj_tile);
				int height = AdjacentHexagonHeight(adj_tiles[d]);
				corners = MeshGeneration.OuterVertexHeights(adj_tile, adj_shift, adj_height);
				Vector3 center = MeshGeneration.AdjCenters[d];
				surface_vertex = MeshGeneration.CalculateSurfaceVertices(center, base_height + height, corners);
				base_vertex = MeshGeneration.CalculateBaseVertices(center, 0);
				MeshGeneration.AddHexagon(surface_vertex, base_vertex, vertices, triangles);
			}

			Mesh mesh = new Mesh();
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.UploadMeshData(false);

			SetMesh(mesh);
		}

		private static int CalculateLeftHeight()
		{
			return 0;
		}

		private static int CalculateRightHeight()
		{
			return 0;
		}

		private static int AdjacentHexagonHeight(AdjBlendTile adj_tile)
		{
			int height = 0;
			switch (adj_tile)
			{
				case AdjBlendTile.L:
				case AdjBlendTile.LC:
				case AdjBlendTile.lc:
					height = -1;
					break;
				case AdjBlendTile.H1:
					height = 1;
					break;
				case AdjBlendTile.H2:
					height = 2;
					break;
				case AdjBlendTile.H3:
					height = 3;
					break;
				case AdjBlendTile.H4:
					height = 4;
					break;
				case AdjBlendTile.X:
					height = 6;
					break;
			}
			return height;
		}

		private static BlendTile ClassifyAdjacentTile(AdjBlendTile adj_tile)
		{
			BlendTile tile = BlendTile.None;
			switch(adj_tile)
			{
				case AdjBlendTile.BC:
					tile = BlendTile.Triple;
					break;

				case AdjBlendTile.bc:
				case AdjBlendTile.LC:
				case AdjBlendTile.lc:
					tile = BlendTile.Single;
					break;
			}
			return tile;
		}
		private static int ClassifyAdjacentHeight(BlendTile tile)
		{
			int height = 0;
			if (tile != BlendTile.None)
			{
				height = 1;
			}
			return height;
		}

		private static int CalculateAdjacentShift(BlendTile tile, AdjBlendTile adj_tile, int d)
		{
			if (tile == BlendTile.Triple)
			{
				return MeshGeneration.D[5,d];
			}
			else if (tile == BlendTile.Single)
			{
				if (adj_tile == AdjBlendTile.bc || adj_tile == AdjBlendTile.lc)
				{
					return MeshGeneration.D[4,d];
				}
				else
				{
					return MeshGeneration.D[3,d];
				}
			}
			else
			{
				return 0;
			}
		}

		private void SetMesh(Mesh mesh)
		{
			GetComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer renderer = GetComponent<MeshRenderer>();
			if (renderer.sharedMaterial == null)
			{
				renderer.sharedMaterial = new Material(Shader.Find("Standard"));
			}
		}
	}
}

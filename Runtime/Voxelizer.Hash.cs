using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Voxell.Mathx;

namespace Voxell.Physics.Shatter
{
  public partial class Voxelizer
  {
    [BurstCompile]
    private struct HashTrianglesJob : IJobParallelFor
    {
      public Grid3D grid3D;
      public float3 minBound;

      /// <summary>Normal direction of a grid cell that contains a triangle.</summary>
      [WriteOnly, NativeDisableParallelForRestriction]
      public NativeArray<Voxel> voxels;
      /// <summary>Mesh vertices.</summary>
      [ReadOnly, NativeDisableParallelForRestriction]
      public NativeArray<float3> vertices;
      /// <summary>Triangle indices.</summary>
      [ReadOnly, NativeDisableParallelForRestriction]
      public NativeArray<int> triangles;

      /// <summary>Initialize data to hash the grid index that are related to each triangles.</summary>
      /// <param name="grid3D">the 3D grid configuration</param>
      /// <param name="vertices">mesh vertices</param>
      /// <param name="triangles">triangle indices</param>
      public HashTrianglesJob(
        ref Grid3D grid3D,
        float3 minBound,
        ref NativeArray<float3> vertices,
        ref NativeArray<int> triangles
      )
      {
        this.grid3D = grid3D;
        this.minBound = minBound;
        this.vertices = vertices;
        this.triangles = triangles;

        this.voxels = new NativeArray<Voxel>(grid3D.gridCount, Allocator.TempJob);
      }

      public void Execute(int index)
      {
        int tIdx = index*3;
        int t0 = triangles[tIdx];
        int t1 = triangles[tIdx + 1];
        int t2 = triangles[tIdx + 2];

        float3 v0 = vertices[t0];
        float3 v1 = vertices[t1];
        float3 v2 = vertices[t2];
        float3 minCoor = math.min(math.min(v0, v1), v2);
        float3 maxCoor = math.max(math.max(v0, v1), v2);

        int3 minGrid = MathUtil.PointToGrid(minCoor - minBound, grid3D.unitSize);
        int3 maxGrid = MathUtil.PointToGrid(maxCoor - minBound, grid3D.unitSize);

        for (int x=minGrid.x; x <= maxGrid.x; x++)
        {
          for (int y=minGrid.y; y <= maxGrid.y; y++)
          {
            for (int z=minGrid.z; z <= maxGrid.z; z++)
            {
              int3 gridPos = new int3(x, y, z);
              int gridIdx = grid3D.GetGridIdx(gridPos);
              Voxel voxel = voxels[gridIdx];
              voxel.frontFace = IsFrontFace(in v0, in v1, in v2);
            }
          }
        }
      }

      public bool IsFrontFace(in float3 v0, in float3 v1, in float3 v2)
      {
        float3 normal = math.cross(v1 - v0, v2 - v0);
        return math.dot(normal, float3x.forward) < 0.0f;
      }
    }
  }
}
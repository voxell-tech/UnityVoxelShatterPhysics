using Unity.Mathematics;

namespace Voxell.Physics.Shatter
{
  public partial class Voxelizer
  {
    private struct Triangle
    {
      public int i0, i1, i2;
      public Triangle(int i0, int i1, int i2, int startIdx)
      { this.i0 = i0 + startIdx; this.i1 = i1 + startIdx; this.i2 = i2 + startIdx; }
    }

    /// <summary>Structure data to hold the indices of a cube.</summary>
    /// <remarks>
    /// The normals should be in the format of:<br/>
    /// front^2, back^2, left^2, right^2, top^2, down^2
    /// </remarks>
    private struct Cube
    {
      public float3 v0, v1, v2, v3, v4, v5, v6, v7;
      public Triangle t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11;
      public Cube(
        float3 v0, float3 v1, float3 v2, float3 v3, float3 v4, float3 v5, float3 v6, float3 v7,
        Triangle t0, Triangle t1, Triangle t2, Triangle t3, Triangle t4, Triangle t5,
        Triangle t6, Triangle t7, Triangle t8, Triangle t9, Triangle t10, Triangle t11
      )
      {
        this.v0 = v0; this.v1 = v1; this.v2 = v2; this.v3 = v3;
        this.v4 = v4; this.v5 = v5; this.v6 = v6; this.v7 = v7;

        this.t0 = t0; this.t1 = t1; // front
        this.t2 = t2; this.t3 = t3; // back
        this.t4 = t4; this.t5 = t5; // left
        this.t6 = t6; this.t7 = t7; // right
        this.t8 = t8; this.t9 = t9; // top
        this.t10 = t10; this.t11 = t11; // down
      }
    }

    /// <summary>Generate voxel mesh based on local min-max bounds.</summary>
    private static Cube GenerateCube(float3 minBound, float3 maxBound, int startIdx)
    {
      float3 v0, v1, v2, v3, v4, v5, v6, v7;
      //         v1      v2
      //         =========
      // normal /| front |
      //     |/_ |       |
      //         =========
      //         v0      v3
      v0 = minBound;
      v1 = new float3(minBound.x, maxBound.y, minBound.z);
      v2 = new float3(minBound.x, maxBound.y, minBound.z);
      v3 = new float3(maxBound.x, minBound.y, minBound.z);

      // v5      v6
      // =========  __
      // | back  |  /|
      // |       |/ normal
      // =========
      // v4      v7
      v4 = new float3(minBound.x, minBound.y, maxBound.z);
      v5 = new float3(minBound.x, maxBound.y, maxBound.z);
      v6 = maxBound;
      v7 = new float3(maxBound.x, minBound.y, maxBound.z);

      Triangle t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11;
      // front
      t0 = new Triangle(0, 1, 2, startIdx);
      t1 = new Triangle(0, 2, 3, startIdx);
      // back
      t2 = new Triangle(4, 6, 5, startIdx);
      t3 = new Triangle(4, 7, 6, startIdx);
      // left
      t4 = new Triangle(1, 5, 0, startIdx);
      t5 = new Triangle(0, 5, 4, startIdx);
      // right
      t6 = new Triangle(2, 6, 3, startIdx);
      t7 = new Triangle(3, 6, 7, startIdx);
      // top
      t8 = new Triangle(1, 5, 6, startIdx);
      t9 = new Triangle(1, 6, 2, startIdx);
      // down
      t10 = new Triangle(0, 7, 4, startIdx);
      t11 = new Triangle(0, 3, 7, startIdx);

      return new Cube(
        v0, v1, v2, v3, v4, v5, v6, v7,
        t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11
      );
    }
  }
}
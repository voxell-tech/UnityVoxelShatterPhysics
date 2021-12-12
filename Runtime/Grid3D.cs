using Unity.Mathematics;
using Voxell.Mathx;

namespace Voxell.Physics.Shatter
{
  public struct Grid3D
  {
    public int Width => boundDiff.x;
    public int Height => boundDiff.y;

    public readonly float unitSize;
    public readonly int3 minBound;
    public readonly int3 maxBound;
    public readonly int3 boundDiff;
    public readonly int gridCount;


    public Grid3D(float unitSize, float3 minBound, float3 maxBound)
    {
      this.unitSize = unitSize;
      this.minBound = MathUtil.PointToGrid(minBound, unitSize);
      this.maxBound = MathUtil.PointToGrid(maxBound, unitSize);

      this.boundDiff = this.maxBound - this.minBound + 1;
      this.gridCount = boundDiff.x*boundDiff.y*boundDiff.z;
    }

    public int GetGridIdx(float3 p)
    {
      int3 g = MathUtil.PointToGrid(p, unitSize) - minBound;
      return g.x + g.y*Width + g.z*Width*Height;
    }
  }
}
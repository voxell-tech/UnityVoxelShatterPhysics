using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Physics.Shatter
{
  public partial class Voxelizer
  {
    private int _subMeshCount;
    private Grid3D _grid3D;
    private float3 _minBound, _maxBound;

    private NativeArray<int>[] _triangles;
    private NativeArray<float3>[] _vertices;

    public Voxelizer(ref Mesh mesh, ref Grid3D grid3D)
    {
      _subMeshCount = mesh.subMeshCount;
      _minBound = mesh.bounds.min;
      _maxBound = mesh.bounds.max;

      _triangles = new NativeArray<int>[_subMeshCount];
      _vertices = new NativeArray<float3>[_subMeshCount];

      using(Mesh.MeshDataArray dataArray = Mesh.AcquireReadOnlyMeshData(mesh))
      {
        for (int d=0, length=dataArray.Length; d < length; d++)
        {
          Mesh.MeshData meshData = dataArray[d];
          _vertices[d] = meshData.GetVertexData<float3>();
          meshData.GetIndices(_triangles[d], d);
        }
      }
    }
  }
}
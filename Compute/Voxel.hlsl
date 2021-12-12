#ifndef __VOXEL_COMMON_INCLUDED__
#define __VOXEL_COMMON_INCLUDED__

struct Voxel
{
  float3 position;
  float2 uv;
  bool fill;
  bool frontFace;
};

inline bool is_front_voxel(in Voxel v)
{ return v.fill && v.frontFace; }

inline bool is_back_voxel(in Voxel v)
{ return v.fill && !v.frontFace; }

inline bool is_empty_voxel(in Voxel v)
{ return !v.fill; }

#endif

using System;
using Enum;
using UnityEngine;

namespace Vo
{
  [Serializable]
  
  public struct ParticleEffectVo
  {
    public VFX Name;

    public ParticleSystem ParticleSystem;

    public int Count;
  }
}
using System;

namespace Vo
{
  [Serializable]
  public struct LevelStatsVo
  {
    public int Level;
    
    public int MeteorCount;
    
    public int MinHealth;
    
    public int MaxHealth;

    public bool IsBossLevel;
  }
}
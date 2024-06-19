using System;
using Level;
using UnityEngine;
using Vo;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    [SerializeField]
    private LevelData _levelData;

    public LevelStatsVo GetLevelData(int level)
    {
      LevelStatsVo newLevelStatsVo = level % 5 == 0 ? BossLevel(level) : NormalLevel(level);

      try
      {
        _levelData.LevelStatsList[level] = newLevelStatsVo;
      }
      catch (Exception)
      {
        _levelData.LevelStatsList.Add(newLevelStatsVo);
      }
      
      return newLevelStatsVo;
    }

    private LevelStatsVo BossLevel(int level)
    {
      LevelStatsVo newLevelStatsVo = new()
      {
        Level = level,
        MinHealth = level / 5 * 1000,
        MaxHealth = level / 5 * 1000,
        MeteorCount = 1,
        IsBossLevel = true
      };

      return newLevelStatsVo;
    }

    private LevelStatsVo NormalLevel(int level)
    {
      LevelStatsVo previousLevel = _levelData.LevelStatsList[level - 1];

      if (level > 5 && level % 5 == 1)
      {
        previousLevel = _levelData.LevelStatsList[level - 2];
      }
      
      int minIncrease = (int)Math.Ceiling((float)level / 5);
      int maxIncrease = (int)Math.Ceiling((float)level / 5) * 2;
      int meteorCount = (int)Math.Ceiling((float)level / 5);
      
      LevelStatsVo newLevelStatsVo = new()
      {
        Level = level,
        MinHealth = previousLevel.MinHealth + minIncrease,
        MaxHealth = previousLevel.MaxHealth + maxIncrease,
        MeteorCount = meteorCount,
        IsBossLevel = false
      };

      return newLevelStatsVo;
    }
  }
}
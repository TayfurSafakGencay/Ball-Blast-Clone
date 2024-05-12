using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vo;

namespace Managers
{
  public sealed partial class GameManager
  {
    public static T GetRandomElementFromList<T>(List<T> list)
    {
      Random random = new();
      int index = random.Next(0, list.Count);
      return list[index];
    }

    public static async Task Delay(float second)
    {
      await Task.Delay((int)(second * 1000));
    }
    
    // Getter - Setter
    public bool GetIsGameStarted()
    {
      return _isGameStarted;
    }

    public bool GetIsGameFinished()
    {
      return _isGameFinished;
    }

    public LevelStatsVo GetLevelStats()
    {
      return _levelStatsVo;
    }
  }
}
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
      return _levelData.LevelStatsList[level];
    }
  }
}
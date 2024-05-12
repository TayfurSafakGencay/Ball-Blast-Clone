using System.Collections.Generic;
using UnityEngine;
using Vo;

namespace Level
{
  [CreateAssetMenu(fileName = "Level", menuName = "Tools/Level", order = 0)]
  public class LevelData : ScriptableObject
  {
    public List<LevelStatsVo> LevelStatsList;
  }
}
using Enum;
using UnityEngine;
using Vo;

namespace Managers
{
  public static class SaveSystemManager
  {
    public static void SaveSkill(SkillStat skillStat)
    {
      PlayerPrefs.SetInt(skillStat.Key + "Level", skillStat.Level);
      PlayerPrefs.SetInt(skillStat.Key + "Cost", skillStat.Cost);
      PlayerPrefs.SetFloat(skillStat.Key + "Stat", skillStat.Stat);
    }

    public static SkillStat LoadSkill(SkillType key)
    {
      if (!PlayerPrefs.HasKey(key + "Level"))
      {
        SkillStat skillStat = new()
        {
          Key = key,
          Level = 1,
          Stat = 1,
          Cost = key == SkillType.Gold || key == SkillType.BulletCount ? 100 : 10
        };

        SaveSkill(skillStat);
      }

      SkillStat skillStatVo = new()
      {
        Key = key,
        Level = PlayerPrefs.GetInt(key + "Level"),
        Cost = PlayerPrefs.GetInt(key + "Cost"),
        Stat = PlayerPrefs.GetFloat(key + "Stat")
      };

      return skillStatVo;
    }

    public static void SaveLevel(int level)
    {
      PlayerPrefs.SetInt(_level, level);
    }

    private const string _level = "Level";
    
    public static int LoadLevel()
    {
      if (!PlayerPrefs.HasKey(_level))
      {
        SaveLevel(1);
      }

      return PlayerPrefs.GetInt(_level);
    }
    
    private const string _money = "Money";

    public static void SaveMoney(int money)
    {
      PlayerPrefs.SetInt(_money, money);
    }
    public static int LoadMoney()
    {
      if (!PlayerPrefs.HasKey(_money))
      {
        SaveMoney(0);
      }

      return PlayerPrefs.GetInt(_money);
    }
  }
}
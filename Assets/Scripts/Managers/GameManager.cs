using System;
using Enum;
using MeteorFeatures;
using UnityEngine;
using Vo;

namespace Managers
{
  public sealed partial class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    public LevelManager LevelManager;

    public EffectManager EffectManager;

    private LevelStatsVo _levelStatsVo;

    [Space(20)]
    public MeteorSpawner meteorSpawner;

    [Space(20)]
    public Camera Camera;

    [Space(20)]
    public Transform BulletStack;

    private bool _isGameStarted;

    private bool _isGameFinished;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      
      SetLevelData();
      InitialMoney();

      // SkillStat skillStat = new()
      // {
      //   Key = SkillType.BulletCount,
      //   Cost = 100,
      //   Level = 1,
      //   Stat = 1
      // };
      //
      // SkillStat skillStat2 = new()
      // {
      //   Key = SkillType.AttackSpeed,
      //   Cost = 95,
      //   Level = 18,
      //   Stat = 0.15f
      // };
      //
      // SaveSystemManager.SaveSkill(skillStat);
      // SaveSystemManager.SaveSkill(skillStat2);
      // SaveSystemManager.SaveLevel(29);
    }

    public static Action GameStarted;
    public void StartGame()
    {
      SetLevelData();
      
      _isGameStarted = true;
      _isGameFinished = false;  
      
      Time.timeScale = 1f;
      
      GameStarted.Invoke();
    }

    public void SetLevelData()
    {
      _levelStatsVo = LevelManager.GetLevelData(SaveSystemManager.LoadLevel());
    }

    public static Action<bool> GameFinished;
    public async void GameOver(bool success = false)
    {
      if (success)
      {
        EffectManager.PlayParticleEffect(new Vector2(0, -2), VFX.Firework);
        await Delay(2f);
      }
      else
      {
        await Delay(0.05f);
      }

      _isGameFinished = true;
      _isGameStarted = false;

      Time.timeScale = 0f;

      GameFinished?.Invoke(success);
      
      if (success)
      {
        SaveSystemManager.SaveLevel(SaveSystemManager.LoadLevel() + 1);
      }
    }

    public static Action<int, int> MeteorDestroyed;
    public void MeteorDestroy(int destroyedCount, int totalMeteor)
    {
      MeteorDestroyed?.Invoke(destroyedCount, totalMeteor);
    }

    #region Skill

    public SkillStat GetSkillStats(SkillType key)
    {
      return SaveSystemManager.LoadSkill(key);
    }

    public Action<SkillType> SkillUpdate;
    public void UpgradeSkillInvoke(SkillType key)
    {
      SkillUpdate?.Invoke(key);
    }

    public void UpgradeSkillSave(SkillType key, float stat)
    {
      SkillStat skillStat = GetSkillStats(key);
      
      RemoveMoney(skillStat.Cost);
      
      skillStat.Level++;
      skillStat.Stat = stat;

      switch (key)
      {
        case SkillType.AttackSpeed:
          skillStat.Cost += 405;
          break;
        case SkillType.AttackDamage:
          skillStat.Cost += 10;
          break;
        case SkillType.Gold:
          skillStat.Cost *= 2;
          break;
        case SkillType.BulletCount:
          skillStat.Cost *= 4;
          break;
      }
      
      SaveSystemManager.SaveSkill(skillStat);
    }
    
    #endregion

    #region Money

    public int Money;

    private void InitialMoney()
    {
      Money = SaveSystemManager.LoadMoney();
    }
    public int GetPlayerCoin()
    {
      return Money;
    }

    public Action<int, Vector3> CollectedMoney;

    public void MoneyCollected(int money, Vector3 position)
    {
      CollectedMoney.Invoke(money, position);
    }

    public void MoneyCollectedWithCoefficient(int money, Vector3 position)
    {
      Money += money;

      CollectedMoneyWithCoefficient.Invoke(money, position);
      TotalMoneyChanged.Invoke(Money);
    }

    public void RemoveMoney(int value)
    {
      Money -= value;
      
      TotalMoneyChanged.Invoke(Money);
    }

    public Action<int, Vector3> CollectedMoneyWithCoefficient;
    
    public Action<int> TotalMoneyChanged;
    
    #endregion

    private void OnApplicationQuit()
    {
      SaveSystemManager.SaveMoney(Money);
    }
  }
}
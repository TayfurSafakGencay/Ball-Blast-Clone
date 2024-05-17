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
      
      SaveSystemManager.SaveLevel(1);

      GameFinished += AddPlayerCoin;
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
    public void GameOver(bool success = false)
    {
      if (success)
      {
        SaveSystemManager.SaveLevel(SaveSystemManager.LoadLevel() + 1);
      }
      
      _isGameFinished = true;
      _isGameStarted = false;

      Time.timeScale = 0f;

      GameFinished?.Invoke(success);
    }

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
      
      skillStat.Level++;
      skillStat.Cost += key == SkillType.Gold || key == SkillType.BulletCount ? 100 : 10;
      skillStat.Stat = stat;
      
      SaveSystemManager.SaveSkill(skillStat);
    }

    public int GetPlayerCoin()
    {
      return SaveSystemManager.LoadMoney();
    }


    private int _moneyForThisTurn;
    public void MoneyForTurn(int money, Vector3 position)
    {
      _moneyForThisTurn += money;

      CollectedMoney.Invoke(money, position);
      CollectedMoneyForThisTurn.Invoke(_moneyForThisTurn);
    }

    public Action<int, Vector3> CollectedMoney;
    
    public Action<int> CollectedMoneyForThisTurn;

    public void AddPlayerCoin(bool success)
    {
      int newMoney = SaveSystemManager.LoadMoney() + _moneyForThisTurn;
      _moneyForThisTurn = 0;
      
      SaveSystemManager.SaveMoney(newMoney);
    }
  }
}
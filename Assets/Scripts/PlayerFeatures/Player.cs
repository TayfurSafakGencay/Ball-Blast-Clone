using Enum;
using Managers;
using UnityEngine;

namespace PlayerFeatures
{
  public class Player : MonoBehaviour
  {
    public PlayerStat PlayerStat { get; private set; }

    private GameManager _gameManager;

    private void Start()
    {
      _gameManager = GameManager.Instance;

      PlayerStat = new PlayerStat
      {
        AttackSpeed = _gameManager.GetSkillStats(SkillType.AttackSpeed).Stat,
        BulletCount = (int)_gameManager.GetSkillStats(SkillType.BulletCount).Stat,
        AttackDamage = (int)_gameManager.GetSkillStats(SkillType.AttackDamage).Stat,
        Gold = (int)_gameManager.GetSkillStats(SkillType.Gold).Stat,
      };
      
      _gameManager.SkillUpdate += UpgradeSkill;
    }

    private void UpgradeSkill(SkillType key)
    {
      float stat = 0;
      switch (key)
      {
        case SkillType.AttackDamage:
          PlayerStat.AttackDamage++;
          stat = PlayerStat.AttackDamage;
          break;
        case SkillType.AttackSpeed:
          PlayerStat.AttackSpeed -= 0.01f;
          stat = PlayerStat.AttackSpeed;
          break;
        case SkillType.BulletCount:
          PlayerStat.BulletCount++;
          stat = PlayerStat.BulletCount;
          break;
        case SkillType.Gold:
          PlayerStat.Gold++;
          stat = PlayerStat.Gold;
          break;
      }
      
      _gameManager.UpgradeSkillSave(key, stat);
    }
  }
}
using UnityEngine;

namespace PlayerFeatures
{
  public class Player : MonoBehaviour
  {
    public PlayerStat PlayerStat { get; private set; }

    private void Awake()
    {
      PlayerStat = new PlayerStat
      {
        AttackSpeed = 1,
        BulletCount = 1,
        BulletPenetration = 1,
        AttackDamage = 1,
      };
    }
  }
}
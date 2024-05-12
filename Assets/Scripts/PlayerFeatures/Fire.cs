using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace PlayerFeatures
{
  public class Fire : MonoBehaviour
  {
    private Player _player;
    
    [SerializeField]
    private Transform _aim;

    [SerializeField]
    private GameObject _missile;

    private Transform _missileStack;

    private readonly Queue<GameObject> _missileQueue = new();

    private float _attackTime;

    private float _attackSpeed;

    private int _bulletCount;
    
    private void Start()
    {
      _missileStack = GameManager.Instance.BulletStack;
      
      _player = GetComponent<Player>();
      _attackSpeed = _player.PlayerStat.AttackSpeed;
      _bulletCount = _player.PlayerStat.BulletCount;

      _attackTime = _attackSpeed;
    }

    private void Update()
    {
      _attackTime -= Time.deltaTime;

      if (_attackTime <= 0)
      {
        Attack();
      }
    }

    private void Attack()
    {
      if (_missileQueue.Count < _bulletCount)
      {
        AddNewMissileToQueue();
        Shoot();
      }
      else
      {
        Shoot();
      }
    }

    private void Shoot()
    {
      _attackTime = _attackSpeed;

      GameObject missile = _missileQueue.Dequeue();
      missile.transform.position = _aim.position;
      missile.SetActive(true);
      AttackAnimation();
    }

    private void AddNewMissileToQueue()
    {
      for (int i = 0; i < _bulletCount; i++)
      {
        GameObject missile = Instantiate(_missile, _aim.position, Quaternion.identity, _missileStack);
        _missileQueue.Enqueue(missile);
        missile.GetComponent<Missile>().SetFire(this);
        missile.SetActive(false);
      }
    }

    [Space(20)]
    [SerializeField]
    private GameObject _cannon;
    private void AttackAnimation()
    {
      _cannon.transform.DOLocalMoveY(1.1f, 0.1f)
        .SetEase(Ease.OutQuad) 
        .OnComplete(() =>
        {
          _cannon.transform.DOLocalMoveY(1.3f, 0.1f)
            .SetEase(Ease.InQuad);
        });
    }

    public void AddMissileToQueue(GameObject missile)
    {
      _missileQueue.Enqueue(missile);
    }

    public int GetBulletPenetration()
    {
      return _player.PlayerStat.BulletPenetration;
    }

    public int GetAttackDamage()
    {
      return _player.PlayerStat.AttackDamage;
    }
  }
}
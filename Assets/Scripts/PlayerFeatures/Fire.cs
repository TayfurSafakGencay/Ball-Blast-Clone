using System;
using System.Collections.Generic;
using DG.Tweening;
using Enum;
using Managers;
using Unity.Mathematics;
using UnityEngine;

namespace PlayerFeatures
{
  public class Fire : MonoBehaviour
  {
    private Player _player;
    
    [SerializeField]
    private Transform _aim;

    private List<Transform> _childAims = new();

    [SerializeField]
    private GameObject _missile;

    private Transform _missileStack;

    private readonly Queue<GameObject> _missileQueue = new();
    
    private float _attackTime;

    private float _attackSpeed;

    private int _bulletCount;

    private bool _gameStarted;

    private void Start()
    {
      _missileStack = GameManager.Instance.BulletStack;
      
      _player = GetComponent<Player>();

      AddNewMissileToQueue(15);

      GameManager.GameStarted += StartGame;
    }

    private void StartGame()
    {
      _bulletCount = _player.PlayerStat.BulletCount;
      _attackSpeed = _player.PlayerStat.AttackSpeed;
      _attackTime = _attackSpeed;
      
      _childAims.Clear();

      for (int i = 0; i < _aim.childCount; i++)
        DestroyImmediate(_aim.GetChild(i).gameObject);

      if (_bulletCount % 2 == 1) OddBulletCount();
      else EvenBulletCount();
      
      _gameStarted = true;
    }

    private void Update()
    {
      if(!_gameStarted) return;
      if (_bulletCount <= 0) return;
      if (_childAims.Count == 0) return;
      
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
        print("fffff");
        AddNewMissileToQueue(_bulletCount);
      }
      else
      {
        Shoot();
      }
    }

    private void Shoot()
    {
      _attackTime = _attackSpeed;

      for (int i = 0; i < _bulletCount; i++)
      {
        GameObject missile = _missileQueue.Dequeue();
        missile.name = "Missile: " + i;
        missile.transform.position = _childAims[i].position;
        missile.SetActive(true);
      }
      
      AttackAnimation();
    }

    private void OddBulletCount()
    {
      GameObject newAim = new();
      
      for (int j = 0; j < _bulletCount; j++)
      {
        GameObject newAimPoint = Instantiate(newAim, Vector3.zero, quaternion.identity, _aim);
        newAimPoint.name = "Aim: " + j + 1;
        _childAims.Add(newAimPoint.transform);

        if (j == 0)
        {
          newAimPoint.transform.localPosition = Vector3.zero;
          continue;
        }
        
        int x = (int)Math.Pow(-1, j);
        int y = (j + 1) / 2;
        float xPosition = x * y * 0.5f;

        float newXPosition = Vector3.zero.x + xPosition;
        newAimPoint.transform.localPosition = new Vector2(newXPosition, 0);
      }
    }

    private void EvenBulletCount()
    {
      GameObject newAim = new();

      for (int j = 0; j < _bulletCount; j++)
      {
        GameObject newAimPoint = Instantiate(newAim, Vector3.zero, quaternion.identity, _aim);
        newAimPoint.name = "Aim: " + (j + 1);
        _childAims.Add(newAimPoint.transform);

        int x = (int)Math.Pow(-1, j);
        int y = (int)Math.Ceiling((j + 1) / 2.0);
        float xPosition = x * y * 0.25f;

        float newXPosition = Vector3.zero.x + xPosition;
        newAimPoint.transform.localPosition = new Vector2(newXPosition, 0);
      }
    }

    private void AddNewMissileToQueue(int count)
    {
      for (int i = 0; i < count; i++)
      {
        GameObject missile = Instantiate(_missile, _aim.position, Quaternion.identity, _missileStack);
        missile.GetComponent<Missile>().SetFire(this);
      }
    }

    [Space(20)]
    [SerializeField]
    private GameObject _cannon;
    
    [SerializeField]
    private Transform _fireParticleParent;
    private void AttackAnimation()
    {
      GameManager.Instance.EffectManager.PlayParticleEffectFromPool(_aim.position, VFX.Fire, _fireParticleParent);
      
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

    public int GetAttackDamage()
    {
      return _player.PlayerStat.AttackDamage;
    }
  }
}
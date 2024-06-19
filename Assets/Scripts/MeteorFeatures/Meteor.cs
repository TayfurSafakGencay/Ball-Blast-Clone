using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeteorFeatures
{
  public class Meteor : MonoBehaviour
  {
    private MeteorSpawner _meteorSpawner;

    private Rigidbody2D _rb2D;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private TextMeshPro _healthText;

    [SerializeField]
    private SpriteRenderer _sprite;

    private int _health;

    private int _maxHealth;

    private int _stage;

    private int _maxStage;

    private bool _isDied;

    private void Awake()
    {
      _rb2D = GetComponent<Rigidbody2D>();
      _meteorSpawner = GameManager.Instance.meteorSpawner;
    }

    private void Update()
    {
      float zRotation = Time.deltaTime * 45;
      transform.Rotate(0f, 0f, zRotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.CompareTag("Wall"))
      {
        _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
        ApplyForce();
      }
      else if (other.gameObject.CompareTag("Ground"))
      {
        HitGround();
      }
      else if (other.gameObject.CompareTag("Player"))
      {
        GameManager.Instance.EffectManager.PlayParticleEffect(other.transform.position, VFX.Hit);
        
        GameManager.Instance.GameOver();
      }
    }

    public void SetMeteorInitialStats(int health, int stage, Color color, int maxStage, bool initial = true)
    {
      _stage = stage;
      _maxStage = maxStage;
      _health = health;
      _maxHealth = health;
      _sprite.color = color;

      SetLocalScale();
      SetHealthText();

      if (!initial) return;
      ApplyForce(75);
    }

    public void TakeDamage(int damage)
    {
      _health -= damage;
      SetHealthText();
      TakeDamageAnimation();

      if (_health > 0 || _isDied) return;
      _isDied = true;
      Die();
    }

    private void Die()
    {
      switch (_stage)
      {
        case > 1:
          _meteorSpawner.ShredMeteor(_stage, _maxHealth, transform.position, _sprite.color, _maxStage);
          break;
        case 1:
          _meteorSpawner.MeteorDestroyed(_maxStage, transform.position);
          break;
        case 0:
          _meteorSpawner.BossDestroyed(transform.position);
          break;
      }
      
      GameManager.Instance.EffectManager.PlayParticleEffectFromPool(transform.position, VFX.MeteorDestroy);

      Destroy(gameObject);
    }

    private void SetHealthText()
    {
      if (_health > 1000)
      {
        float divided = _health / 1000f;
        string text = Math.Round(divided, 1).ToString("0.0") + "k";
        _healthText.text = text;
        return;
      }
      _healthText.text = _health.ToString();
    }

    public void RebirthForce(string key)
    {
      switch (key)
      {
        case "left":
          transform.localPosition -= new Vector3(transform.localScale.x, 0, 0);
          break;
        case "right":
          transform.localPosition += new Vector3(transform.localScale.x, 0, 0);
          break;
      }

      _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce / 2f);

      ApplyForce(40, key);
    }

    private void ApplyForce(int force = 50, string key = null)
    {
      switch (key)
      {
        case null when transform.position.x > 0:
          _rb2D.AddForce(Vector2.left * force);
          break;
        case null:
          _rb2D.AddForce(Vector2.right * force);
          break;
        case "left":
          _rb2D.AddForce(Vector2.left * force);
          break;
        case "right":
          _rb2D.AddForce(Vector2.right * force);
          break;
      }
    }

    private TweenerCore<Vector3, Vector3, VectorOptions> _takeDamageAnimation;

    private void TakeDamageAnimation()
    {
      _takeDamageAnimation = transform.DOScale(new Vector3(_scale + 0.2f, _scale + 0.2f, 0), 0.125f).SetEase(Ease.InBounce).OnComplete(() =>
      {
        transform.DOScale(new Vector3(_scale, _scale, 0), 0.125f);
      });
    }

    private void OnDestroy()
    {
      _takeDamageAnimation.Kill();
    }

    private void HitGround()
    {
      _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce);

      Vector2 hitPosition = new(transform.position.x, transform.position.y - transform.localScale.y / 2);
      GameManager.Instance.EffectManager.PlayParticleEffectFromPool(hitPosition, VFX.HitGround);
    }

    private float _scale;

    private void SetLocalScale()
    {
      int coefficient = _stage;
      if (_stage == 0)
        coefficient = 4;
      
      float value = 0.175f + 0.15f * coefficient;
      _scale = value;
      GetComponent<SortingGroup>().sortingOrder = 5 - coefficient;
      
      transform.localScale = new Vector3(_scale, _scale, 0);
    }
  }
}
using System;
using TMPro;
using UnityEngine;

namespace MeteorFeatures
{
  public class Meteor : MonoBehaviour
  {
    private MeteorSpawner _meteorSpawner;
    
    private Rigidbody2D _rb2D;
    
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private TextMeshProUGUI _healthText;

    private int _health;

    private int _maxHealth;

    private int _stage;

    private void Awake()
    {
      _rb2D = GetComponent<Rigidbody2D>();
      _meteorSpawner = GameManager.Instance.meteorSpawner;
    }

    private void Update()
    {
      float zRotation = Time.deltaTime * 30;
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
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce);
      }
      else if (other.gameObject.CompareTag("Player"))
      {
        GameManager.Instance.GameOver();
      }
    }

    public void SetMeteorInitialStats(int health, int stage, bool initial = true)
    {
      _stage = stage;
      _health = health;
      _maxHealth = health;
      
      SetLocalScale();
      SetHealthText();
      
      if (initial)
        ApplyForce(75);
    }

    public void TakeDamage(int damage)
    {
      _health -= damage;
      SetHealthText();
      
      if (_health <= 0)
        Die();
    }
    
    private void Die()
    {
      if (_stage > 1)
      {
        _meteorSpawner.DestroyMeteor(_stage, _maxHealth, transform.position);
      }
      
      Destroy(gameObject);
    }

    private void SetHealthText()
    {
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
      
      _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce / 1.5f);

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

    private void SetLocalScale()
    {
      transform.localScale = _stage switch
      {
        4 => new Vector3(1.3f, 1.3f),
        3 => new Vector3(1f, 1f),
        2 => new Vector3(0.7f, 0.7f),
        _ => new Vector3(0.4f, 0.4f)
      };
    }
  }
}
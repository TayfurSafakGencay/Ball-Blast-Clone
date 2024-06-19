using MeteorFeatures;
using PlayerFeatures;
using UnityEngine;

public class Missile : MonoBehaviour
{
  private Fire _fire;

  private Rigidbody2D _rb2D;

  private readonly Vector2 _direction = new(0, 12f);

  private int _attackDamage;

  private void Awake()
  {
    _rb2D = GetComponent<Rigidbody2D>();
  }

  public void SetFire(Fire fire)
  {
    _fire = fire;
    
    gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    if (_fire == null) return;

    _rb2D.velocity = _direction;

    _attackDamage = _fire.GetAttackDamage();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Wall"))
    {
      gameObject.SetActive(false);
    }
    else if (other.gameObject.CompareTag("Meteor"))
    {
      other.gameObject.GetComponent<Meteor>().TakeDamage(_attackDamage);

      gameObject.SetActive(false);
    }
  }

  private void OnDisable()
  {
    _fire.AddMissileToQueue(gameObject);

    _rb2D.velocity = Vector2.zero;
  }
}
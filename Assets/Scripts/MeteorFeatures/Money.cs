using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MeteorFeatures
{
  public class Money : MonoBehaviour
  {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private int _value;

    private Rigidbody2D _rb2D;

    private void Awake()
    {
      _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      GameManager.GameStarted += OnGameStart;
      
      int xValue = Random.Range(-50, 50);
      _rb2D.AddForce(new Vector2(xValue, 0));
      _rb2D.drag = Random.Range(0.4f, 0.6f);

      StartCoroutine(DestroyEnumerator());
    }

    private void OnGameStart()
    {
      Destroy(gameObject);
    }

    public void SetValue(int value, Sprite sprite)
    {
      _value = value;
      _spriteRenderer.sprite = sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.CompareTag("Ground"))
      {
        _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
      }
      else if (other.gameObject.CompareTag("Player"))
      {
        GameManager.Instance.MoneyForTurn(_value, transform.position);
        Destroy(gameObject);
      }
    }

    private IEnumerator DestroyEnumerator()
    {
      yield return new WaitForSeconds(3f);
      _spriteRenderer.DOFade(0.5f, 1f);
      
      yield return new WaitForSeconds(3f);
      Destroy(gameObject);
    }
  }
}
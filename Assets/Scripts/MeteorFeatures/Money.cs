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

      DestroyEnumerator();
    }

    private void OnGameStart()
    {
      if (gameObject != null)
      {
        gameObject.SetActive(false);
      }
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
        GameManager.Instance.MoneyCollected(_value, transform.position);
        Destroy(gameObject);
      }
    }

    private void OnDisable()
    {
      GameManager.GameStarted -= OnGameStart;
    }

    private void OnDestroy()
    {
      GameManager.GameStarted -= OnGameStart;

      _isDestroyed = true;
    }

    private bool _isDestroyed;

    private async void DestroyEnumerator()
    {
      await GameManager.Delay(3f);
      
      if (_isDestroyed) return;
      _spriteRenderer.DOFade(0.5f, 1f);
      
      await GameManager.Delay(3f);
      if (_isDestroyed) return;
      Destroy(gameObject);
    }
  }
}
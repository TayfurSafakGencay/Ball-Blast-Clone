using Managers;
using UnityEngine;

namespace PlayerFeatures
{
  public class Movement : MonoBehaviour
  {
    private GameManager _gameManager;

    private Rigidbody2D _rb2D;

    [SerializeField]
    private GameObject _leftWheel;

    [SerializeField]
    private GameObject _rightWheel;

    private void Start()
    {
      _gameManager = GameManager.Instance;

      _camera = _gameManager.Camera;
      _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      if (!_gameManager.GetIsGameStarted() || _gameManager.GetIsGameFinished())
        return;

      MoveToTouchPosition();
    }

    private Vector3 _targetPosition;

    [SerializeField]
    private float _speed;

    private Camera _camera;

    private void MoveToTouchPosition()
    {
      float zRotation = Time.deltaTime * _rb2D.velocity.x * 150;
      _leftWheel.transform.Rotate(0f, 0f, zRotation);
      _rightWheel.transform.Rotate(0f, 0f, zRotation);
      
      if (Input.touchCount > 0)
      {
        Touch touch = Input.GetTouch(0);

        _targetPosition = _camera.ScreenToWorldPoint(touch.position);
        _targetPosition.y = transform.position.y;

        Vector3 direction = _targetPosition - transform.position;
        _rb2D.velocity = direction.normalized * _speed;
      }

      if (Mathf.Abs(_targetPosition.x - transform.position.x) > 0.1f) return;
      _rb2D.velocity = Vector2.zero;
    }
  }
}
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MeteorFeatures
{
  public class MeteorSpawner : MonoBehaviour
  {
    [Space(15)]
    [SerializeField]
    private List<Transform> _meteorSpawners;

    [Space(15)]
    [SerializeField]
    private Transform _meteorContainer;

    [Space(15)]
    [SerializeField]
    private GameObject _meteorObject;

    private GameManager _gameManager;

    private float _time;

    private float _remainingTime;

    private void Start()
    {
      _gameManager = GameManager.Instance;
      _remainingTime = 1;
      
      SetSpawnersPositions();
    }

    private void Update()
    {
      _remainingTime -= Time.deltaTime;

      if (_remainingTime > 0f) return;

      _time = Random.Range(3, 8);
      _remainingTime = _time;
      SpawnMeteor();
    }

    private void SpawnMeteor()
    {
      Transform meteorSpawner = GameManager.GetRandomElementFromList(_meteorSpawners);

      GameObject meteor = Instantiate(_meteorObject, meteorSpawner.transform.position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteor.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(3, 4);
    }

    public void DestroyMeteor(int stage, int maxHealth, Vector3 position)
    {
      int newStage = stage - 1;
      int newMaxHealth = maxHealth / 2;
      if (newMaxHealth <= 0) newMaxHealth = 1;
      
      RebirthMeteor(position, newMaxHealth, newStage, "left");
      RebirthMeteor(position, newMaxHealth, newStage, "right");
    }

    private void RebirthMeteor(Vector3 position, int newMaxHealth, int newStage, string key)
    {
      GameObject meteorObject = Instantiate(_meteorObject, position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteorObject.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(newMaxHealth, newStage, false);
      meteorComponent.RebirthForce(key);
    }

    private Vector2 screenSize;

    private void SetSpawnersPositions()
    {
      screenSize.x = Vector2.Distance(_gameManager.Camera.ScreenToWorldPoint(new Vector2(0, 0)), 
        _gameManager.Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f; 
      screenSize.y = Vector2.Distance(_gameManager.Camera.ScreenToWorldPoint(new Vector2(0, 0)), 
        _gameManager.Camera.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
      
      _meteorSpawners[0].position = new Vector3(_gameManager.Camera.transform.position.x + screenSize.x + 0.1f, _gameManager.Camera.transform.position.y + 2.5f, 0);
      _meteorSpawners[1].position = new Vector3(_gameManager.Camera.transform.position.x - screenSize.x - 0.1f, _gameManager.Camera.transform.position.y + 2.5f, 0);
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Vo;
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

    [Space(15)]
    [SerializeField]
    private List<Color> _meteorColors;

    private float _time;

    private float _remainingTime;

    private int _spawnedCount;

    private bool _stopSpawn;

    private bool _isGameStarted;

    private void Start()
    {
      SetSpawnersPositions();

      GameManager.GameStarted += GameStarted;
    }

    private void GameStarted()
    {
      StartCoroutine(DestroyAllMeteors());

      LevelStatsVo levelStatsVo = GameManager.Instance.GetLevelStats();

      _meteorCount = levelStatsVo.MeteorCount;
      _minHealth = levelStatsVo.MinHealth;
      _maxHealth = levelStatsVo.MaxHealth;

      _isGameStarted = true;
      _remainingTime = 1;

      _spawnedCount = 0;
      _stopSpawn = false;

      _destroyedMeteorCount = 0;
    }

    private void Update()
    {
      if (!_isGameStarted) return;
      if (_stopSpawn) return;

      _remainingTime -= Time.deltaTime;

      if (_remainingTime > 0f) return;

      SpawnMeteor();
    }

    private int _meteorCount;

    private int _minHealth;

    private int _maxHealth;

    private void SpawnMeteor()
    {
      _spawnedCount++;
      if (_spawnedCount >= _meteorCount) _stopSpawn = true;

      _time = Random.Range(3, 8);
      _remainingTime = _time;

      Transform meteorSpawner = GameManager.GetRandomElementFromList(_meteorSpawners);
      Color color = GameManager.GetRandomElementFromList(_meteorColors);

      int meteorStage = Random.Range(1, 5);
      int meteorHealth = Random.Range(_minHealth, _maxHealth + 1);

      GameObject meteor = Instantiate(_meteorObject, meteorSpawner.transform.position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteor.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(meteorHealth, meteorStage, color);
    }

    public void ShredMeteor(int stage, int maxHealth, Vector3 position, Color color)
    {
      int newStage = stage - 1;
      int newMaxHealth = maxHealth / 2;
      if (newMaxHealth <= 0) newMaxHealth = 1;

      RebirthMeteor(position, newMaxHealth, newStage, "left", color);
      RebirthMeteor(position, newMaxHealth, newStage, "right", color);
    }

    private void RebirthMeteor(Vector3 position, int newMaxHealth, int newStage, string key, Color color)
    {
      GameObject meteorObject = Instantiate(_meteorObject, position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteorObject.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(newMaxHealth, newStage, color, false);
      meteorComponent.RebirthForce(key);
    }

    private Vector2 screenSize;

    private void SetSpawnersPositions()
    {
      GameManager _gameManager = GameManager.Instance;

      screenSize.x = Vector2.Distance(_gameManager.Camera.ScreenToWorldPoint(new Vector2(0, 0)),
        _gameManager.Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
      screenSize.y = Vector2.Distance(_gameManager.Camera.ScreenToWorldPoint(new Vector2(0, 0)),
        _gameManager.Camera.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

      _meteorSpawners[0].position = new Vector3(_gameManager.Camera.transform.position.x + screenSize.x + 0.1f, _gameManager.Camera.transform.position.y + 2.5f, 0);
      _meteorSpawners[1].position = new Vector3(_gameManager.Camera.transform.position.x - screenSize.x - 0.1f, _gameManager.Camera.transform.position.y + 2.5f, 0);
    }

    private int _destroyedMeteorCount;
    public void MeteorDestroyed()
    {
      _destroyedMeteorCount++;

      if (_destroyedMeteorCount >= _meteorCount)
      {
        GameManager.Instance.GameOver(true);
      }
    }

    private IEnumerator DestroyAllMeteors()
    {
      while (_meteorContainer.childCount > 0)
      {
        Destroy(_meteorContainer.GetChild(0).gameObject);
        yield return null;
      }
    }
  }
}
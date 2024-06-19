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

    private readonly List<int> _meteorStageList = new();

    private int _totalMeteorValue;

    private int _level;
    
    private void Start()
    {
      SetSpawnersPositions();

      GameManager.GameStarted += GameStarted;
    }

    private void GameStarted()
    {
      _meteorStageList.Clear();
      
      _isGameStarted = true;
      _remainingTime = 1;

      _spawnedCount = 0;
      _stopSpawn = false;

      _totalMeteorValue = 0;
      _destroyedCount = 0;
      
      StartCoroutine(DestroyAllMeteors());

      LevelStatsVo levelStatsVo = GameManager.Instance.GetLevelStats();

      _level = levelStatsVo.Level;
      _meteorCount = levelStatsVo.MeteorCount;
      _minHealth = levelStatsVo.MinHealth;
      _maxHealth = levelStatsVo.MaxHealth;

      for (int i = 0; i < _meteorCount; i++)
      {
        int meteorStage = Random.Range(1, 5);

        if (levelStatsVo.IsBossLevel)
        {
          meteorStage = 0;
        }
        
        int totalMeteorValue = 1;

        for (int j = 0; j < meteorStage - 1; j++)
        {
          totalMeteorValue = totalMeteorValue * 2 + 1;
        }
        
        _totalMeteorValue += totalMeteorValue;

        _meteorStageList.Add(meteorStage);
      }
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

      _time = Random.Range(2, 5);
      _remainingTime = _time;

      Transform meteorSpawner = GameManager.GetRandomElementFromList(_meteorSpawners);
      Color color = GameManager.GetRandomElementFromList(_meteorColors);

      int meteorHealth = Random.Range(_minHealth, _maxHealth + 1);

      GameObject meteor = Instantiate(_meteorObject, meteorSpawner.transform.position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteor.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(meteorHealth, _meteorStageList[_spawnedCount - 1], color, _meteorStageList[_spawnedCount - 1]);
    }

    public void ShredMeteor(int stage, int maxHealth, Vector3 position, Color color, int maxStage)
    {
      MeteorDestroyedCounter();
      
      int newStage = stage - 1;
      int newMaxHealth = maxHealth / 2;
      if (newMaxHealth <= 0) newMaxHealth = 1;

      RebirthMeteor(position, newMaxHealth, newStage, "left", color, maxStage);
      RebirthMeteor(position, newMaxHealth, newStage, "right", color, maxStage);
    }

    private void RebirthMeteor(Vector3 position, int newMaxHealth, int newStage, string key, Color color, int maxStage)
    {
      GameObject meteorObject = Instantiate(_meteorObject, position, Quaternion.identity, _meteorContainer);
      Meteor meteorComponent = meteorObject.GetComponent<Meteor>();
      meteorComponent.SetMeteorInitialStats(newMaxHealth, newStage, color, maxStage,false);
      meteorComponent.RebirthForce(key);
    }

    [Space(20)]
    [SerializeField]
    private GameObject _money;

    [SerializeField]
    private Transform _moneyContainer;
    
    [SerializeField]
    private List<Sprite> _sprites;

    private int _destroyedCount;

    public async void MeteorDestroyed(int maxStage, Vector3 position)
    {
      float chance = 50;
      for (int i = 0; i < maxStage; i++)
      {
        if (Random.Range(1, 101) > chance) continue;

        chance -= 10;
        int value = Random.Range(1, 8);
        GameObject moneyInstantiate = Instantiate(_money, position, Quaternion.identity, _moneyContainer);
        Money money = moneyInstantiate.GetComponent<Money>();
        money.SetValue(value, _sprites[value - 1]);
      }
      
      MeteorDestroyedCounter();
      
      await GameManager.Delay(0.15f);
      if (!_stopSpawn) return;
      if (_meteorContainer.childCount != 0) return;

      GameManager.Instance.GameOver(true);
    }

    public async void BossDestroyed(Vector3 position)
    {
      int stage = _level / 5 + 10;
      for (int i = 0; i < stage; i++)
      {
        int value = Random.Range(1, 8);
        GameObject moneyInstantiate = Instantiate(_money, position, Quaternion.identity, _moneyContainer);
        Money money = moneyInstantiate.GetComponent<Money>();
        money.SetValue(value, _sprites[value - 1]);
      }
      
      await GameManager.Delay(0.15f);

      GameManager.Instance.GameOver(true);
    }

    private void MeteorDestroyedCounter()
    {
      _destroyedCount++;
      
      GameManager.Instance.MeteorDestroy(_destroyedCount, _totalMeteorValue);
    }

    private IEnumerator DestroyAllMeteors()
    {
      while (_meteorContainer.childCount > 0)
      {
        Destroy(_meteorContainer.GetChild(0).gameObject);
        yield return null;
      }
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
  }
}
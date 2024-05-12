using MeteorFeatures;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vo;

namespace Managers
{
  public sealed partial class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    public LevelManager LevelManager;

    private LevelStatsVo _levelStatsVo;

    [Space(20)]
    public MeteorSpawner meteorSpawner;

    [Space(20)]
    public Camera Camera;

    [Space(20)]
    public Transform BulletStack;

    private bool _isGameStarted;

    private bool _isGameFinished;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      
      SetLevelData();
    }

    private void Start()
    {
      _isGameStarted = true;
    }

    private void SetLevelData()
    {
      _levelStatsVo = LevelManager.GetLevelData(int.Parse(SceneManager.GetActiveScene().name));
    }

    public void GameOver()
    {
      _isGameFinished = true;

      Time.timeScale = 0;
    }
  }
}
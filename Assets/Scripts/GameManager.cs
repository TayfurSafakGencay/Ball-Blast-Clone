using System.Collections.Generic;
using MeteorFeatures;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera Camera;

    [Space(20)]
    public Transform BulletStack;

    [FormerlySerializedAs("ObstacleSpawner")]
    [Space(20)]
    public MeteorSpawner meteorSpawner;

    private bool _isGameStarted;

    private bool _isGameFinished;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _isGameStarted = true;
    }

    public void GameOver()
    {
        _isGameFinished = true;

        Time.timeScale = 0;
    }
    
    public static T GetRandomElementFromList<T>(List<T> list)
    {
        Random random = new();
        int index = random.Next(0, list.Count);
        return list[index];
    }
    
    // Getter - Setter

    public bool GetIsGameStarted()
    {
        return _isGameStarted;
    }

    public bool GetIsGameFinished()
    {
        return _isGameFinished;
    }
}

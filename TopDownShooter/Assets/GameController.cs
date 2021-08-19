using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class GameController : MonoBehaviour
{

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] List<GameObject> EnemyTypes = new List<GameObject>();
    [SerializeField] int WaveTimer;
    [SerializeField] int MinWaveTimer;
    [SerializeField] int WaveTimerDecrease;
    [SerializeField] UIManager UIManager;
    public Health player;
    public int points = 0;

    private int BestScore;
    private float BestTime;


    private void Start()
    {
        LoadData();
    }
    public void SetupGame()
    {
        //activate controls
        player.GetComponent<PlayerInput>().enabled = true;
        UIManager.CloseMainMenu();
        StartCoroutine(SpawnEnemies());
        player.OnDeath += GameOver;
    }

    IEnumerator SpawnEnemies()
    {
        foreach (GameObject spawn in spawnPoints)
        {
            GameObject EnemyToSpawn = EnemyTypes[UnityEngine.Random.Range(0, EnemyTypes.Count)];
            GameObject Enemy = Instantiate(EnemyToSpawn, spawn.transform.position, Quaternion.identity);
            Enemy.GetComponent<Health>().OnDeath += ScorePoints;
        }
        yield return new WaitForSeconds(WaveTimer);
        WaveTimer = Mathf.Max(WaveTimer - WaveTimerDecrease, MinWaveTimer);
        StartCoroutine(SpawnEnemies());
    }

    void GameOver()
    {
        //disable input and spawns 
        StopAllCoroutines();
        player.GetComponent<PlayerInput>().enabled = false;
        UIManager.GameOverShowPanel();

        if(points > BestScore && BestTime < UIManager.StopTime)
        {
            SaveData();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ScorePoints()
    {
        points += 10;
        UIManager.ChangeScore(points);
    }

    void SaveData()
    {
        SaveData saveData = new SaveData();
        saveData.Score = points;
        saveData.Time = UIManager.StopTime;
        try
        {
            File.WriteAllText(Application.persistentDataPath + "/SaveData.save", saveData.ToJson());
        }catch(Exception e)
        {
            Debug.LogError($"Failed to write with exception {e}");
        }
    }

    void LoadData()
    {
        SaveData saveData = new SaveData();
        try
        {
        saveData.LoadFromJson((File.ReadAllText(Application.persistentDataPath + "/SaveData.save")));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read with exception {e}");
        }

        BestScore = saveData.Score;
        BestTime = saveData.Time;
        UIManager.ShowBestScore(BestScore, BestTime);
    }
}

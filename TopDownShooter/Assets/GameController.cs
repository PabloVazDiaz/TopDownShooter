using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] List<PoolObject> EnemyTypes = new List<PoolObject>();
    [SerializeField] List<PowerUpInfo> PowerUps = new List<PowerUpInfo>();
    [SerializeField] int WaveTimer;
    [SerializeField] int MinWaveTimer;
    [SerializeField] int WaveTimerDecrease;
    [SerializeField] UIManager UIManager;

    public GameObject powerUpPrefab;

    [Range(0,1)]
    public float PowerUpProb;
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
            PoolObject EnemyToSpawn = EnemyTypes[UnityEngine.Random.Range(0, EnemyTypes.Count)];
            //GameObject Enemy = Instantiate(EnemyToSpawn, spawn.transform.position, Quaternion.identity);
            GameObject Enemy = ObjectPooler.instance.SpawnFromPool(EnemyToSpawn, spawn.transform.position, Quaternion.identity);
            Enemy.GetComponent<Health>().OnDeath += ScorePoints;
            Enemy.GetComponent<Health>().OnDeath += SpawnPowerUp;
            Enemy.GetComponent<IPooledObject>().OnObjectSpawn();
        }
        yield return new WaitForSeconds(WaveTimer);
        WaveTimer = Mathf.Max(WaveTimer - WaveTimerDecrease, MinWaveTimer);
        StartCoroutine(SpawnEnemies());
    }

    private void SpawnPowerUp(Vector3 position)
    {
        if(UnityEngine.Random.Range(0, 1) < PowerUpProb)
        {

            GameObject go = Instantiate(powerUpPrefab, position, Quaternion.identity);
            go.GetComponent<PowerUp>().powerUpInfo = PowerUps[UnityEngine.Random.Range(0, PowerUps.Count)];
        }
    }

    void GameOver(Vector3 position)
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

    void ScorePoints(Vector3 position)
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

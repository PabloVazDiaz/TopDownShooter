using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            GameObject EnemyToSpawn = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
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

    }

    void LoadData()
    {

    }
}

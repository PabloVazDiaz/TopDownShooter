using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Health player;
    public Slider HealthSlider;
    public Slider PowerUpSlider;
    public Text ScoreText;
    public Text TimeText;
    public Text BestScoreText;
    public Text PowerUpText;
    public Image HealthFlashImage;
    public CanvasGroup GameOverImage;
    public RectTransform GameOverText;
    public GameObject GameOverScore;
    public GameObject MainMenu;
    public GameObject HUDCanvas;
    public GameObject PowerUPCanvas;

    private float StartTime;
    public float StopTime;


   
    private void Start()
    {
        player.onHPChanged += ChangeHP;
    }

    private void Update()
    {
        if (StopTime > 0)
            return;
        ShowTime();
    }
    public void ChangeScore(int Score)
    {
        ScoreText.text = $"Score: {Score}";
    }

    public void ChangeHP(int newValue)
    {
        StopAllCoroutines();
        if(HealthSlider.value < newValue)
        {
            HealthFlash(Color.green, 0.1f);

        }
        else
        {
            StartCoroutine( HealthFlash(Color.red, 0.1f));
        }
        HealthSlider.value = newValue;
    }

    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        //pause
    }


    public void CloseMainMenu()
    {
        MainMenu.SetActive(false);
        HUDCanvas.SetActive(true);
        StartTime = Time.time;
    }

    public void GameOverShowPanel()
    {
        StopTime = Time.time - StartTime;
        LeanTween.alphaCanvas(GameOverImage, 1, 1f).setDelay(1.5f);
        LeanTween.alphaText(GameOverText, 1, 1f).setDelay(1.5f).setOnComplete(ShowGameOverScore);
        
    }

    public void ShowGameOverScore()
    {
        //Añadir tiempo
        GameOverScore.GetComponent<Text>().text = ScoreText.text;
        ScoreText.enabled = false;
        LeanTween.scale(GameOverScore, Vector3.one, 1f).setOnComplete(x=> SceneManager.LoadScene(SceneManager.GetSceneAt(0).name)) ;
    }

    private IEnumerator HealthFlash(Color color, float duration)
    {
        HealthFlashImage.color = color;
        for (float i = 0; i < duration; i+=Time.deltaTime)
        {
            HealthFlashImage.color = Color.Lerp(HealthFlashImage.color, Color.clear,  i / Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ShowBestScore(int Score, float time)
    {
        int minutes;
        int seconds;
        DisplayTime(time, out minutes, out seconds);
        BestScoreText.text = string.Format("Best score: \n {0} pts  {1:00}:{2:00}", Score, minutes, seconds);
    }

    public void ShowTime()
    {
        int minutes;
        int seconds;
        DisplayTime(Time.time - StartTime, out minutes, out seconds);
        TimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void DisplayTime(float time, out int minutes, out int seconds)
    {
         minutes = Mathf.FloorToInt(time / 60);
         seconds = Mathf.FloorToInt(time % 60);
    }

    public void showPowerUp(string name, float remainingTime)
    {
        if (remainingTime > 0)
        {
            PowerUPCanvas.SetActive(true);
            PowerUpText.text = name;
            PowerUpSlider.value =  remainingTime;
        }
        else
        {

            PowerUPCanvas.SetActive(false);
        }
    }
    
}

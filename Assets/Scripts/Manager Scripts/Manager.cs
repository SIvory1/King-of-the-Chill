using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{

    [Header("Input System")]
    public bool isPaused;

    [Header("Audio Manager")]
    public GameObject audioManagerObject; 
     AudioManager audioManager;

    [Header("User Interface")]
    public GameObject mainMenuUI;
    public GameObject howToPlayUI;
    public GameObject exitUI;
    public GameObject pauseUI;
    public GameObject ingameUI;
    public GameObject mpHostUI;
    public GameObject mpScreenUI;

    [Header("Round Timer")]
    public float roundTimer;
    public float minRoundTimer;
    public float maxRoundTimer;
    public bool roundStarted;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI redScore;
    public TextMeshProUGUI blueScore;

    public RawImage RedWin;
    public RawImage BlueWin;



    public WaterSplash points;

    
    public void Awake()
    {

        roundTimer = maxRoundTimer;

    }
    // Start is called before the first frame update
    void Start()
    {
        //Freezes all actions until gameplay is started
        Time.timeScale = 0;
        //Allows us to call any audio clip from the audio manager script
        audioManager = audioManagerObject.GetComponent<AudioManager>();
    }

    public void Update()
    {


        if (roundStarted == true)
        {
           roundTimer -= Time.deltaTime;
           DisplayTime(roundTimer);
        }

        if (roundTimer < minRoundTimer)
        {
            roundTimer = 0;
            roundStarted = false;
        }

        redScore.text = "" + points.redPoints;
        blueScore.text = "" + points.bluePoints;

        if (roundTimer <= 0 && points.bluePoints > points.redPoints)
        {
            SceneManager.LoadScene(2);
        }
        else if (roundTimer <= 0 && points.redPoints > points.bluePoints)
            SceneManager.LoadScene(3);

        if (roundTimer <= 0 && points.bluePoints == points.redPoints)
        {
            roundTimer += 30f;
            DisplayTime(roundTimer);
            roundStarted = true;
        }
    }


    // converts the time into minutes and seconds
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Menu()
    {
        if (isPaused == false)
        {
            pauseUI.SetActive(enabled);
            isPaused = true;
            audioManager.Pause();
            Time.timeScale = 0;
        }

        else
        {
            pauseUI.SetActive(false);
            isPaused = false;
            audioManager.Unpause();
            Time.timeScale = 1;
        }
    }

        //Allows us to play audio through the unity inspector through "OnClick()" button
    public void ButtonClickSound()
    {
        audioManager.InteractSound();
    }

    //Function which starts the game and disables the current active menu UI
    public void StartGame()
    {
        Time.timeScale = 1;
        mpHostUI.gameObject.SetActive(false);
        audioManager.AmbientOcean();
        audioManager.Music();
        ingameUI.gameObject.SetActive(true);
        audioManager.MenuNoiseStop();
        roundStarted = true;
    }

    public void StartOnline()
    {
        SceneManager.LoadScene(1);
        Debug.Log("LOAD ONLINE SCENE");
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene(0);
        Debug.Log("LOAD MENU SCENE");
    }

    //Function which quits game (Displays quit message in debug log as application.quit does not function within Unity's client)
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

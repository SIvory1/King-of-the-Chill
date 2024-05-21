using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject pauseUI;
    public bool isPaused;

    public GameObject[] playerScoreBoardArray;
    public Material[] colourArray;

    // called when pause button is pressed
    public void PauseGame()
    {
        if (isPaused == false)
        {
            pauseUI.SetActive(enabled);
            isPaused = true;
          //  audioManager.Pause();
            Time.timeScale = 0;
        }
        else
        { 
            pauseUI.SetActive(false);
            isPaused = false;
          //  audioManager.Unpause();
            Time.timeScale = 1;
        }
    }
}
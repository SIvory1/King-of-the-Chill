using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject pauseUI;
    public bool isPaused;

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

    public void UpdateScoreboardUI(GameObject specScoreboard, int currentScore)
    {
        specScoreboard.GetComponent<TextMeshProUGUI>().text = currentScore+"";
    }

}

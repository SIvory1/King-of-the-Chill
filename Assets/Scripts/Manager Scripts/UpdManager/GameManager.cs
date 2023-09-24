using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // give every manager we use a refernce here so we can just go through the game manager to use them

    /*  public ScoreManager scoreManager { get; private set; }
      public CurrencyManager currencyManager { get; private set; }
      public UIManager uiManager { get; private set; }
      public WaveManager waveManager { get; private set; }
      public AnimationManager animationManager { get; private set; }
      public AudioManager audioManager { get; private set; }
      public VFXManager vfxManager { get; private set; }
    */

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }

    }
}

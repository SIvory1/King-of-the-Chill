using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // give every manager we use a refernce here so we can just go through the game manager to use them
    public UIManager uiManager { get; private set; }


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

        InitManagers();
    }

    // attach all managers to a gamemanager prefab that we can out in each scene 
    void InitManagers()
    {
        uiManager = GetComponent<UIManager>();
    }
}

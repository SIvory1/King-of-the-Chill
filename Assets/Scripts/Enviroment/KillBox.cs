using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{

    public Transform spawnPoint;
    public AudioManager audioManager;
    public GameObject penguin;
 

    public void OnCollisionEnter(Collision col)
     {
        if (col.gameObject.name == "Penguin Blue")
        {
            col.gameObject.GetComponent<PlayerMove1>().Respawn();
            col.gameObject.GetComponent<PlayerMove1>().isFalling = true;

            col.gameObject.transform.position = spawnPoint.position;
            audioManager.PlayerFall();

        }

        if (col.gameObject.name == "Penguin Red")
        {
            col.gameObject.GetComponent<PlayerMove2>().Respawn();
            col.gameObject.GetComponent<PlayerMove2>().isFalling = true;

            col.gameObject.transform.position = spawnPoint.position;
            audioManager.PlayerFall();

        }
    }
}

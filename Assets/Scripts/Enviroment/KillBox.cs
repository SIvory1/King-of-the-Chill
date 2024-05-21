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
        if (col.gameObject.CompareTag("Player"))
        {

            col.gameObject.GetComponent<PlayerInputs>().Respawn();
            //  col.gameObject.GetComponent<PlayerInputs>().isFalling = true;


            col.gameObject.transform.position = spawnPoint.position;
            audioManager.PlayerFall();
        }
    }
}

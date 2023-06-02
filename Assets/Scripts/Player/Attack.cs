using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public GameObject enemyPlayer;
    public GameObject penguinPlayer;
    public float attackMultiplier;
    public float blockDeflectMultiplier;
    public GameObject audioManagerObject;
    AudioManager audioManager;

    public void Awake()
    {
        // gets the gameobject wiothout attachign it in the inspector
        // cause it keeps undeclaring itself for some reason
        audioManagerObject = GameObject.Find("Audio Manager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        enemyPlayer = col.gameObject;

        if (col.gameObject.tag == "Player" && enemyPlayer.GetComponent<PlayerMovementTest>().canBeHit == true && enemyPlayer.GetComponent<PlayerMovementTest>().iFrames == false)
        {
            enemyPlayer.GetComponent<PlayerMovementTest>().hitByEnemy = 1;
            audioManager.PlayerHit();
            enemyPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * attackMultiplier, ForceMode.Impulse);
            enemyPlayer.GetComponent<PlayerMovementTest>().animations.SetTrigger("Take Damage");
        }

        // if player hits someone who is blocking and they are attacking htey will be pushed backwards 
        if (col.gameObject.tag == "Player" && enemyPlayer.GetComponent<PlayerMovementTest>().canBeHit == false && enemyPlayer.GetComponent<PlayerMovementTest>().iFrames == false)
        {    
            penguinPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * blockDeflectMultiplier * -1, ForceMode.Impulse);
        }
    }
}

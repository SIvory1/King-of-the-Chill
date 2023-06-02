using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAttack : MonoBehaviour
{

    public GameObject enemyPlayer;
    public GameObject penguinPlayer;
    public float attackMultiplier;
    public float blockDeflectMultiplier;
    public GameObject attackParticle;
    public ParticleSystem attackParticleSystem;
    public GameObject audioManagerObject;
    AudioManager audioManager;

    public bool timerBool;
    public float timer;
    public float timerMax;

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

        if (col.gameObject.tag == "Player" && enemyPlayer.GetComponent<PlayerMove2>().canBeHit == true && enemyPlayer.GetComponent<PlayerMove2>().iFrames == false)
        {
            attackParticle.SetActive(enabled);
            attackParticleSystem.Play();

            timerBool = true;

            audioManager.PlayerHit();
            enemyPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * attackMultiplier, ForceMode.Impulse);
            enemyPlayer.GetComponent<PlayerMove2>().animations.SetTrigger("Take Damage");
        }

        // if player hits someone who is blocking and they are attacking htey will be pushed backwards 
        if (col.gameObject.tag == "Player" && enemyPlayer.GetComponent<PlayerMove2>().canBeHit == false && enemyPlayer.GetComponent<PlayerMove2>().iFrames == false)
        {    
            penguinPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * blockDeflectMultiplier * -1, ForceMode.Impulse);
        }
    }

    public void Update()
    {
        if (timerBool == true)
        {
            timer += Time.deltaTime;

            if (timer > timerMax)
            {
                attackParticleSystem.Stop();
                attackParticle.SetActive(false);
                timerBool = false;
                timer = 0;
            }
        }
    }

}

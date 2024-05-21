using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{

    public Vector3 waterHitLocation;
    public ParticleSystem waterParticle;
    public AudioManager audioManager;

    public int redPoints;
    public int bluePoints;


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerInputs>().PlayerDeath();

            audioManager.WaterSplash();

            // puts particle at where the player intersects with the water 
            waterHitLocation = col.gameObject.transform.position;
            waterParticle.transform.position = waterHitLocation;
            waterParticle.Play();

        }

    }

}

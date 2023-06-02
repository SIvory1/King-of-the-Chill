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


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            audioManager.WaterSplash();

            // puts particle at where the player intersects with the water 
            waterHitLocation = other.gameObject.transform.position;
            waterParticle.transform.position = waterHitLocation;
            waterParticle.Play();

        }
        if (other.gameObject.name == "Penguin Blue")
        {
            bluePoints += 1; 
        }

        if (other.gameObject.name == "Penguin Red")
        {
            redPoints += 1;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("UI Audio Components")]
    public AudioSource uiInteract;
    public AudioSource pause;
    public AudioSource unpause;

    [Header("Player Noises")]
    public AudioSource playerHit;
    public AudioSource playerAttack;
    public AudioSource playerBlock;
    public AudioSource playerFall;
    public AudioSource playerSlide;
    public AudioSource playerStunned;
    public AudioSource playerStunHit;
    public AudioSource playerLunge;


    [Header("Non-player noises")]
    public AudioSource iceCrack;
    public AudioSource ambientOcean;
    public AudioSource waterSplash;
    public AudioSource countDown;
    public AudioSource victory;
    public AudioSource music;
    public AudioSource menuNoise;


    public void InteractSound()
    {
        uiInteract.Play();
    }
    public void PlayerHit()
    {
        playerHit.Play();
    }
    public void PlayerAttack()
    {
        playerAttack.Play();
    }
    public void PlayerBlock()
    {
        playerBlock.Play();
    }
    public void PlayerFall()
    {
        playerFall.Play();
    }
    public void PlayerSlide()
    {
        playerSlide.Play();
    }
    public void PlayerStunned()
    {
        playerStunned.Play();
    }
    public void IceCrack()
    {
        iceCrack.Play();
    }
    public void AmbientOcean()
    {
        ambientOcean.Play();
    }
    public void WaterSplash()
    {
        waterSplash.Play();
    }
    public void Pause()
    {
        pause.Play();
    }
    public void Unpause()
    {
        unpause.Play();
    }
    public void PlayerStunHit()
    {
        playerStunHit.Play();
    }
    public void CountDown()
    {
        countDown.Play();
    }
    public void Victory()
    {
        victory.Play();
    }
    public void Music()
    {
        music.Play();
    }
    public void PlayerLunge()
    {
        playerLunge.Play();
    }
    public void MenuNoisePlay()
    {
        menuNoise.Play();
    }
    public void MenuNoiseStop()
    {
        menuNoise.Stop();
    }
}

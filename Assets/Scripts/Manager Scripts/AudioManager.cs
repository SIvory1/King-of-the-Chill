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

    public GameObject audioObject;

    /*
    [Header("Player Noises")]
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerAttack;
    [SerializeField] private AudioClip playerBlock;
    [SerializeField] private AudioClip playerFall;
    [SerializeField] private AudioClip playerSlide;
    [SerializeField] private AudioClip playerStunned;
    [SerializeField] private AudioClip playerStunHit;
    [SerializeField] private AudioClip playerLunge;


    [Header("Non-player noises")]
    [SerializeField] private AudioClip iceCrack;
    [SerializeField] private AudioClip ambientOcean;
    [SerializeField] private AudioClip waterSplash;
    [SerializeField] private AudioClip countDown;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip menuNoise;
    */

     private void PlaySound(AudioClip audioClip)
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(audioClip);
        StartCoroutine(DeleteAudioObject(audio));
    }

    IEnumerator DeleteAudioObject(GameObject audio)
    {
        yield return new WaitForSeconds(10);
        Destroy(audio);
    }

    public void PlayerHitAudio()
    {
    //    PlaySound(playerHit);
    }

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

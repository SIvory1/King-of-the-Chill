using System.Collections;
using UnityEngine;

public class SFXManager : MonoBehaviour
{ 
    
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

    public GameObject audioObject;

    public void TakenDmgAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerHit);
        StartCoroutine(DeleteAudio(audio));
    }

    public void BasicAttackAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerAttack);
        StartCoroutine(DeleteAudio(audio));
    }

    public void BlockAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerFall);
        StartCoroutine(DeleteAudio(audio));
    }

    public void FallingAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerBlock);
        StartCoroutine(DeleteAudio(audio));
    }

    public void SlideAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerSlide);
        StartCoroutine(DeleteAudio(audio));
    }

    public void StunnedAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerStunned);
        StartCoroutine(DeleteAudio(audio));
    }

    public void HitStunAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerStunHit);
        StartCoroutine(DeleteAudio(audio));
    }

    public void LungeAudio()
    {
        GameObject audio = Instantiate(audioObject, Vector3.zero, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(playerLunge);
        StartCoroutine(DeleteAudio(audio));
    }



    float deleteAudio = 10;
    IEnumerator DeleteAudio(GameObject audio)
    {
        yield return new WaitForSeconds(deleteAudio);
        Destroy(audio);
    }
}
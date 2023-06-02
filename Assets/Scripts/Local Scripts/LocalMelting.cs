using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMelting : MonoBehaviour
{
    public List<GameObject> icebergSections = new List<GameObject>();

    public List<GameObject> crackGif = new List<GameObject>();

    public float meltTimer;
    public float crackTimer;


    public GameObject audioManagerObject;
    AudioManager audioManager;

    public void Start()
    {
        
        StartCoroutine(IcebergMeltEnum());

        // gets the gameobject wiothout attachign it in the inspector
        // cause it keeps undeclaring itself for some reason
        audioManagerObject = GameObject.Find("Audio Manager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
    }


    private IEnumerator IcebergMeltEnum()
    {
       WaitForSeconds wait = new WaitForSeconds(crackTimer);

        while (true)
        {
            yield return wait;

            // sanity check to make sure it doesnt try and destory an obejct when there is none
            if (icebergSections.Capacity <= 0)
                yield return wait;

            //plays iceberg crack noise
            audioManager.IceCrack();

            // gets a section of the iceberg randomly
            GameObject choosenIcebergSection = icebergSections[Random.Range(0, icebergSections.Capacity)];

            // gets the child object of the iceberg section selected
            choosenIcebergSection.transform.GetChild(0).gameObject.SetActive(true);
            //calls the function that will destory the slected section
            StartCoroutine(IcebergDestroyEnum(choosenIcebergSection));
        }
    }

    private IEnumerator IcebergDestroyEnum(GameObject choosenIcebergSection)
    {
        WaitForSeconds wait = new WaitForSeconds(meltTimer);

        while (true)
        {
            yield return wait;
                // removes it from scene
                Destroy(choosenIcebergSection);
                // removes gameobject from the list
                icebergSections.Remove(choosenIcebergSection);
                // removes empty element from list
                if (icebergSections.Capacity > 0)
                    icebergSections.Capacity -= 1;
            }
        }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IcebergMelting : MonoBehaviour //Network Behaviour
{
    public GameObject playerObject;

    public List<GameObject> icebergSections = new List<GameObject>();

    public Vector3 targetPos;

    public float meltTimer;

    public GameObject audioManagerObject;
    AudioManager audioManager;

    public bool willSectionBreak;
    public bool doubleSectionBreak;
    public bool noSectionBreak;

   /* public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        StartCoroutine(IcebergMeltEnum());

        // gets the gameobject wiothout attachign it in the inspector
        // cause it keeps undeclaring itself for some reason
        audioManagerObject = GameObject.Find("Audio Manager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
    }*/


    private IEnumerator IcebergMeltEnum()
    {
       WaitForSeconds wait = new WaitForSeconds(meltTimer);

            while (true)
            {
            yield return wait;

            if (icebergSections.Capacity <= 0)
                yield return wait;

            //plays iceberg crack noise
            audioManager.IceCrack();

            // gets a section of the iceberg randomly
            GameObject choosenIcebergSection = icebergSections[Random.Range(0, icebergSections.Capacity)];
            // moves chunk for client view
            choosenIcebergSection.transform.position = targetPos;
            yield return new WaitForSeconds(1f);
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
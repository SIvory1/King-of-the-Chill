using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergMelting : MonoBehaviour
{
    public List<GameObject> icebergSections = new List<GameObject>();

    public List<GameObject> crackGif = new List<GameObject>();

    public float meltTimer;
    public float crackTimer;

    AudioManager audioManager;

    public void Start()
    {
        StartCoroutine(IcebergMeltEnum());

        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }


    private IEnumerator IcebergMeltEnum()
    {
        while (icebergSections.Count > 0)
        {
           // Debug.Log(icebergSections.Count);
            yield return new WaitForSeconds(crackTimer);

            // Play SFX
            audioManager.IceCrack();

            // Get Section
            GameObject icebergSection = icebergSections[Random.Range(0, icebergSections.Count-1)];

            // Activate Break Animation
            icebergSection.transform.GetChild(0).gameObject.SetActive(true);

            // Remove from list
            icebergSections.Remove(icebergSection);

            // Destroy Iceberg After Time
            Destroy(icebergSection, meltTimer);
        }
    }
}

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] float attackForceMultiplier;
    [SerializeField] float deflectedForceMultiplier;
    GameObject player;
    [SerializeField] GameObject attackEffect;

    private void Awake()
    {
        player = transform.parent.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider col)
    {
        // if player htis penguin they go back 
        if (col.gameObject.CompareTag("Player")) //&& col.GetComponent<PlayerMove2>().iFrames == false)
        {
            if(!col.gameObject.GetComponent<PlayerInputs>().canBeHit)
                 return;

            // checks if they are blocking
            if (col.gameObject.GetComponent<PlayerInputs>().isBlocking == false)
            {
                attackEffect.SetActive(true);
                StartCoroutine(RemoveAttackParticle());
                col.GetComponent<Rigidbody>().AddForce(transform.forward * attackForceMultiplier, ForceMode.Impulse);
                col.GetComponent<PlayerInputs>().PlayerTakenDmg();

            }
            else if (col.gameObject.GetComponent<PlayerInputs>().isBlocking)
            {
                player.GetComponent<Rigidbody>().AddForce(-transform.forward * deflectedForceMultiplier, ForceMode.Impulse);
                player.GetComponent<PlayerInputs>().PlayerStunned();
            }
        }
    }

    IEnumerator RemoveAttackParticle()
    {
        yield return new WaitForSeconds(0.4f);
        attackEffect.SetActive(false);
    }
}

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
        // if player hits penguin they go back 
        if (col.gameObject.CompareTag("Player")) 
        {
            if(!col.gameObject.GetComponent<PlayerInputs>().canBeHit)
                 return;

            // checks if they are blocking
            if (col.gameObject.GetComponent<PlayerInputs>().isBlocking == false)
            {
                col.GetComponent<PlayerInputs>().HitByEnemy(player);

                attackEffect.SetActive(true);
                StartCoroutine(RemoveAttackParticle());

                col.GetComponent<Rigidbody>().AddForce(transform.forward * attackForceMultiplier, ForceMode.Impulse);
                col.GetComponent<PlayerInputs>().PlayerTakenDmg();

            }
            // if you lunge attack they continue to stun but also lunge fix that 
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

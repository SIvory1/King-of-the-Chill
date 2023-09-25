using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] float attackForceMultiplier;
    [SerializeField] float deflectedForceMultiplier;
    GameObject player;

    private void Awake()
    {
        player = transform.parent.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider col)
    {
        // if player htis penguin they go back 
        if (col.gameObject.tag == "Player") //&& col.GetComponent<PlayerMove2>().iFrames == false)
        {
            // checks if they are blocking
            if (col.gameObject.GetComponent<PlayerInputs>().isBlocking == false)
            {
                col.GetComponent<Rigidbody>().AddForce(transform.forward * attackForceMultiplier, ForceMode.Impulse);
              //  col.GetComponent<PlayerInputs>().PlayerStunned();
                // call a fucntion in playe
            }
            else if (col.gameObject.GetComponent<PlayerInputs>().isBlocking)
            {
                player.GetComponent<Rigidbody>().AddForce(-transform.forward * deflectedForceMultiplier, ForceMode.Impulse);
                player.GetComponent<PlayerInputs>().PlayerStunned();
            }
        }
    }
}

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
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerInputs>().isBlocking == false) //&& col.GetComponent<PlayerMove2>().iFrames == false)
        {
            col.GetComponent<Rigidbody>().AddForce(transform.forward * attackForceMultiplier, ForceMode.Impulse);
        }
        // if they hit player and they are blocking, they get pushed back and stunned etc
        else if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerInputs>().isBlocking)
        {
            player.GetComponent<Rigidbody>().AddForce(-transform.forward * deflectedForceMultiplier, ForceMode.Impulse);
        }
    }
}

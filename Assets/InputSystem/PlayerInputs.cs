using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputs : MonoBehaviour
{
    // https://www.youtube.com/watch?v=l9HrraxtdGY
    // https://www.youtube.com/watch?v=WIl6ysorTE0

    //input fields
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed;

    //Abilities
    bool canUseAbility;
    [SerializeField] float dodgeSpeedMultiplier;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void Start()
    {
        currentSpeed = maxSpeed;
        canUseAbility = true;
    }

    //reason we do this with direct refernces to strings is so the system can tell the difference between inputsystems
    private void OnEnable()
    {
        // playerInputActions.Player.Movement.started += DoAttack;
        player.FindAction("Attack").started += DoAttack;
        player.FindAction("Block").started += DoBlock;
        player.FindAction("Lunge").started += DoLunge;
        player.FindAction("Dodge").started += DoDodge;
        move = player.FindAction("Movement");
        player.Enable();
    }


    private void OnDisable()
    {
        player.FindAction("Attack").started -= DoAttack;
        player.FindAction("Block").started -= DoBlock;
        player.FindAction("Lunge").started -= DoLunge;
        player.FindAction("Dodge").started -= DoDodge;
        player.Disable();
    }

    // in the original we just had a bunch of if statements and timers/bool in update. will do it using courtines this time, alot of the code can
    //probaly be repourposed but also a bunch of it will be mess
    private void FixedUpdate()
    {
        Movement();
    }

    // all of this is used for the combat. they have funciton in the other scripts that can be repurposed.
    #region Ablities

    // proably doint even need to have an attack script, i think we just doa spehrecast infront of the player and use that to add forces. i have 
    // this from hamster wrangler probaly wont be hto ad to set up. 
    private void DoAttack(InputAction.CallbackContext obj)
    {
        print("attackjhon");
    }


    #region Blocking
    bool isBlocking;
    private void DoBlock(InputAction.CallbackContext obj)
    {
        print("blockjohn");
        //when a player is hit and stunned they wont be able to sue abiolites ergo we check to see if they can use block
        if (!canUseAbility)
            return;
        currentSpeed = 0f;
        // stops the player form using other ablities 
        isBlocking = true;
        StartCoroutine(BlockCooldown());
        canUseAbility = false;
    }

    // we choose how long the player is in the block for 
    float exitBlockTimer = 1.5f;
    IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(exitBlockTimer);
        canUseAbility = true;
        currentSpeed = maxSpeed;
    }
    #endregion

    private void DoLunge(InputAction.CallbackContext obj)
    {
        print("lungejohn");
    }


    bool isDodging;
    private void DoDodge(InputAction.CallbackContext obj)
    {
        if (!canUseAbility)
            return;

        // used for locking slide in place and particle effect
        isDodging = true;
        // makes it so player cnat use any other ability mid use
        // adds a force to the player, spped can be adjusted with dodgeMultiplier
        rb.AddForce(transform.forward * dodgeSpeedMultiplier, ForceMode.Impulse);

        StartCoroutine(DodgeCooldown());
        canUseAbility = false;

    }

    // do this in a the aniamtion keyframes proabnly, at the end of it come out of dodge for best player expericance
    float exitDodgeTimer = 1.5f;
    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(exitDodgeTimer);
        isDodging = false;
        canUseAbility = true;
    }

    #endregion

    public void Movement()
    {
        if (Time.timeScale == 0)
            return;
        //   if (isDodging)
        //       return;

        //player movement, can only use vector2 for controller so we use a vector3
        // but store the x and z in a vector 2
        Vector2 inputVector = move.ReadValue<Vector2>();
        Vector3 tempVec = new Vector3(inputVector.x, 0, inputVector.y);

        // adds force to the vector, do this seperately so we can use
        //the variable for the player rotation
        rb.AddForce(tempVec * currentSpeed, ForceMode.Force);

        if (tempVec != Vector3.zero)
        {
            // finds the direction the player is moving
            Quaternion targetRotation = Quaternion.LookRotation(tempVec);
            // rotates players towards the way they are facing
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 270 * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
    }
}
/*   // when a player dashes at someone when they are blocking it breaks them out of the dodge.
   GameObject enemy;
   public void OnCollisionEnter(Collision col)
   {
       // for player respawning
       //  if (respawning == true)
       //     return;

       if (!isDodging)
           return;

       if (col.gameObject.tag == "Player")
           enemy = col.gameObject;

       if (enemy.GetComponent<PlayerInput>().isBlocking)
       {
           // sets all the vairables needed to stun the enemy player
           // use this stun counter so the stun aniamtion doesnt loop
           enemy.GetComponent<PlayerMovementTest>().isStunnedCounter += 1;
           enemy.GetComponent<PlayerMovementTest>().isStunned = true;
           enemy.GetComponent<PlayerMovementTest>().ForceUnblock();

       }
   }
/*
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// THEY TOO DAMN SLIDY< THIS GAME DOESNT WORK RN< ICE BERG NEEDS WORK< IT LOOKS BAD< JUST EVEREYTHIGN
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

    public int currentPoints;
    public GameObject scoreBoardUI;

    //Abilities
    [Header("General Ablities")]
    bool canUseAbility;
    bool isStunned;
    bool canMove;

    [Header("Dodge")]
    [SerializeField] float dodgeSpeedMultiplier;
    bool isDodging;
    float exitDodgeTimer = 1.5f;

    [Header("Attacking")]
    [SerializeField] GameObject attackObject;
    [SerializeField] GameObject lungeObject;

    [Header("Block")]
    GameObject enemy;
    IEnumerator unblockCoroutineVar;
    public bool isBlocking;
    bool blockCooldownFinished;
    float exitBlockTimer = 3f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void Start()
    {
        unblockCoroutineVar = UnblockCooldown();
        currentSpeed = maxSpeed;
        canUseAbility = true;
        canMove = true;
    }

    //reason we do this with direct refernces to strings is so the system can tell the difference between inputsystems
    private void OnEnable()
    {
        // playerInputActions.Player.Movement.started += DoAttack;
        player.FindAction("Attack").started += DoAttack;
        player.FindAction("Block").started += DoBlock;
        player.FindAction("Lunge").started += DoLunge;
        player.FindAction("Dodge").started += DoDodge;
        player.FindAction("Pause").started += DoPause;
        move = player.FindAction("Movement");
        player.Enable();
    }


    private void OnDisable()
    {
        player.FindAction("Attack").started -= DoAttack;
        player.FindAction("Block").started -= DoBlock;
        player.FindAction("Lunge").started -= DoLunge;
        player.FindAction("Dodge").started -= DoDodge;
        player.FindAction("Pause").started -= DoPause;
        player.Disable();
    }

    private void Update() {}

    // in the original we just had a bunch of if statements and timers/bool in update. will do it using courtines this time, alot of the code can
    //probaly be repourposed but also a bunch of it will be mess
    private void FixedUpdate()
    {
        Movement();
    }

    private void DoPause(InputAction.CallbackContext obj)
    {
        GameManager.instance.uiManager.PauseGame();
    }

    #region points and death

    // each player is given a piece of ui when they spawn in, when they die they call a functiuon
    // in ui manager that will update, make it so that it sends through which ui it is 

    GameObject currentEnemy;
    public void HitByEnemy(GameObject assignedEnemy)
    {
        // if player falls off and dies withotu beign hit they will lose a point? cap at zero
        currentEnemy = assignedEnemy;
        StartCoroutine(ReassignEnemy());
    }

    IEnumerator ReassignEnemy()
    {
        yield return new WaitForSeconds(3);
        currentEnemy = null;
    }

    public void PlayerDeath()
    {     
        if (currentEnemy != null)
        {
            // probs clean this up cause its stupid having the player hold its own ui? maybe?
            currentEnemy.GetComponent<PlayerInputs>().currentPoints += 1;
            GameManager.instance.uiManager.UpdateScoreboardUI(currentEnemy.GetComponent<PlayerInputs>().scoreBoardUI, 
            currentEnemy.GetComponent<PlayerInputs>().currentPoints);
        }    
    }

    #endregion

    // all of this is used for the combat. they have funciton in the other scripts that can be repurposed.
    #region Ablities

    public void PlayerStunned()
    {
        isStunned = true;
        canUseAbility = false;
        canMove = false;
        // noise and animation
        print("has been hit");
        StartCoroutine(stunCooldown());
    }

    IEnumerator stunCooldown()
    {
        yield return new WaitForSeconds(3f);
        attackObject.SetActive(false);
        isStunned = false;
        canUseAbility = true;
        canMove = true;
        print("done stun");
    }

    // proably doint even need to have an attack script, i think we just doa spehrecast infront of the player and use that to add forces. i have 
    // this from hamster wrangler probaly wont be hto ad to set up. 
    private void DoAttack(InputAction.CallbackContext obj)
    {

        // what happens if  a player is hit when they are sliding
        if (!canUseAbility)
            return;
        attackObject.SetActive(true);
        StartCoroutine(attackCooldown());
        canUseAbility = false;
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        attackObject.SetActive(false);
        canUseAbility = true;
    }

    private void DoLunge(InputAction.CallbackContext obj)
    {
        if (!canUseAbility)
            return;
        lungeObject.SetActive(true);
        StartCoroutine(lungeCooldown());
        canUseAbility = false;
    }

    IEnumerator lungeCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        lungeObject.SetActive(false);
        canUseAbility = true;
    }

    #region Blocking

    private void DoBlock(InputAction.CallbackContext obj)
    {
        if (!isBlocking && canUseAbility)
        {
          currentSpeed = 0f;
        // stops the player form using other ablities 
          isBlocking = true;
          StartCoroutine(UnblockCooldown());
          canUseAbility = false;      
        }
        else if (isBlocking && blockCooldownFinished)
        {
            currentSpeed = maxSpeed;
            // stops the player form using other ablities 
            isBlocking = false;
            blockCooldownFinished = false;
            canUseAbility = true;
        }    
    }

    // maybe do all this in the animations, want to make it so if the player blocks they are locked into the full aniamtion
    // beofre they unblock
    IEnumerator UnblockCooldown()
    {
        yield return new WaitForSeconds(exitBlockTimer);
        blockCooldownFinished = true;
    }
    #endregion

    private void DoDodge(InputAction.CallbackContext obj)
    {
        if (!canUseAbility)
            return;

        // used for locking slide in place and particle effect
        canMove = false;

        isDodging = true;

        // adds a force to the player, spped can be adjusted with dodgeMultiplier
        rb.AddForce(transform.forward * dodgeSpeedMultiplier, ForceMode.Impulse);

        StartCoroutine(DodgeCooldown());
        // makes it so player cnat use any other ability mid use
        canUseAbility = false;

    }

    // do this in a the aniamtion keyframes proabnly, at the end of it come out of dodge for best player expericance
    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(exitDodgeTimer);
        canMove = true;
        canUseAbility = true;
        isDodging = true;
    }

    // reusable function to reset ablitie bools, can only be used to set things true which would need to be taken into account
    // not sure if it is actaully viable as alot of this will be done in animations
    IEnumerator ResetAbilityCooldown(bool[] boolArray, float coolDownTimer)
    {
        yield return new WaitForSeconds(coolDownTimer);
        for (int x = 0; x < boolArray.Length; x++)
        {
            boolArray[x] = true;
        }
    }
    #endregion

    public void Movement()
    {
        if (Time.timeScale == 0)
            return;
           if (!canMove)
               return;

        //player movement, can only use vector2 for controller so we use a vector3
        // but store the x and z in a vector 2
        Vector2 inputVector = move.ReadValue<Vector2>();
        Vector3 tempVec = new Vector3(inputVector.x, 0, inputVector.y);

        // adds force to the vector, do this seperately so we can use
        //the variable for the player rotation
        rb.AddForce(tempVec * currentSpeed, ForceMode.Force);

        // this is weird, need the rotatetowards line for the keyboard input
        if (tempVec != Vector3.zero)
        {
            // finds the direction the player is moving
            Quaternion targetRotation = Quaternion.LookRotation(tempVec);
            // rotates players towards the way they are facing
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 270 * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
    }

    // when a player dashes at someone when they are blocking it breaks them out of the dodge.
    public void OnCollisionEnter(Collision col)
    {
        // for player respawning
        //  if (respawning == true)
        //     return;

        if (!isDodging)
            return;

        if (col.gameObject.tag == "Player")
            enemy = col.gameObject;
   
        if (enemy.GetComponent<PlayerInputs>().isBlocking)
        {
            enemy.GetComponent<PlayerInputs>().PlayerStunned();

            enemy.GetComponent<PlayerInputs>().StopCoroutine(unblockCoroutineVar);
            enemy.GetComponent<PlayerInputs>().currentSpeed = enemy.GetComponent<PlayerInputs>().maxSpeed;
            // stops the player form using other ablities 
            enemy.GetComponent<PlayerInputs>().isBlocking = false;
            enemy.GetComponent<PlayerInputs>().blockCooldownFinished = false;
            enemy.GetComponent<PlayerInputs>().canUseAbility = true;
            print("contact block broken");
        }
    }   
}




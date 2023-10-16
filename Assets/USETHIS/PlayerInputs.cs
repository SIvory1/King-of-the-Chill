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

   [SerializeField] Animator animator;

    //Abilities
    [Header("General Ablities")]
    bool canUseAbility;
    bool canMove;
    bool isStunned;

    [Header("Dodge")]
    [SerializeField] float dodgeSpeedMultiplier;
    bool isDodging;

    [Header("Attacking")]
    [SerializeField] GameObject attackObject;
    [SerializeField] GameObject lungeObject;

    [Header("Block")]
    GameObject enemy;
    IEnumerator unblockCoroutineVar;
    public bool isBlocking;
    bool blockCooldownFinished;
    [SerializeField] float blockCooldownTimer;

    [Header("Lunge")]
    [SerializeField] float lungeForceMultiplier;
    [SerializeField] float lungeReducedSpeed;

    [Header("Particle Effects")]
    [SerializeField] GameObject stunEffect;
    [SerializeField] GameObject slideEffect;

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
        canBeHit = true;
        blockCooldownFinished = false;
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

    // fall animation is so fucking bad bro omg why does it make you go in the air fucking prober bro
    public bool canBeHit;
    public void Respawn()
    {
        animator.SetTrigger("Fall");
        canBeHit = false;
        canUseAbility = false;
        StartCoroutine("RemoveIFrames");
    }

    IEnumerator RemoveIFrames()
    {
        yield return new WaitForSeconds(2f);
        canBeHit = true;
        canUseAbility = true;
    }

    #endregion


    #region Ablities

    // slow down stun anim and end it through keyframes
    #region Stun/Taken Dmg

    public void PlayerStunned()
    {
        isStunned = true;
        canUseAbility = false;
        canMove = false;
        // noise and animation
        animator.SetTrigger("Stun");

   

        stunEffect.SetActive(true);

        StartCoroutine(stunCooldown());
    }

    // makes sure every bool is set back to normal, because we do thing y keyframe, if they are
    // take dmage mid anim then it wont reach the keyframe, this makes sure that no weird
    // issue occur because of that 
    public void PlayerTakenDmg()
    {
        animator.SetTrigger("Take Damage");

        GameManager.instance.audioManager.TakenDmg();

        isStunned = false;
        canUseAbility = true;
        canMove = true;

        isBlocking = false;
        blockCooldownFinished = false;

        currentSpeed = maxSpeed;
        isDodging = false;
    }

    IEnumerator stunCooldown()
    {
        yield return new WaitForSeconds(2f);

        stunEffect.SetActive(false);

        //why is this line here?
        attackObject.SetActive(false);

        isStunned = false;
        canUseAbility = true;
        canMove = true;
    }
    #endregion

    #region Basic Attack

    private void DoAttack(InputAction.CallbackContext obj)
    {
        // what happens if  a player is hit when they are sliding
        if (!canUseAbility)
            return;
        animator.SetTrigger("Attack");
        canUseAbility = false;
    }

    //called in the aniamtion using keyframes

    public void StartAttackKeyframe()
    {
        attackObject.SetActive(true);
    }

    public void EndAttackKeyframe()
    {
        attackObject.SetActive(false);
        canUseAbility = true;
    }

    #endregion

    #region Lunge
    // the lunge is 2 seperate amiamtions, we first use charge and then at the end of 
    // the charge a keyframe calls the start lunge which also has keyframes 
    private void DoLunge(InputAction.CallbackContext obj)
    {
        if (!canUseAbility)
            return;
        animator.SetTrigger("Charge");
        currentSpeed = lungeReducedSpeed;
        canUseAbility = false;
    }

    // instead of doing a hitbox should it just be a raycast so that you cant swing it sideways and hit a 
    //player

    // this looks bad rn find a way to fix the animation tree as it onyl works if it comes from any state
    // needs to look like one long animation

    public void StartLungeKeyframe()
    {
        if (!isStunned)
        {
            animator.SetTrigger("Lunge");
            rb.AddForce(transform.forward * lungeForceMultiplier, ForceMode.Impulse);
            lungeObject.SetActive(true);
        }    
    }

    public void EndLungeKeyframe()
    {
        lungeObject.SetActive(false);
        currentSpeed = maxSpeed;
        canUseAbility = true;
        print("john");
    }
    #endregion

    #region Blocking

    private void DoBlock(InputAction.CallbackContext obj)
    {
        if (!isBlocking && canUseAbility)
        {
          currentSpeed = 0f;

        //  StartCoroutine(UnblockCooldown());
          animator.SetTrigger("Block");
          canUseAbility = false;
           print("block started");
        }
        else if (isBlocking) //&& blockCooldownFinished)
        {
           currentSpeed = maxSpeed;
            // stops the player form using other ablities 
            isBlocking = false;
            blockCooldownFinished = false;
           canUseAbility = true;

            print("block canceled");

            // block is one long animation we freeze it half way to pretned its a block
            animator.speed = 1f;
           // BlockCanceled(this.gameObject);
        }    
    }
    // *UFP stands for unidentified flying penguin as it can be either the player or the enemy
    void BlockCanceled(GameObject UFP)
    {
        print("block canceled");

        UFP.GetComponent<PlayerInputs>().currentSpeed = UFP.GetComponent<PlayerInputs>().maxSpeed;
        // stops the player form using other ablities 
        UFP.GetComponent<PlayerInputs>().isBlocking = false;
        UFP.GetComponent<PlayerInputs>().blockCooldownFinished = false;
        UFP.GetComponent<PlayerInputs>().canUseAbility = true;

        // block is one long animation we freeze it half way to pretned its a block
        UFP.GetComponent<PlayerInputs>().animator.speed = 1f;
    }




    // stops them from instanly leaving block
    IEnumerator UnblockCooldown()
    {
        yield return new WaitForSeconds(blockCooldownTimer);
        blockCooldownFinished = true;
        print("block middle");
    }

    public void BlockKeyframeStopAni()
    {
        // stops the player form using other ablities 
        isBlocking = true;
        animator.speed = 0f;
    }
    #endregion

    #region Dodge
    private void DoDodge(InputAction.CallbackContext obj)
    {
        if (!canUseAbility)
            return;

        // used for locking slide in place and particle effect
        canMove = false;

        isDodging = true;
        animator.SetTrigger("Slide");

        slideEffect.SetActive(true);

        // adds a force to the player, spped can be adjusted with dodgeMultiplier
        rb.AddForce(transform.forward * dodgeSpeedMultiplier, ForceMode.Impulse);

        // makes it so player cnat use any other ability mid use
        canUseAbility = false;
    }

    public void EndDodgeKeyframe()
    {
        canMove = true;
        canUseAbility = true;
        isDodging = false;
        slideEffect.SetActive(false);
    }
    #endregion

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
            animator.SetFloat("Walk", Mathf.Abs(rb.velocity.magnitude));
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
   
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerInputs>().isBlocking)
        {
            enemy.GetComponent<PlayerInputs>().PlayerStunned();
            enemy.GetComponent<PlayerInputs>().StopCoroutine(unblockCoroutineVar);
            BlockCanceled(enemy);
        }
    }   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMove1 : MonoBehaviour
{

    [Header("Default")]
    Rigidbody rb;
    public Animator animations;
    public ParticleSystem stunParticle;
    public GameObject audioManagerObject;
    AudioManager audioManager;
    public bool canUseAbility;
    public bool isFalling;

    [Header("Movement")]
    public float speed;
    public float baseSpeed;

    [Header("Dodge")]
    public float dodgeMultiplier;
    public float dodgeTimer;
    public float dodgeTimerMax;
    public int dodgeCheck;
    public float howLongDodgeLast;
    public bool isDodging;
    public GameObject enemyPlayer;
    public GameObject dodgeParticle;
    public bool isStunned = false;
    public float stunLockTimer;
    public float stunLockTimerEnd;
    public int isStunnedCounter;

    [Header("Attack")]
    public int attackReset;
    public float attackTimer;
    public float attackTimerResetMax;
    public float attackPlayingTimerEnd;
    public float lungeAttackLungeMultiplier;
    public GameObject attackParticle;
    public GameObject attackHitboxObject;
    public GameObject lungeAttackHitboxObject;

    [Header("Block")]
    public bool canBeHit;
    public bool isBlocking = false;
    public float reducedMovementSpeed;
    public bool startIsBlockingTimer;
    public float isBlockingTimer;
    public float isBlockingTimerMax;

    [Header("Invinciblity")]
    public bool respawning;
    public bool iFrames;
    public float iFramesTimer;
    public float iFramesTimerMax;
    public float matAlpha;

    public void Awake()
    {

        canUseAbility = true;

        // gets the gameobject wiothout attachign it in the inspector
        // cause it keeps undeclaring itself for some reason//
        audioManagerObject = GameObject.Find("Audio Manager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();

        rb = GetComponent<Rigidbody>();

        // freezes rotation
        rb.freezeRotation = true;

    }

    public void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (iFrames == true)
        {
            iFramesTimer += Time.deltaTime;

            if (iFramesTimer > iFramesTimerMax)
            {
                iFrames = false;
                respawning = false;
                iFramesTimer = 0;
            }
        }

        // dodge timers and reset
        if (dodgeCheck != 1)
            dodgeTimer += Time.deltaTime;

        if (dodgeTimer > dodgeTimerMax)
        {
            dodgeCheck += 1;
            dodgeTimer = 0;
        }

        // turns on the slide particle when sliding 
        if (isDodging == true)
            dodgeParticle.SetActive(enabled);
        else
            dodgeParticle.SetActive(false);

        // sets how long the EFFECT of the dash lasts for
        // not how long the player will move but how long 
        // they will be able to collide with another player 
        // and stun them
        if (dodgeTimer > howLongDodgeLast)
        {
            isDodging = false;
            canUseAbility = true;
        }
        // attack timers and reset
        if (attackReset != 1)
        {
            attackTimer += Time.deltaTime;
        }

        // after a certian amount of time the player attack goes away
        if (attackTimer > attackPlayingTimerEnd)
        {
            //attackParticle.SetActive(false);
            attackHitboxObject.SetActive(false);
            lungeAttackHitboxObject.SetActive(false);
            canUseAbility = true;
        }

        if (attackTimer > attackTimerResetMax)
        {
            attackReset += 1;
            attackTimer = 0;
        }

        // makes it so attacks dont go into the minuses
        if (attackReset < 0)
            attackReset = 0;

        // when player gets stunned it starts a timer where they cannot do any moves
        if (isStunned == true && isStunnedCounter == 1)
        {
            // for effects
            audioManager.PlayerStunned();
            stunParticle.Play();
            animations.SetTrigger("Stun");
            isStunnedCounter -= 1;
            speed = 0;


            stunLockTimer += Time.deltaTime;
            if (stunLockTimer > stunLockTimerEnd)
            {
                isStunned = false;
                stunLockTimer = 0f;
                speed = baseSpeed;
            }
        }
        else stunParticle.Stop();

        // chanegs between animations depending on how fast the player is moving 
        if (rb.velocity.magnitude > 4f)
            animations.SetFloat("Walk", Mathf.Abs(rb.velocity.magnitude));
        if (rb.velocity.magnitude < 4f)
            animations.SetFloat("Idle", Mathf.Abs(rb.velocity.magnitude));

        // when the player first presses the block button it will force
        // them to stay in block and start this timer
        if (startIsBlockingTimer == true)
        {
            isBlockingTimer += Time.deltaTime;
        }
        // when the timer is done the player is now allowed to unblock
        if (isBlockingTimer > isBlockingTimerMax)
        {
            isBlockingTimer = 0;
            isBlocking = true;
        }

        // was having error when you could attack and then block
        // but the attack timer would make canUseAblitity true
        // whioch means the user could not unblock. enelagant. Too bad!
        if (isBlocking == true)
            canUseAbility = false;

        // for new input

        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Dodge();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Block();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Lunge();
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        // for player respawning
        if (respawning == true)
            return;
        // if you hit a player

        if (col.gameObject.tag == "Player")
        {
            // sets the player hit as a gameobject that cabn be affected in script
            enemyPlayer = col.gameObject;
            // and in dodge, number picked just as an idea of how long the dash could last
            if (dodgeTimer > .1f && dodgeTimer < 1f && enemyPlayer.GetComponent<PlayerMove2>().isBlocking == true)
            {
                audioManager.PlayerStunHit();

                // sets all the vairables needed to stun the enemy player
                // use this stun counter so the stun aniamtion doesnt loop
                enemyPlayer.GetComponent<PlayerMove2>().isStunnedCounter += 1;
                enemyPlayer.GetComponent<PlayerMove2>().isStunned = true;
                enemyPlayer.GetComponent<PlayerMove2>().ForceUnblock();
            }
        }
    }

    public void FixedUpdate()
    {

        // calls movement function
        Movement();
    }

    public void Respawn()
    {
        respawning = true;
        iFrames = true;
        if (isBlocking == true)
            ForceUnblock();
    }

    public void Lunge()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        if (attackReset == 1 && isStunned == false && canUseAbility == true)
        {
            canUseAbility = false;
            animations.SetTrigger("Charge");
            speed = reducedMovementSpeed;
        }
    }

    public void LungeAttack()
    {
        // for lunge attack
        audioManager.PlayerLunge();
        animations.SetTrigger("Lunge");
        rb.AddForce(transform.forward * lungeAttackLungeMultiplier, ForceMode.Impulse);
        lungeAttackHitboxObject.SetActive(enabled);
        attackReset -= 1;
        speed = baseSpeed;
    }

    public void Attack()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        if (attackReset == 1 && isStunned == false && canUseAbility == true)
        {
            animations.SetTrigger("Attack");
            canUseAbility = false;
        }
    }

    public void AttackKeyframe()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        audioManager.PlayerAttack();
        attackHitboxObject.SetActive(enabled);
        attackReset -= 1;
    }

    public void Dodge()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        // checks if the player has a dodge 
        if (dodgeCheck == 1 && isStunned == false && canUseAbility == true)
        {
            canUseAbility = false;
            // used for lockign slide in place and particle effect
            isDodging = true;
            // adds a force to the player, spped can be adjusted with dodgeMultiplier
            rb.AddForce(transform.forward * dodgeMultiplier, ForceMode.Impulse);
            // removes dodge
            dodgeCheck -= 1;
            animations.SetTrigger("Slide");
            audioManager.PlayerSlide();
        }
    }

    public void Block()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        if (isStunned == false)
        {
            // when player blocks
            if (isBlocking == false && canUseAbility == true)
            {
                // starts a timer im update that makes it so the 
                //player cannot unblock instanly, done this for quality of life
                // but also it helps as was havign errors using a bool in here
                startIsBlockingTimer = true;
                canBeHit = false;
                speed = reducedMovementSpeed;
                animations.SetBool("Block", true);
                // stops the player form using other ablities 
                canUseAbility = false;
                audioManager.PlayerBlock();
            }

            if (isBlocking == true && canUseAbility == false)
            {
                // resets animation speed 
                animations.speed = 1f;
                // cancels above mentioned timer
                startIsBlockingTimer = false;
                isBlockingTimer = 0;
                canBeHit = true;
                speed = baseSpeed;
                animations.SetBool("Block", false);
                // makes it so the player can block again 
                isBlocking = false;
                // lets the player use other abilitys 
                canUseAbility = true;
                audioManager.PlayerBlock();
            }
        }
    }


    public void ForceUnblock()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        // resets animation speed 
        animations.speed = 1f;
        // cancels above mentioned timer
        startIsBlockingTimer = false;
        isBlockingTimer = 0;
        canBeHit = true;
        speed = baseSpeed;
        animations.SetBool("Block", false);
        // makes it so the player can block again 
        isBlocking = false;
        // lets the player use other abilitys 
        canUseAbility = true;
        audioManager.PlayerBlock();
        isStunned = false;
    }



    // this is called during the block anaiamtion to freeze it in position
    public void HoldBlock()
    {
        // for player respawning
        if (respawning == true)
            return;
        if (Time.timeScale == 0)
            return;
        animations.speed = 0f;
    }

    public void Movement()
    {
        if (Time.timeScale == 0)
            return;


        // used for locking slide in place, no rotation 
        if (isDodging == false)
        {

            float xDirection = Input.GetAxisRaw("Horizontal2");
            float yDirection = Input.GetAxisRaw("Vertical2");


            Vector3 tempVec = new Vector3(xDirection * 1, 0, yDirection * 1);

            // adds force to the vector, do this seperately so we can use
            //the variable for the player rotation
            rb.AddForce(tempVec * speed, ForceMode.Force);

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
}
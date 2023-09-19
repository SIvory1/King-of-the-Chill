using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Vector3 forceDirection = Vector3.zero;



    private void Awake()
    {      
        rb = this.GetComponent<Rigidbody>();

        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
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
        player.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        print("attackjhon");
    }

    private void DoBlock(InputAction.CallbackContext obj)
    {
        print("blockjohn");
    }

    private void DoLunge(InputAction.CallbackContext obj)
    {
        print("lungejohn");
    }

    private void DoDodge(InputAction.CallbackContext obj)
    {
        print("dodgejohn");
    }

    public void Movement()
    {
        if (Time.timeScale == 0)
            return;

                    //player movement, can only use vector2 for controller so we use a vector3
                    // but store the x and z in a vector 2
                    Vector2 inputVector = move.ReadValue<Vector2>();
                    Vector3 tempVec = new Vector3(inputVector.x, 0, inputVector.y);

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

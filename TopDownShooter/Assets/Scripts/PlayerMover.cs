using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [Powerable]
    public float speed = 6;
    [SerializeField] Animator animator;
    [SerializeField] InputMaster Controls;
    [SerializeField] PlayerInput playerInput;
    
    Rigidbody playerRigidbody;


    private float VerticalInput;
    private float HorizontalInput;
    private Camera camera;

    

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;
        playerInput = GetComponent<PlayerInput>();

    }

    private void FixedUpdate()
    {
        
        Move();
        //Turn();
    }


    public void getInputMove(InputAction.CallbackContext ctx)
    {
        HorizontalInput = ctx.ReadValue<Vector2>().x;
        VerticalInput = ctx.ReadValue<Vector2>().y;

    }

    public void Move()
    {
        Vector3 inputVector = new Vector3(HorizontalInput, 0f, VerticalInput);
        if(inputVector.magnitude != 0f)
        {
            animator.SetBool("IsWalking", true);
            Vector3 MovementVector = inputVector.normalized * speed * Time.deltaTime;
            //transform.position = transform.position + MovementVector;
            playerRigidbody.MovePosition(transform.position + MovementVector);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

    }

    public void ControlTurn(InputAction.CallbackContext ctx)
    {

        if (playerInput.currentControlScheme.Equals("Keyboard and Mouse"))
        {

            // Ray camRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Ray camRay = camera.ScreenPointToRay(ctx.ReadValue<Vector2>());
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, 100f, LayerMask.GetMask("Floor")))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                playerRigidbody.MoveRotation(newRotation);
            }
        }
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            
            Vector3 PointToLook = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
            if (PointToLook == Vector3.zero)
                return;
            //Vector3 PlayerToPoint = PointToLook - transform.position;
            //playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(PointToLook);
            playerRigidbody.MoveRotation(newRotation);
        }

    }

    /*
    private void Turn()
    {
        //Debug.Log(Controls.Player.LookPosition.ReadValue<Vector2>());
       // Ray camRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Ray camRay = camera.ScreenPointToRay(Controls.Player.LookPosition.ReadValue<Vector2>());
        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, 100f, LayerMask.GetMask("Floor")))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
            
    }
    */
}

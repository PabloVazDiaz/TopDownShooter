using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{

    [SerializeField] float speed = 6;
    [SerializeField] Animator animator;
    [SerializeField] InputMaster Controls;
    
    Rigidbody playerRigidbody;


    private float VerticalInput;
    private float HorizontalInput;
    private Camera camera;

    

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;


    }

    private void FixedUpdate()
    {
        Move();
        Turn();
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



    private void Turn()
    {
        
        Ray camRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Ray camRay = camera.ScreenPointToRay(Controls.Player.LookPosition.ReadValue<Vector2>());
        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, 100f, LayerMask.GetMask("Floor")))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
            
    }
}

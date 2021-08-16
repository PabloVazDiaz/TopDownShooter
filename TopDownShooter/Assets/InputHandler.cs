using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    [SerializeField] InputMaster controls;
    [SerializeField] PlayerMover playerMover;


    private void Awake()
    {
        //controls.Player.Shoot.performed += x => Shoot();
    }


    private void Update()
    {
        Vector3 MovementVector = new Vector3(controls.Player.HorizontalMove.ReadValue<float>(), 0f, controls.Player.VerticalMove.ReadValue<float>());
        
    }

    void Shoot()
    {

    }

    void Move(Vector2 moveVector)
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character_Controller;

    private Vector3 move_Direction;

    public float speed = 5f;
    private float gravity = 20f;

    public float jump_Force = 10;
    private float vertical_Verlocity;

    private void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }
    

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        move_Direction = transform.TransformDirection(move_Direction);

        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);
    } // moving player
    void ApplyGravity()
    {
        vertical_Verlocity -= gravity * Time.deltaTime;


        // jump the player
        PlayerJump();
        

        move_Direction.y = vertical_Verlocity * Time.deltaTime;

    }// Gravity 
    void PlayerJump()
    {
        if (character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Verlocity = jump_Force;
        }
    }
} // class end









































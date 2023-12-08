using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
    private CharacterController characterController;
    private float playerSpeed = 25.0f;
    private float rotationSpeed = 150f;

    float jumpForce = 5;
    Boolean doubleJump;
    float gravityModifier = 1.5f;
    float yVelocity = 0;




    public GameObject cameraObject;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //gravity
        if (!characterController.isGrounded)
        {
            yVelocity += Physics.gravity.y * gravityModifier * Time.deltaTime;

        }
        if (characterController.isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            doubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (characterController.isGrounded || doubleJump)
            {
                yVelocity = jumpForce;
                doubleJump = !doubleJump;
            }
        }



        //move character controller
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move.y = yVelocity;
        characterController.Move(move * Time.deltaTime * playerSpeed);


        //rotate character
        float yRotationLeft = -1 * rotationSpeed * Time.deltaTime;
        float yRotationRight = 1 * rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, yRotationLeft, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, yRotationRight, 0, Space.Self);
        }

        //follow camera
        Vector3 newCamPos = transform.position + -transform.forward * 30 + Vector3.up * 8;
        cameraObject.transform.position = newCamPos;
        cameraObject.transform.LookAt(transform);

    }
}

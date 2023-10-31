using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    private float playerSpeed = 25.0f;
    private float rotationSpeed = 150f;

    float jumpForce = 5;
    Boolean doubleJump;
    float gravityModifier = 1.5f;
    float yVelocity = 0;




    public GameObject cameraObject;
    public GameObject skullshotPrefab;
    public Transform respawnPoint;

    public AudioSource source;
    public AudioClip unlock;
    public AudioClip obtained;
    public AudioClip fail;
    public AudioClip victory;


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
        if (characterController.isGrounded &&!Input.GetKeyDown(KeyCode.Space)) 
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

        // skull shot
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 front = transform.position + transform.forward * 5 +Vector3.up * 5;
            GameObject ball = Instantiate(skullshotPrefab, front, transform.rotation);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(ball.transform.forward * 2500);
            Destroy(ball, 3);
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if (hit.gameObject.tag == ("Pumpkin"))
        {
            transform.position = respawnPoint.position;
            //this doesn't actually work as expected
            //character controller does not react to other objects hitting it
            //only when the controller walks into an object
            source.PlayOneShot(fail);
        }
        if (hit.gameObject.tag == ("key"))
        {
            Destroy(hit.gameObject);
            source.PlayOneShot(unlock);
            source.PlayOneShot(obtained);
            GameObject[] gos = GameObject.FindGameObjectsWithTag("gate");
            foreach (GameObject go in gos)
                Destroy(go);
        }
        if (hit.gameObject.tag == ("prize"))
        { 
            transform.position = respawnPoint.position;
            source.PlayOneShot(victory);
        }

    }

}

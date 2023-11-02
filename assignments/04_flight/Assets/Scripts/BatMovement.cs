using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    [SerializeField] private float speed, levitationSpeed;

    private CharacterController characterController;
    private Vector3 moveDirection;
    public GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fly();
        //follow camera
        Vector3 newCamPos = transform.position + -transform.forward * 30 + Vector3.up * 10;
        cameraObject.transform.position = newCamPos;
        cameraObject.transform.LookAt(transform);

    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 move = transform.forward * verticalInput + transform.right * horizontalInput;
        characterController.Move(move);
    }

    private void Fly()
    {
        moveDirection = Vector3.up * levitationSpeed *Time.deltaTime;

        if(Input.GetKey(KeyCode.Space))
        {
            characterController.Move(moveDirection);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            characterController.Move(-moveDirection);
        }
    }
}

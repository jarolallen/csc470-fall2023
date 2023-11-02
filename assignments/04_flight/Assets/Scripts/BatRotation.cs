using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRotation : MonoBehaviour
{
    [SerializeField] private float mouseSense;
    [SerializeField] private Transform playerBody;

    private float xRotation, yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") *mouseSense *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float speed = 100;
    
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float yRotationLeft = -1 * speed * Time.deltaTime;
        float yRotationRight = 1 * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, yRotationLeft, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, yRotationRight, 0, Space.Self);
        }
    }

}

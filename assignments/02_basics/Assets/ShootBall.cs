using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    Rigidbody tumbleweed;

    // Start is called before the first frame update
    void Start()
    {
        tumbleweed = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tumbleweed.AddForce(0, 0, 500, ForceMode.Impulse);
        }
    }
}

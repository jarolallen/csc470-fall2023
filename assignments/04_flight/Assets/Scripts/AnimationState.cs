using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D)) 
        {
            animator.SetBool("isRunning", true);
        }
        else
        //if (!Input.GetKeyDown(KeyCode.W) | !Input.GetKeyDown(KeyCode.A) | !Input.GetKeyDown(KeyCode.S) | !Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isRunning", false);
        }
    }
}
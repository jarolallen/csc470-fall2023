using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLaunch : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform launcherTransform;

    public AudioSource source;
    public AudioClip clip;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ball = Instantiate(ballPrefab, launcherTransform.position, launcherTransform.rotation);
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();
            ballRB.AddForce(launcherTransform.forward * 1000);
            source.PlayOneShot(clip);
            Destroy(ball, 3);
        }

    }

}

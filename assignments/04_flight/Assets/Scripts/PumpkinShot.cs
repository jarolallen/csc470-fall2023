using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinShot : MonoBehaviour
{
    public GameObject pumpkinPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootPumpkin", 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShootPumpkin()
    {
        Vector3 front = transform.position + transform.forward * 3 + Vector3.up * 1;
        GameObject ball = Instantiate(pumpkinPrefab, front, transform.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(ball.transform.forward * 4000);
        Destroy(ball, 5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("skull"))
        {
            Destroy(this.gameObject);
        }

    }
}

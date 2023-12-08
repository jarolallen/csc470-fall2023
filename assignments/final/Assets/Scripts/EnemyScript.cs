using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public CharacterController cc;
    public Animator animator;

    public GameObject attackPrefab;


    float moveSpeed = 10;


    public AudioSource source;
    public AudioClip despawnEnemySound;
    public AudioClip enemyAttackSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemyTargetLogic();
        
    }
    public GameObject FindClosestGatherer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("gatherer");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        Debug.Log(closest);
        return closest;

    }
    void EnemyTargetLogic()
    {
        Vector3 amountToMove = Vector3.zero;
        Vector3 closeGatherer = FindClosestGatherer().transform.position;

        Vector3 vectorToTarget = (closeGatherer - transform.position).normalized;

        float step = 5 * Time.deltaTime;
        Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
        rotatedTowardsVector.y = 0;
        transform.forward = rotatedTowardsVector;

        amountToMove = transform.forward * moveSpeed * Time.deltaTime;
        cc.Move(amountToMove);

        //animator.SetFloat("speed", amountToMove.magnitude);
        bool walking = false;
        if (amountToMove.magnitude > 0)
        {
            walking = true;
        }
        animator.SetBool("walking", walking);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public CharacterController cc;
    public Animator animator;

    public GameObject attackPrefab;


    float moveSpeed = 20;


    public AudioSource source;
    public AudioClip despawnEnemySound;
    public AudioClip enemyAttackSound;

    public int damage = 5;
    //public float attackCooldown;
    //float _lastAttackTime;
    Coroutine _attackInProgress;
    int _playerContacts;

    float gravityModifier = 1f;
    float yVelocity = 0;



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
        //Invoke("EnemyTargetLogic", 2.0f);
        //Invoke("CheckEnemyForward", 2.0f);
        EnemyTargetLogic();
        CheckEnemyForward();
        if (!cc.isGrounded)
        {
            yVelocity += Physics.gravity.y * gravityModifier * Time.deltaTime;

        }
        
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
        return closest;
    }
    public GameObject FindClosestPlayerBase()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("playerbase");
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
        return closest;
    }

    void EnemyTargetLogic()
    {
        Vector3 amountToMove = Vector3.zero;
        GameObject closega = FindClosestGatherer();
        if (closega != null)
        {
            Vector3 closeGatherer = closega.transform.position;
            Vector3 vectorToTarget = (closeGatherer - transform.position).normalized;

            float step = 5 * Time.deltaTime;
            Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
            rotatedTowardsVector.y = 0;
            transform.forward = rotatedTowardsVector;

            amountToMove = transform.forward * moveSpeed * Time.deltaTime;
            amountToMove.y = yVelocity;
            cc.Move(amountToMove);
        }
        if (closega == null)
        {
            Vector3 closePlayerBase = FindClosestPlayerBase().transform.position;
            Vector3 vectorToTarget = (closePlayerBase - transform.position).normalized;

            float step = 5 * Time.deltaTime;
            Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
            rotatedTowardsVector.y = 0;
            transform.forward = rotatedTowardsVector;

            amountToMove = transform.forward * moveSpeed * Time.deltaTime;
            amountToMove.y = yVelocity;
            cc.Move(amountToMove);
        }
        

        //animator.SetFloat("speed", amountToMove.magnitude);
        bool walking = false;
        if (amountToMove.magnitude > 0)
        {
            walking = true;
        }
        animator.SetBool("walking", walking);
    }
    void CheckEnemyForward()
    {
        float castDistance = 10;
        Vector3 positionToRayCastFrom = transform.position + Vector3.up * 1.8f;
        Ray ray = new Ray(positionToRayCastFrom, transform.forward);
        Debug.DrawRay(positionToRayCastFrom, transform.forward * castDistance, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, castDistance))
        {
            if (hit.collider.tag == "gatherer")
            {
                animator.SetTrigger("attack");
                GameObject target = hit.collider.gameObject;
                target.GetComponent<UnitScript>().ResetUnitState();
            }
            if (hit.collider.tag == "playerbase")
            {              
                _playerContacts++;
                if (_attackInProgress == null) 
                    _attackInProgress = StartCoroutine(AttackLoop());
                /*GameObject target = hit.collider.gameObject;
                // Abort if we already attacked recently.
                if (Time.time - _lastAttackTime < attackCooldown) return;

                if (target.gameObject.CompareTag("playerbase"))
                {
                    GameManager.SharedInstance.playerBaseHP -= damage;

                    // Remember that we recently attacked.
                    _lastAttackTime = Time.time;
                }*/
            }
        }
    }

    public void ResetUnitState()
    {
        animator.SetTrigger("death");
        Invoke("deactivateFunction", 1);
    }
    private IEnumerator AttackLoop()
    {
        while (_playerContacts > 0)
        {
            GameManager.SharedInstance.playerBaseHP -= damage;
            animator.SetTrigger("attack");

            // You can construct this once and cache it if you like.
            yield return new WaitForSeconds(2);
        }
        _attackInProgress = null;
    }

    void deactivateFunction()
    {
        gameObject.SetActive(false);
    }
}

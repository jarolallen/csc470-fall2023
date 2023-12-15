using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public Renderer bodyRenderer;
    public CharacterController cc;
    public Animator animator;

    public GameObject gatherPrefab;

    public Color selectedColor;
    public Color hoverColor;
    Color defaultColor;

    float moveSpeed = 20;

    bool hover = false;
    public bool selected = false;

    Vector3 target;
    bool hasTarget = false;

    private int resources = 0;
    private bool hasResource = false;


    public AudioSource source;
    public AudioClip despawnUnitSound;
    public AudioClip gatherTreeSound;

    public int damage = 5;
    Coroutine _attackInProgress;
    int _enemyContacts;
    float gravityModifier = 1f;
    float yVelocity = 0;


    // Start is called before the first frame update
    void Start()
    {
        defaultColor = bodyRenderer.material.color;
        GameManager.SharedInstance.units.Add(this);
        animator = GetComponent<Animator>();

    }

    private void OnDestroy()
    {
        GameManager.SharedInstance.units.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Invoke("GathererBehavior", 1f);
        //Invoke("AttackerBehavior", 1f);
        GathererBehavior();
        AttackerBehavior();
        if (!cc.isGrounded)
        {
            yVelocity += Physics.gravity.y * gravityModifier * Time.deltaTime;

        } 
    }

    public void SetTarget(Vector3 t)
    {
        target = t;
        hasTarget = true;
    }

    private void OnMouseDown()
    {
        GameManager.SharedInstance.SelectUnit(this);
        SetUnitColor();
    }

    private void OnMouseEnter()
    {
        hover = true;
        SetUnitColor();
    }

    private void OnMouseExit()
    {
        hover = false;
        SetUnitColor();
    }



    public void SetUnitColor()
    {
        if (selected)
        {
            bodyRenderer.material.color = selectedColor;
        }
        else if (hover)
        {
            bodyRenderer.material.color = hoverColor;
        }
        else
        {
            bodyRenderer.material.color = defaultColor;
        }
    }


    void checkInventory()
    {
        if (resources >= 20)
        {
            hasResource = true;
        }
    }
    void moveSawmill()
    {
        if (hasResource)
        {
            Vector3 sawmill = (FindClosestSawmill()).transform.position;
            SetTarget(sawmill);
        }
        
    }

    public GameObject FindClosestSawmill()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("sawmill");
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

    public GameObject FindClosestTree()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("tree");
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
    



    void CheckGatherForward()
    {
        float castDistance = 10;
        Vector3 positionToRayCastFrom = transform.position + Vector3.up * 3f;
        Ray ray = new Ray(positionToRayCastFrom, transform.forward);
        Debug.DrawRay(positionToRayCastFrom, transform.forward * castDistance, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, castDistance))
        {
            // If we get in here, we hit something
            // 'hit' contains information about what we hit
            //Destroy(hit.collider.gameObject);
            if (hit.collider.tag == "tree" && !hasResource)
            {
                animator.SetTrigger("gather");
                resources += 5;
                Debug.Log(resources);
                GameObject gather = Instantiate(gatherPrefab, hit.transform.position, Quaternion.identity);
                Destroy(gather, 1.5f);
                Destroy(hit.collider.gameObject);
            }
            if (hit.collider.tag == "sawmill")
            {
                GameManager.SharedInstance.totalResources += resources;
                Debug.Log(GameManager.SharedInstance.totalResources + " is total and " + resources +"is personal");
                ResetUnitState();
                
            }
            if (hit.collider.tag == "golem")
            {
                animator.SetTrigger("gather");  
                //GameObject gather = Instantiate(gatherPrefab, hit.transform.position, Quaternion.identity);
                //Destroy(gather, 1.5f);
                GameObject target = hit.collider.gameObject;
                target.GetComponent<GolemScript>().ResetUnitState();
            }

        }
    }

    void CheckAttackerForward()
    {
        float castDistance = 10;
        Vector3 positionToRayCastFrom = transform.position + Vector3.up * 1.8f;
        Ray ray = new Ray(positionToRayCastFrom, transform.forward);
        Debug.DrawRay(positionToRayCastFrom, transform.forward * castDistance, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, castDistance))
        {
            if (hit.collider.tag == "enemy")
            {
                animator.SetTrigger("attack");
                GameObject target = hit.collider.gameObject;
                target.GetComponent<EnemyScript>().ResetUnitState();
            }
            if (hit.collider.tag == "enemybase")
            {
                _enemyContacts++;
                if (_attackInProgress == null)
                    _attackInProgress = StartCoroutine(AttackLoop());
            }
        }
    }

    void GathererBehavior()
    {
        if (this.tag == "gatherer")
        {
            Vector3 amountToMove = Vector3.zero;

            if (hasTarget)
            {
                Vector3 vectorToTarget = (target - transform.position).normalized;

                float step = 5 * Time.deltaTime;
                Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                rotatedTowardsVector.y = 0;
                transform.forward = rotatedTowardsVector;

                amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                amountToMove.y = yVelocity;
                cc.Move(amountToMove);

                if (Vector3.Distance(transform.position, target) < 0.01f)
                {
                    hasTarget = false;
                }
            }
            if (!hasTarget)
            {

                GameObject attacktarget = FindClosestGolem();
                if (attacktarget != null)
                {
                    Vector3 closeEnemy = attacktarget.transform.position;
                    Vector3 vectorToTarget = (closeEnemy - transform.position).normalized;

                    float step = 5 * Time.deltaTime;
                    Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                    rotatedTowardsVector.y = 0;
                    transform.forward = rotatedTowardsVector;

                    amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                    amountToMove.y = yVelocity;
                    cc.Move(amountToMove);
                    if (Vector3.Distance(transform.position, target) < 0.01f)
                    {
                        hasTarget = false;
                    }
                }
                if (attacktarget == null)
                {
                    Vector3 closeEnemy = FindClosestTree().transform.position;
                    Vector3 vectorToTarget = (closeEnemy - transform.position).normalized;

                    float step = 5 * Time.deltaTime;
                    Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                    rotatedTowardsVector.y = 0;
                    transform.forward = rotatedTowardsVector;

                    amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                    amountToMove.y = yVelocity;
                    cc.Move(amountToMove);
                    if (Vector3.Distance(transform.position, target) < 0.01f)
                    {
                        hasTarget = false;
                    }
                }
                bool walking = false;
                if (amountToMove.magnitude > 0)
                {
                    walking = true;
                }
                animator.SetBool("walking", walking);
            }
            CheckGatherForward();
            checkInventory();
            moveSawmill();

        }
    }
    void AttackerBehavior()
    {
        if (this.tag == "attacker")
        {
            Vector3 amountToMove = Vector3.zero;

            if (hasTarget)
            {
                Vector3 vectorToTarget = (target - transform.position).normalized;

                float step = 5 * Time.deltaTime;
                Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                rotatedTowardsVector.y = 0;
                transform.forward = rotatedTowardsVector;

                amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                amountToMove.y = yVelocity;
                cc.Move(amountToMove);

                if (Vector3.Distance(transform.position, target) < 0.01f)
                {
                    hasTarget = false;
                }
            }
            if (!hasTarget)
            {

                GameObject attacktarget = FindClosestEnemy();
                if (attacktarget != null)
                {
                    Vector3 closeEnemy = attacktarget.transform.position;
                    Vector3 vectorToTarget = (closeEnemy - transform.position).normalized;

                    float step = 5 * Time.deltaTime;
                    Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                    rotatedTowardsVector.y = 0;
                    transform.forward = rotatedTowardsVector;

                    amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                    amountToMove.y = yVelocity;
                    cc.Move(amountToMove);
                    if (Vector3.Distance(transform.position, target) < 0.01f)
                    {
                        hasTarget = false;
                    }
                }
                if (attacktarget == null)
                {
                    Vector3 closeEnemy = FindClosestEnemyBase().transform.position;
                    Vector3 vectorToTarget = (closeEnemy - transform.position).normalized;

                    float step = 5 * Time.deltaTime;
                    Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
                    rotatedTowardsVector.y = 0;
                    transform.forward = rotatedTowardsVector;

                    amountToMove = transform.forward * moveSpeed * Time.deltaTime;
                    amountToMove.y = yVelocity;
                    cc.Move(amountToMove);
                    if (Vector3.Distance(transform.position, target) < 0.01f)
                    {
                        hasTarget = false;
                    }
                }
            }
            bool walking = false;
            if (amountToMove.magnitude > 0)
            {
                walking = true;
            }
            animator.SetBool("walking", walking);
            CheckAttackerForward();
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("enemy");
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
    public GameObject FindClosestEnemyBase()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("enemybase");
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
    public GameObject FindClosestGolem()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("golem");
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
    private IEnumerator AttackLoop()
    {
        while (_enemyContacts > 0)
        {
            GameManager.SharedInstance.enemyBaseHP -= damage;
            animator.SetTrigger("attack");

            // You can construct this once and cache it if you like.
            yield return new WaitForSeconds(2);
        }
        _attackInProgress = null;
    }

    public void ResetUnitState ()
    {
        resources = 0;
        hasResource = false;
        hasTarget = false;
        Invoke("deactivateFunction", 1);
    }
    void deactivateFunction ()
    {
        gameObject.SetActive(false);
    }

    

}

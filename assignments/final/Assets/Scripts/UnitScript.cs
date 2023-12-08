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

    float moveSpeed = 10;

    bool hover = false;
    public bool selected = false;

    Vector3 target;
    bool hasTarget = false;

    private int resources = 0;
    private bool hasResource = false;

    public int HP = 10;

    public AudioSource source;
    public AudioClip despawnUnitSound;
    public AudioClip gatherTreeSound;

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


        TargetLogic();
        GathererBehavior();
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
        float castDistance = 15;
        Vector3 positionToRayCastFrom = transform.position + Vector3.up * 1.8f;
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
                animator.SetTrigger("full");
                GameManager.SharedInstance.totalResources += resources;
                resources = 0;
                hasResource = false;
                Debug.Log(GameManager.SharedInstance.totalResources + " is total and " + resources +"is personal");
                hasTarget = false;
                Invoke("deactivateFunction", 1);
                
            }

        }
    }

    void TargetLogic()
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
            cc.Move(amountToMove);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                hasTarget = false;
            }
        }
        if (!hasTarget && this.tag == "gatherer")
        {
            //float x = UnityEngine.Random.Range(10, 990);
            //float z = UnityEngine.Random.Range(10, 990);
            //Vector3 randomTar = new Vector3(x, 0, z);
            Vector3 closeTree = FindClosestTree().transform.position;

            Vector3 vectorToTarget = (closeTree - transform.position).normalized;

            float step = 5 * Time.deltaTime;
            Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
            rotatedTowardsVector.y = 0;
            transform.forward = rotatedTowardsVector;

            amountToMove = transform.forward * moveSpeed * Time.deltaTime;
            cc.Move(amountToMove);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                hasTarget = false;
            }
        }
        if (!hasTarget && this.tag == "attacker")
        {
            //float x = UnityEngine.Random.Range(10, 990);
            //float z = UnityEngine.Random.Range(10, 990);
            //Vector3 randomTar = new Vector3(x, 0, z);
            Vector3 closeEnemy = FindClosestEnemy().transform.position;

            Vector3 vectorToTarget = (closeEnemy - transform.position).normalized;

            float step = 5 * Time.deltaTime;
            Vector3 rotatedTowardsVector = Vector3.RotateTowards(transform.forward, vectorToTarget, step, 1);
            rotatedTowardsVector.y = 0;
            transform.forward = rotatedTowardsVector;

            amountToMove = transform.forward * moveSpeed * Time.deltaTime;
            cc.Move(amountToMove);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                hasTarget = false;
            }
        }

        //animator.SetFloat("speed", amountToMove.magnitude);
        bool walking = false;
        if (amountToMove.magnitude > 0)
        {
            walking = true;
        }
        animator.SetBool("walking", walking);
    }
    void GathererBehavior()
    {
        if (this.tag == "gatherer")
        {
            CheckGatherForward();
            checkInventory();
            moveSawmill();

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
    void deactivateFunction ()
    {
        gameObject.SetActive(false);
    }

}

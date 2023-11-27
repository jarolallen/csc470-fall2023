using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static event Action<UnitScript> UnitSelectedHappened;

    public static GameManager SharedInstance;
    public List<UnitScript> units = new List<UnitScript>();


    public GameObject unitPrefab;
    

    UnitScript selectedUnit;

    public int totalResources;
    public bool enoughResources = false;

    public bool spawnSawmill = false;

    public GameObject spawnPrefab;

    public GameObject treePrefab;
    private float numbTrees = 500;


    void Awake()
    {
        if (SharedInstance != null)
        {
            Debug.Log("wut");
        }
        SharedInstance = this;

    }

    void Start()
    {
        generateTrees();
    }

    // Update is called once per frame
    void Update()
    {
        MoveUnit();
        SpawnUnit();
        countResource();
        
    }

    public void SelectUnit(UnitScript unit)
    {
        // Deselect any units that think they are selected
        foreach (UnitScript u in units)
        {
            u.selected = false;
            u.SetUnitColor();
        }
        selectedUnit = unit;
        selectedUnit.selected = true;
        selectedUnit.SetUnitColor();

        UnitSelectedHappened?.Invoke(unit);
    }

     void generateTrees()
    {
        for (int i = 0; i < numbTrees; i++)
        {
            float x = UnityEngine.Random.Range(10, 990);
            float z = UnityEngine.Random.Range(10, 990);
            Vector3 pos = new Vector3(x, 10, z);
            GameObject floorObj = Instantiate(treePrefab, pos, Quaternion.identity);
        }
        
    }

    void countResource()
    {
        if (totalResources >= 100)
            enoughResources = true;
    }
    void MoveUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999999))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.SetTarget(hit.point);
                    }
                }
            }
        }
    }

    void SpawnUnit()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999999))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    //Instantiate(unitPrefab, hit.point, Quaternion.identity);
                    GameObject unit = ObjectPool.instance.GetPooledObject();

                    if (unit != null)
                    {
                        unit.transform.position = hit.point;
                        unit.SetActive(true);
                        GameObject spawn = Instantiate(spawnPrefab, unit.transform.position + Vector3.up * 3, Quaternion.identity);
                        spawn.transform.localScale = new Vector3(6, 6, 6); // change its local scale in x y z format

                        Destroy(spawn, 2f);
                    }
                }
            }
        }
    }

}

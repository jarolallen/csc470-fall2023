using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action<UnitScript> UnitSelectedHappened;

    public static GameManager SharedInstance;
    public List<UnitScript> units = new List<UnitScript>();


    public GameObject unitPrefab;
    public GameObject sawmillPrefab;
    public TMP_Text totalResourceTxt;
    public TMP_Text currentUnitsTxt;

    public AudioSource source;
    public AudioClip spawnUnitSound;
    public AudioClip spawnSawmillSound;


    UnitScript selectedUnit;

    public int totalResources;
    public bool enoughResources = false;
    public bool spawnSawmill = false;

    public GameObject spawnPrefab;
    public GameObject spawnSawmillPrefab;

    public GameObject treePrefab;
    private float numbTrees = 500;

    public float playerBaseHP = 100;
    public float enemyBaseHP = 100;

    Coroutine _spawnInProgress;

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
        SpawnAttacker();
        countResource();
        SpawnSawmill();
        UpdateResourceDisplay();
        healthReport();
        if (_spawnInProgress == null)
            _spawnInProgress = StartCoroutine(SpawnEnemy());

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
        {
            enoughResources = true;
        }
            
        if (totalResources < 100)
        {
            enoughResources = false;
        }
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
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999999))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    GameObject unit = ObjectPool.instance.GetPooledObject();

                    if (unit != null)
                    {
                        unit.transform.position = hit.point;
                        unit.SetActive(true);
                        source.PlayOneShot(spawnUnitSound);
                        GameObject spawn = Instantiate(spawnPrefab, unit.transform.position + Vector3.up * 3, Quaternion.identity);
                        spawn.transform.localScale = new Vector3(6, 6, 6); // change its local scale in x y z format
                        Destroy(spawn, 0.25f);
                    }
                }
            }
        }
    }
    void SpawnAttacker()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift) && (totalResources >= 20))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999999))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    //Instantiate(unitPrefab, hit.point, Quaternion.identity);
                    GameObject unit = ObjectPool.instance.GetAttackerPooledObject();

                    if (unit != null)
                    {
                        unit.transform.position = hit.point;
                        unit.SetActive(true);
                        source.PlayOneShot(spawnUnitSound);
                        totalResources -= 20;
                        GameObject spawn = Instantiate(spawnPrefab, unit.transform.position + Vector3.up * 3, Quaternion.identity);
                        spawn.transform.localScale = new Vector3(6, 6, 6); // change its local scale in x y z format
                        Destroy(spawn, 0.25f);
                    }
                }
            }
        }
    }

    void SpawnSawmill()
    {
        if (Input.GetKey(KeyCode.Space) && enoughResources)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999999))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    Instantiate(sawmillPrefab, hit.point + Vector3.up * 15, Quaternion.identity);
                    source.PlayOneShot(spawnSawmillSound);
                    totalResources -= 100;
                    GameObject dust = Instantiate(spawnSawmillPrefab, hit.point + Vector3.up * 10, Quaternion.identity);
                    dust.transform.localScale = new Vector3(40, 40, 20);
                    Destroy(dust, 2f);
                    Debug.Log("sawmill done");
                }
            }
        }
    }
    
    private IEnumerator SpawnEnemy()
    {
        while (enemyBaseHP >= 0)
        {

            GameObject enemy = ObjectPool.instance.GetEnemyPooledObject();
            GameObject golem = ObjectPool.instance.GetGolemPooledObject();
            float x = UnityEngine.Random.Range(500, 800);
            float z = UnityEngine.Random.Range(500, 800);
            float a = UnityEngine.Random.Range(500, 800);
            float b = UnityEngine.Random.Range(500, 800);
            Vector3 pos1 = new Vector3(x, 0, z);
            Vector3 pos2 = new Vector3(a, 0, b);
            enemy.transform.position = pos1;
            enemy.SetActive(true);
            golem.transform.position = pos2;
            golem.SetActive(true);
            yield return new WaitForSeconds(10);
        }
        _spawnInProgress = null;
    }

    void UpdateResourceDisplay ()
    {
        totalResourceTxt.text = "Current Resources: "+ totalResources.ToString();
        currentUnitsTxt.text = "Active Units: "+ ObjectPool.instance.ActivePool.ToString();
    }

    void healthReport()
    {
        Debug.Log("Playerbase hp: "+playerBaseHP + "Enemybase hp: " + enemyBaseHP);
        if (playerBaseHP <= 0)
        {
            Debug.Log("GameOver");
        }
        if (enemyBaseHP <= 0)
        {
            Debug.Log("You Win!");
        }
    }



}

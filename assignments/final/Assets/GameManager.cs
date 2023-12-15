using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<UnitScript> UnitSelectedHappened;

    public static GameManager SharedInstance;
    public List<UnitScript> units = new List<UnitScript>();


    public GameObject unitPrefab;
    public GameObject sawmillPrefab;
    public TMP_Text totalResourceTxt;
    public TMP_Text currentUnitsTxt;
    public TMP_Text currentAttackersTxt;
    public TMP_Text playerHPTxt;
    public TMP_Text enemyHPTxt;

    public AudioSource source;
    public AudioClip spawnUnitSound;
    public AudioClip spawnSawmillSound;
    public AudioClip spawnEnemySound;


    UnitScript selectedUnit;

    public int totalResources;
    public bool enoughResources = false;
    public bool spawnSawmill = false;

    public GameObject spawnPrefab;
    public GameObject spawnSawmillPrefab;
    public GameObject enemySpawnPrefab;

    public GameObject treePrefab;
    private float numbTrees = 200;

    [System.NonSerialized]  public float playerBaseHP = 100;
    [System.NonSerialized]  public float enemyBaseHP = 100;

    Coroutine _spawnInProgress;


    public Animator playerKing;
    public Animator enemyKing;

    float spawntimer = 5; //controls how fast enemies are summoned

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
        generateTreesA();
        generateTreesB();
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

     void generateTreesA()
    {
        for (int i = 0; i < numbTrees; i++)
        {
            float x = UnityEngine.Random.Range(0, 500);
            float z = UnityEngine.Random.Range(500, 1000);
            Vector3 pos = new Vector3(x, 10, z);
            GameObject floorObj = Instantiate(treePrefab, pos, Quaternion.identity);
        }
        
    }
    void generateTreesB()
    {
        for (int i = 0; i < numbTrees; i++)
        {
            float x = UnityEngine.Random.Range(500, 1000);
            float z = UnityEngine.Random.Range(0, 500);
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
        if (enemyBaseHP >= 0)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                GameObject enemy = ObjectPool.instance.GetEnemyPooledObject();
                if (enemy != null)
                {
                    float x = UnityEngine.Random.Range(400, 650);
                    float z = UnityEngine.Random.Range(400, 650);
                    Vector3 pos1 = new Vector3(x, 0, z);
                    source.PlayOneShot(spawnEnemySound);
                    GameObject spawn = Instantiate(enemySpawnPrefab, pos1 + Vector3.up * 3, Quaternion.identity);
                    spawn.transform.localScale = new Vector3(6, 6, 6);
                    Destroy(spawn, 1f);
                    enemy.transform.position = pos1;
                    enemy.SetActive(true);
                }
            }
            if (random == 1)
            {
                GameObject golem = ObjectPool.instance.GetGolemPooledObject();
                if (golem != null)
                {
                    float x = UnityEngine.Random.Range(400, 650);
                    float z = UnityEngine.Random.Range(400, 650);
                    Vector3 pos1 = new Vector3(x, 0, z);
                    source.PlayOneShot(spawnEnemySound);
                    GameObject spawn = Instantiate(enemySpawnPrefab, pos1 + Vector3.up * 6, Quaternion.identity);
                    spawn.transform.localScale = new Vector3(12, 12, 12);
                    Destroy(spawn, 1f);
                    golem.transform.position = pos1;
                    golem.SetActive(true);
                }
            }
            yield return new WaitForSeconds(spawntimer);
        }
        _spawnInProgress = null;
    }

    void UpdateResourceDisplay ()
    {
        totalResourceTxt.text = "Current Resources: "+ totalResources.ToString();
        currentUnitsTxt.text = "Active Gatherers: "+ ObjectPool.instance.ActivePool.ToString();
        currentAttackersTxt.text = "Active Attackers: "+ ObjectPool.instance.ActiveAttacker.ToString();
        playerHPTxt.text = "Player Base HP: "+playerBaseHP.ToString();
        enemyHPTxt.text = "Enemy Base HP: "+enemyBaseHP.ToString();
    }

    void healthReport()
    {
        if (playerBaseHP <= 40)
        {
            playerKing.SetTrigger("die");
        }
        if (enemyBaseHP <= 40)
        {
            enemyKing.SetTrigger("die");
        }
        if (playerBaseHP <= 0)
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("Lose");
        }
        if (enemyBaseHP <= 0)
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene("Win");
        }
    }



}

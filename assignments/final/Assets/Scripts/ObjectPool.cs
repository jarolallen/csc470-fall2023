using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pool = new List<GameObject>();
    private int poolCount = 20;
    public int ActivePool;

    private List<GameObject> attackerPool = new List<GameObject>();
    private int attackerCount = 10;
    public int ActiveAttacker;

    private List<GameObject> enemyPool = new List<GameObject>();
    private int enemyCount = 10;
    public int ActiveEnemy;

    private List<GameObject> GolemPool = new List<GameObject>();
    private int GolemCount = 5;
    public int ActiveGolem;




    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private GameObject AttackerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject GolemPrefab;
    //singleton objectpool to make sure it's done

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = Instantiate(unitPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(unitPrefab);
            obj.SetActive(false);
            enemyPool.Add(obj);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ActivePoolCounter();
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0;i < pool.Count;i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    public GameObject GetEnemyPooledObject()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                return enemyPool[i];
            }
        }
        return null;
    }

    public void ActivePoolCounter()
    {
        int active = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy)
            {
                active += 1;
            }
        }
        ActivePool = active;
    }

    public void ActiveEnemyPoolCounter()
    {
        int active = 0;
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i].activeInHierarchy)
            {
                active += 1;
            }
        }
        ActiveEnemy = active;
    }
}

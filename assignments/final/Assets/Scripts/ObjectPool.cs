using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pool = new List<GameObject>();
    private int poolCount = 10;
    public int ActivePool;

    private List<GameObject> attackerPool = new List<GameObject>();
    private int attackerCount = 5;
    public int ActiveAttacker;

    public int friendlies;

    private List<GameObject> enemyPool = new List<GameObject>();
    private int enemyCount = 5;
    public int ActiveEnemy;

    private List<GameObject> GolemPool = new List<GameObject>();
    private int GolemCount = 5;
    public int ActiveGolem;

    public int enemies;



    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private GameObject AttackerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject GolemPrefab;

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

        for (int i = 0; i < attackerCount; i++)
        {
            GameObject obj = Instantiate(AttackerPrefab);
            obj.SetActive(false);
            attackerPool.Add(obj);
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(EnemyPrefab);
            obj.SetActive(false);
            enemyPool.Add(obj);
        }

        for (int i = 0; i < GolemCount; i++)
        {
            GameObject obj = Instantiate(GolemPrefab);
            obj.SetActive(false);
            GolemPool.Add(obj);
        }

        friendlies = ActivePool + ActiveAttacker;
        enemies = ActiveEnemy + ActiveGolem;

    }

    // Update is called once per frame
    void Update()
    {
        ActivePoolCounter();
        ActiveAttackerPoolCounter();
        ActiveEnemyPoolCounter();
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
    public GameObject GetAttackerPooledObject()
    {
        for (int i = 0; i < attackerPool.Count; i++)
        {
            if (!attackerPool[i].activeInHierarchy)
            {
                return attackerPool[i];
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

    public GameObject GetGolemPooledObject()
    {
        for (int i = 0; i < GolemPool.Count; i++)
        {
            if (!GolemPool[i].activeInHierarchy)
            {
                return GolemPool[i];
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
    public void ActiveAttackerPoolCounter()
    {
        int active = 0;
        for (int i = 0; i < attackerPool.Count; i++)
        {
            if (attackerPool[i].activeInHierarchy)
            {
                active += 1;
            }
        }
        ActiveAttacker = active;
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
    public void ActiveGolemPoolCounter()
    {
        int active = 0;
        for (int i = 0; i < GolemPool.Count; i++)
        {
            if (GolemPool[i].activeInHierarchy)
            {
                active += 1;
            }
        }
        ActiveGolem = active;
    }
}

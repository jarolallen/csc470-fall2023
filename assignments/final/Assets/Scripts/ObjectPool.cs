using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pool = new List<GameObject>();
    private int poolCount = 20;
    public int ActivePool;



    [SerializeField] private GameObject unitPrefab;
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
}

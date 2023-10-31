using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject cannonPrefab;
    public float numbPlaforms = 2000;
    public float numbCannons = 100;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numbPlaforms; i++)
        {
            generatePlatform();
        }
        for (int i = 0;i < numbCannons; i++)
        {
            generateCannon();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void generatePlatform()
    {
        float x = Random.Range(200, 800);
        float y = Random.Range(30, 800);
        float z = Random.Range(200, 800);
        Vector3 pos = new Vector3(x, y, z);
        GameObject floorObj = Instantiate(floorPrefab, pos, Quaternion.identity);
    }
    public void generateCannon()
    {
        float x = Random.Range(200, 800);
        float y = Random.Range(30, 800);
        float z = Random.Range(200, 800);
        Vector3 pos = new Vector3(x, y, z);
        GameObject cannonObj = Instantiate(cannonPrefab, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
    }
}

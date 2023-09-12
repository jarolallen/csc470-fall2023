using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Declaring public variables in a MonoBehaviour script means that you can
    // assign values to these variable in the Unity editor.
    public GameObject cactusPrefab;
    public GameObject rockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        generateCatusPins(5);
        generateThreeAreaRock(4);
        
    }

    void generateCatusPins(int pinrows)
    {
        // Generate a 4 rows (10 cacti) in a bowling pin layout
        int x = 0;
        int y = 0;
        int z = -25;
        int row = 0;
        for (int i = 0; i < pinrows; i++)
        {
            Vector3 pos = new Vector3(x, y, z);
            GameObject cactusObj = Instantiate(cactusPrefab, pos, Quaternion.identity);
            if (row > 0)
            {
                int fill = x;
                //fill is for the remaining pins in each row
                for (int j = 0; j < row; j++)
                {
                    fill += 10;
                    Vector3 fillpos = new Vector3(fill, y, z);
                    Instantiate(cactusPrefab, fillpos, Quaternion.identity);
                }
            }
            row++;
            x -= 5;
            z += 5;
        }
    }

    void generateThreeAreaRock(int number)
    {
        for (int i = 0;i < number;i++)
        {
            {
                generateLeftRock();
                generateRightRock();
                generateMidRock();
            }
        }
    }
    void generateLeftRock()
    {
        float x = Random.Range(-40, -30);
        float y = 0;
        float z = Random.Range(-40, 40);
        Vector3 pos = new Vector3(x, y, z);
        GameObject rockObj = Instantiate(rockPrefab, pos, Quaternion.identity);
    }
    void generateMidRock()
    {
        float x = Random.Range(-40, 40);
        float y = 0;
        float z = Random.Range(30, 40);
        Vector3 pos = new Vector3(x, y, z);
        GameObject rockObj = Instantiate(rockPrefab, pos, Quaternion.identity);
    }
    void generateRightRock()
    {
        float x = Random.Range(40, 30);
        float y = 0;
        float z = Random.Range(-40, 40);
        Vector3 pos = new Vector3(x, y, z);
        GameObject rockObj = Instantiate(rockPrefab, pos, Quaternion.identity);
    }


        // Update is called once per frame
        void Update()
    {

    }
}

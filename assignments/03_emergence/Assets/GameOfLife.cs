using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;

    // Create a 2D array of CellScripts
    public CellScript[,] cells;

    //made grid size a variable
    public int xAxis = 20;
    public int yAxis = 20;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the 2D array that we will use to store the state of the
        // Cells.
        cells = new CellScript[xAxis, yAxis];

        // Using nested for loops is a good way to create patterns like (0,0),
        // (0,1), (0,2), (0,3)... (2,0), (2,1), (2,2).... (5,0), (5,1)... etc.
        for (int x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++)
            {
                // Create a position based on x, y
                Vector3 pos = transform.position;
                float cellWidth = 1f;
                float spacing = 0.1f;
                pos.x = pos.x + x * (cellWidth + spacing);
                pos.z = pos.z + y * (cellWidth + spacing);
                GameObject cellObj = Instantiate(cellPrefab, pos, transform.rotation);
                // (x,y) is the index in the 2D array. Store a reference to the
                // CellScript of the instantiated object because that is the
                // object that contains the information we will be intereated in
                // (the 'alive' variable.
                cells[x, y] = cellObj.GetComponent<CellScript>();
                cells[x, y].x = x;
                cells[x, y].y = y;
                var random = new System.Random();
                cells[x, y].alive = (random.Next(2) == 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}

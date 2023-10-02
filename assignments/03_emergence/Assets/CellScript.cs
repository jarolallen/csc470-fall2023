using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public bool alive = false;
    GameOfLife gol;

    public int x = -1;
    public int y = -1;

    public Color aliveColor;
    public Color deadColor;


    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        GameObject golObj = GameObject.Find("GameOfLifeObj");
        gol = golObj.GetComponent<GameOfLife>();

        rend = gameObject.GetComponentInChildren<Renderer>();
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextGen();
        }
    }

    public void UpdateColor()
    {
        if (alive == true)
        {
            rend.material.color = aliveColor;
        } 
        else if (alive == false)
        {
            rend.material.color = deadColor;
        }
    }

    private void OnMouseDown()
    {
        alive = !alive;
        UpdateColor();

        Debug.Log(CountLiveNeighbors());
    }

    int CountLiveNeighbors()
    {
        int alive = 0;

        for (int xIndex = x - 1; xIndex <= x + 1; xIndex++)
        {
            for (int yIndex = y - 1; yIndex <= y + 1; yIndex++)
            {
             try
                {
                    if (gol.cells[xIndex, yIndex].alive)
                    {
                        alive++;
                    }
                }
            catch (System.IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: {0}");
                }
            }
        }

        return alive;
    }
    public void NextGen()
       
    {
        //Lonely death
        int aliveNeighbors = CountLiveNeighbors();
        if (alive == true && aliveNeighbors < 3)
        {
            alive = false;
        }
        //over population
        else if (alive == true && aliveNeighbors > 4) 
        {
            alive = false;
        }
        //birth
        else if (alive == false && aliveNeighbors == 3) 
        {
            alive = true;
        }
        UpdateColor();
    }

}

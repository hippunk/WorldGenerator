using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell  {

    public bool cellType = false; //<- has to be changed for complex generation behaviour
    public List<Cell> neighbours = new List<Cell>();
    public Vector2 coordinates;
    public bool hasToChange;
    public int aliveCount = 0;

    public Cell(Vector2 coordinates)
    {
        this.coordinates = coordinates;
    }

    public void InitValues()
    {
        int count = neighbours.Count;
        for (int i = 0; i < count; i++)
        {
            if (neighbours[i].cellType == true)
                aliveCount++;
        }
    }

    public void CheckChange(int value)
    {
        //Raw rules, has to be generalized for multiple cell types, complex rules and easy customisation
        if (!cellType)
        {
            if (aliveCount > value)
            {
                hasToChange = true;
            }
        }
        else if (cellType)
        {
            if (aliveCount < value)
            {
                hasToChange = true;
            }
        }
    }

    public void NotifyChange(bool state)
    {
        int count = neighbours.Count;
        for (int i = 0; i < count; i++)
        {
            neighbours[i].aliveCount += state ? 1 : -1;
        }
    }

    /*public void Change()
    {
        if (hasToChange)
        {
            hasToChange = false;
            cellType = !cellType;
            //Color newColor = cellType ? betaCellColor : alphaCellColor;
            //GetComponent<SpriteRenderer>().material.color = newColor;
            NotifyChange(cellType);
        }
    }*/
}

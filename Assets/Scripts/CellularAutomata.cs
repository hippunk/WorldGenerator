using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour {


    public int width = 25;
    public int height = 25;
    public float rateCell = 0.5f;
    public int zLayer = 0;
    public int neigbourhoodDepth = 1;

    public GameObject cellPrefab;

    private List<Cell> worldCells = new List<Cell>();


	// Use this for initialization
	void Start () {
        //Create and initialize Cells
        for (int i = 0; i < width* height; i++)
        {
            GameObject newCell = Instantiate(cellPrefab);

            int cellXcoord = i % width;
            int cellYcoord = i / height;
            newCell.transform.position = new Vector3(cellXcoord * 0.25f, cellYcoord * 0.25f,zLayer);

            Cell newCellComponent = newCell.GetComponent<Cell>();
            worldCells.Add(newCellComponent);
            newCellComponent.coordinates = new Vector2(cellXcoord,cellYcoord);


            //Randomize cell type (should be handled by the cell itself ?)
            float cellType = Random.value;
            Debug.Log(cellType);
            if (cellType > rateCell)
            {
                newCellComponent.cellType = true;
            }
        }

        //Build neighbours map
        foreach(Cell cell in worldCells)
        {
            string str = "Debug Cell coord : x " + cell.coordinates.x + " y " + cell.coordinates.y;
            for(int i = -neigbourhoodDepth; i <= neigbourhoodDepth; i++)//doit permettre de gérer la profondeur sur des formes quadrillages
            {
                for (int j = -neigbourhoodDepth; j<= neigbourhoodDepth; j++)
                {
                    if(i != 0 || j != 0) //si pas moi même
                    {
                        int xWrapped = (width + (int)(cell.coordinates.x) + i) % width;
                        int yWrapped = (height + (int)(cell.coordinates.y) + j) % height;
                        int neighbourId = yWrapped * width + xWrapped;
                        str += "\n neighbour at i " + i + ",j " + j + "as coordinaites : x " + xWrapped + ",y " + yWrapped + " and index : " + neighbourId;
                        Cell neighbourCell = worldCells[neighbourId];
                        cell.neighbours.Add(neighbourCell);
                    }
                }
            }
            //Debug neighbourhood coordinates wrapping computation
            //Debug.Log(str);
        }


        /*
        for (int i = 0; i < width ; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //CreateCell
                GameObject newCell = Instantiate(cellPrefab);
                newCell.transform.position = new Vector3(i * 0.25f, j * 0.25f, zLayer);

                //Randomize cell
                float cellType = Random.Range(0f, 1f);
                if (cellType > rateCell)
                {
                    newCell.GetComponent<SpriteRenderer>().material.color = Color.black;
                }
            }
        }
        */
    }

    // Update is called once per frame
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour {


    public int width = 25;
    public int height = 25;
    public float perinThreshold = 0.5f;
    public float gaussianThreshold = 0.5f;
    public int modifier = 4;
    public int zLayer = 0;
    public int neigbourhoodDepth = 1;
    public float scale = 1.0f;
    public float refreshEvery = 5f;

    private float time;

    public GameObject cellPrefab;

    public Color alphaCellColor;
    public Color betaCellColor;

    private List<Cell> worldCells = new List<Cell>();
    private Texture2D map;

	// Use this for initialization
	void Start () {
        map = new Texture2D(width, height);
        map.filterMode = FilterMode.Point;
        map.wrapMode = TextureWrapMode.Clamp;
        map.mipMapBias = 0;

        Sprite generatedSprite = Sprite.Create(map, new Rect(0, 0, width, height), Vector2.zero);
        GetComponent<SpriteRenderer>().sprite = generatedSprite;

        //Create and initialize Cells
        for (int i = 0; i < width* height; i++)
        {
            //GameObject newCell = Instantiate(cellPrefab);

            int cellXcoord = i % width;
            int cellYcoord = i / height;


            Cell newCell = new Cell(new Vector2(cellXcoord, cellYcoord));
            worldCells.Add(newCell);


            //Randomize cell type (should be handled by the cell itself ?)
            float cellXRat = (float)cellXcoord / width * scale;
            float cellYRat = (float)cellYcoord / height * scale;
            //Debug.Log(cellXRat + " " + cellYRat);
            float cellType = Mathf.PerlinNoise(cellXRat, cellYRat);// Random.value;

            //Debug.Log(cellType);
            /*if (cellType > perinThreshold)
            {
                newCell.cellType = true;
                map.SetPixel(cellXcoord,cellYcoord,betaCellColor);
            }
            else
            {
                map.SetPixel(cellXcoord, cellYcoord, alphaCellColor);
            }*/

            cellType = Random.value;// Random.value;

            //Debug.Log(cellType);
            if (cellType > gaussianThreshold)
            {
                newCell.cellType = true;
                map.SetPixel(cellXcoord, cellYcoord, betaCellColor);
            }
            else
            {
                map.SetPixel(cellXcoord, cellYcoord, alphaCellColor);
            }
        }
        map.Apply();
        //Build neighbours map
        foreach(Cell cell in worldCells)
        {
            //string str = "Debug Cell coord : x " + cell.coordinates.x + " y " + cell.coordinates.y;
            for(int i = -neigbourhoodDepth; i <= neigbourhoodDepth; i++)//doit permettre de gérer la profondeur sur des formes quadrillages
            {
                for (int j = -neigbourhoodDepth; j<= neigbourhoodDepth; j++)
                {
                    if(i != 0 || j != 0) //si pas moi même
                    {
                        int xWrapped = (width + (int)(cell.coordinates.x) + i) % width;
                        int yWrapped = (height + (int)(cell.coordinates.y) + j) % height;
                        int neighbourId = yWrapped * width + xWrapped;
                        //str += "\n neighbour at i " + i + ",j " + j + "as coordinaites : x " + xWrapped + ",y " + yWrapped + " and index : " + neighbourId;
                        Cell neighbourCell = worldCells[neighbourId];
                        cell.neighbours.Add(neighbourCell);
                    }
                }
            }
            //Debug neighbourhood coordinates wrapping computation
            //Debug.Log(str);

        }
        foreach (Cell cell in worldCells)
        {
            cell.InitValues();
        }
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        if (time > refreshEvery)
        {
            time = 0;
            int worldCellCount = worldCells.Count;

            for (int i = 0; i < worldCellCount;i++)
            {
                worldCells[i].CheckChange(modifier);
            }

            for (int i = 0; i < worldCellCount; i++)
            {
                Cell currentCell = worldCells[i];
                if (currentCell.hasToChange)
                {
                    currentCell.hasToChange = false;
                    currentCell.cellType = !currentCell.cellType;
                    Color newColor = currentCell.cellType ? betaCellColor : alphaCellColor;
                    map.SetPixel((int)currentCell.coordinates.x, (int)currentCell.coordinates.y, newColor);


                    currentCell.NotifyChange(currentCell.cellType);
                }
            }

            map.Apply();
        }
    }
}

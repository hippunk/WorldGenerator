using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour {

    public bool cellType = false; //<- has to be changed for complex generation behaviour
    public List<Cell> neighbours;
    public Color alphaCellColor;
    public Color betaCellColor;
    public Vector2 coordinates;

    //Debug neighbours computations
    public void OnMouseDown()
    {
        Debug.Log("Click");
        Color dummyColor = new Color(Random.value, Random.value, Random.value);
        foreach (Cell cell in neighbours)
        {
            cell.gameObject.GetComponent<SpriteRenderer>().material.color = dummyColor;
        }
    }

    // Use this for initialization
    void Start () {

        Color birthColor = cellType?betaCellColor:alphaCellColor;

        GetComponent<SpriteRenderer>().material.color = birthColor;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

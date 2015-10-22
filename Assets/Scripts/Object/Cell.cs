using UnityEngine;

[System.Serializable]
public class Cell
{
    public int CellType;            // type cell with 4 : red; 3 : blue; 2 : gray; 1 : transparent.

    public Vector2 CellPosition;    // position vector 2 of cell

    public int CellEffect;          // 4 : link, 5 : ice

}

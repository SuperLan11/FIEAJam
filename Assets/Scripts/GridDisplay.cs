using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    [TextArea(3, 10)]
    public string shape;
    //. = empty
    //X = full

    public List<List<bool>> grid = new();

    public List<GameObject> tiles;
    public GameObject tilePrefab;
    //true means monsters can be dragged here
    public bool isTargetable = false;
    void Start()
    {
        var rows = shape.Trim().Split();
        int height = rows.Length;
        int width = rows[0].Length;
        grid = new();
        for (int i = 0; i < height; i++)
        {
            List<bool> row = new();
            for (int j = 0; j < width; j++)
            {
                bool filled = rows[i][j] == 'X';
                row.Add(filled);
                if (filled)
                {
                    float x = j + 0.5f;
                    float y = (height - i - 1) + 0.5f;
                    GameObject tile = Instantiate(tilePrefab, transform);
                    tile.transform.localPosition = new Vector2(x, y);
                    tiles.Add(tile);
                }
            }
        }
    }
    
    //given the bottom-right corner of a thing, get the place where
    //it would snap to on the grid, or return null if it's
    //out of bounds
    public Vector2 GetSnapPosition(Vector2 position, out bool found)
    {
        foreach (GameObject tile in tiles)
        {
            Vector2 tilePos = tile.transform.position;
            if (Mathf.Abs(tilePos.x - position.x) <= 0.5f && Mathf.Abs(tilePos.y - position.y) <= 0.5f)
            {
                found = true;
                return tilePos;
            }
        }

        found = false;
        return new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

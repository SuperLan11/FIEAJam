using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    [TextArea(3, 10)]
    public string shape;
    //. = empty
    //X = full

    public static int numTiles = 5;

    public List<List<bool>> grid = new();
    public List<List<bool>> free = new();

    public Dictionary<(int, int), GameObject> tiles = new();
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
                    tiles.Add((i, j), tile);
                }
                else
                {
                    tiles.Add((i, j), null);
                }
            }
            grid.Add(row);
        }
        
        grid.ForEach((item) =>
        {
            free.Add(new List<bool>(item));
        });
    }
    
    //given the bottom-right corner of a thing, get the place where
    //it would snap to on the grid, or return null if it's
    //out of bounds or unable to be placed
    //has side effects if found a position, changing the occupied array
    public Vector2 AttemptSnap(Vector2 position, List<List<bool>> shape, out bool found)
    {
        foreach (var record in tiles)
        {
            var tile = record.Value;
            if (tile ==null)
            {
                continue;
            }
            Vector2 tilePos = tile.transform.position;
            if (Mathf.Abs(tilePos.x - position.x) <= 0.5f && Mathf.Abs(tilePos.y - position.y) <= 0.5f)
            {
                (int, int) pos = record.Key;
                int tileI = pos.Item1;
                int tileJ = pos.Item2;
                
                found = true;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[0].Count; j++)
                    {
                        if (!shape[i][j])
                        {
                            continue;
                        }
                        int newI = tileI - shape.Count + 1 + i;
                        int newJ = j + tileJ;

                        if (newI < 0)
                        {
                            found = false;
                            break;
                        }

                        if (newJ >= grid[0].Count)
                        {
                            found = false;
                            break;
                        }

                        if (!free[newI][newJ])
                        {
                            found = false;
                        }
                    }
                }

                if (found)
                {
                    for (int i = 0; i < shape.Count; i++)
                    {
                        for (int j = 0; j < shape[0].Count; j++)
                        {
                            free[-shape.Count + 1 + i + tileI][j+tileJ] = false;
                        }
                    }
                }
                
                return tilePos;
            }
        }

        found = false;
        return new Vector2(0, 0);
    }

    // hard-coded upgrades for now
    public void UpgradeSize(int numTiles)
    {
        if(numTiles == 6)
        {
            Debug.Log("changed shape");
            shape = "XXX\nXXX\nXXX";
        }
        else if(numTiles == 9)
        {
            Debug.Log("changed shape");
            shape = "XXXX\nXXXX\nXXXX";
        }
        else if(numTiles == 12)
        {
            Debug.Log("changed shape");
            shape = "XXXX\nXXXX\nXXXX\nXXXX";
        }
        grid = new();
        free = new();
        tiles = new();
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

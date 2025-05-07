using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using TMPro;

public class GridDisplay : MonoBehaviour
{
    [TextArea(3, 10)]
    public string shape;
    //. = empty
    //X = full

    public List<SpriteRenderer> cartSprites = new List<SpriteRenderer>();
    [SerializeField] private Sprite cart3xSprite;
    [SerializeField] private Sprite cart4xSprite;

    [SerializeField] private SpriteRenderer cartFront;
    [SerializeField] private Sprite cart3xFront;
    [SerializeField] private Sprite cart4xFront;

    public int visibleCarts = 4;
    public int curCartHeight = 2;
    private float endX = 12f;
    private float returnX = -16f;
    private float cartAccel = 0.15f;
    public Vector2 returnPos = Vector2.zero;
    private Vector2 startPos = Vector2.zero;

    public List<List<bool>> grid = new();
    //0 = nothing
    //other = monster ID
    public List<List<int>> free = new();

    public Dictionary<(int, int), GameObject> tiles = new();
    public GameObject tilePrefab;
    //true means monsters can be dragged here
    public bool isTargetable = false;
    void Start()
    {
        if (startPos == Vector2.zero) startPos = transform.position;
        if (returnPos == Vector2.zero) returnPos = new Vector2(returnX, transform.position.y);

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

        ResetFree();
        passengers = new();

        List<SpriteRenderer> rawSprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        for (int i = 0; i < rawSprites.Count; i++)
        {
            // excludes the white square sprites
            if (rawSprites[i].name.Contains("Cart"))
                cartSprites.Add(rawSprites[i]);
        }
    }

    public void AppendCart()
    {
        if (visibleCarts < cartSprites.Count)
        {
            cartSprites[visibleCarts].enabled = true;
            visibleCarts++;
        }
        Vector2 newFrontPos = cartFront.transform.position;
        newFrontPos.x += 1f;
        cartFront.transform.position = newFrontPos;

        string curShape = shape;
        string newShape = "";

        for (int i = 0; i < curShape.Length; i++)
        {
            // add an extra X just before new line
            if (curShape[i] == '\n' || i == curShape.Length - 1)
                newShape += 'X';
            newShape += curShape[i];
        }
        shape = newShape;
        ResetShape();

        //FixAlignment();
    }

    public void UpgradeHeight()
    {
        if (curCartHeight >= 4)
            return;

        string newShape = shape;
        newShape += '\n';

        curCartHeight++;
        for (int i = 0; i < cartSprites.Count; i++)
        {
            newShape += 'X';
            if (curCartHeight == 3)
            {
                cartSprites[i].sprite = cart3xSprite;
                cartFront.sprite = cart3xFront;
            }
            else if (curCartHeight == 4)
            {
                cartSprites[i].sprite = cart4xSprite;
                cartFront.sprite = cart4xFront;
                GameObject.Find("Enlarge Cart").GetComponent<TextMeshProUGUI>().text = "MAX";
                GameObject.Find("Enlarge Cart Price").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        //Debug.Log("squareCount: " + squareCount);
        /*Vector2 newGroupPos = transform.position;
        newGroupPos.y -= 5f;
        transform.position = newGroupPos;

        Vector2 newFrontPos = cartFront.transform.position;
        newFrontPos.y += 0.5f;
        cartFront.transform.position = newFrontPos;*/

        shape = newShape;
        ResetShape();
        
        FixAlignment();
    }

    private void FixAlignment()
    {
        // do this after reset shape so position changes aren't overriden
        List<SpriteRenderer> sprites = FindObjectsOfType<SpriteRenderer>().ToList();
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite.name.Contains("Cart") || sprite.name.Contains("front"))
            {
                if (curCartHeight == 3)
                {
                    Vector2 newSpritePos = sprite.transform.position;
                    newSpritePos.y += 0.5f;
                    sprite.transform.position = newSpritePos;
                    transform.position += Vector3.down * 1f;
                }
                else if (curCartHeight == 4)
                {
                    Vector2 newSpritePos = sprite.transform.position;
                    newSpritePos.y += 0.5f;
                    sprite.transform.position = newSpritePos;
                    transform.position += Vector3.down * 1.2f;
                }
            }
        }
    }

    private List<int> gridrowToFreerow(List<bool> gridrow)
    {
        List<int> freerow = new();
        gridrow.ForEach((element) => freerow.Add(element ? 0 : -1));
        return freerow;
    }

    private void ResetFree()
    {
        free = new();
        grid.ForEach((row) =>
        {
            free.Add(gridrowToFreerow(row));
        });
    }

    public int GetProfit(int filled, int total, int bodies)
    {
        int profit = 0;
        for (int i = 0; i < bodies; i++)
        {
            profit += UnityEngine.Random.Range(6, 9);
        }
        if (filled == total)
        {
            profit += 10;

        }
        return profit;
    }

    private List<Monster> passengers;
    public bool canSend = true;

    public void Send(ThreadStart upgradeCallback)
    {
        if (!canSend)
        {
            return;
        }
        canSend = false;
        int filled = 0;
        int total = 0;
        foreach (var row in free)
        {
            foreach (int elem in row)
            {
                total++;
                if (elem != 0)
                {
                    filled++;
                }
            }
        }
        StartCoroutine(MoveCart(upgradeCallback));

        int profit = GetProfit(filled, total, passengers.Count);
        MoneyCounter moneyCnt = FindObjectOfType<MoneyCounter>();
        MoneyCounter.profit += profit;
        int startMoney = MoneyCounter.money;
        MoneyCounter.money += profit;
        int endMoney = MoneyCounter.money;
        StartCoroutine(moneyCnt.MoneyRoll(0.03f, startMoney, endMoney));
        // don't increment money immediately as it is set in MoneyRoll
        //MoneyCounter.money += profit;
        ResetFree();
    }

    private IEnumerator MoveCart(ThreadStart upgradeCallback)
    {
        while (transform.position.x < endX)
        {
            yield return new WaitForFixedUpdate();
            Vector2 endPos = transform.position;
            endPos.x = endX + 0.1f;
            transform.position = Vector2.Lerp(transform.position, endPos, cartAccel);
        }
        transform.position = returnPos;
        bool hasAnyone = passengers.Count != 0;
        passengers.ForEach(monster => Destroy(monster.gameObject));
        passengers.Clear();
        
        upgradeCallback.Invoke();
        // teleport then come back
        var panel = FindObjectOfType<DayPanel>();
        if (panel.endOfDay && hasAnyone)
        {
            panel.EndDay();
        }

        while (Mathf.Abs(transform.position.x - startPos.x) > 0.08f)
        {
            yield return new WaitForFixedUpdate();
            transform.position = Vector2.Lerp(transform.position, startPos, cartAccel);
        }
        canSend = true;
    }

    private void ClearMonster(Monster monster)
    {
        foreach (var row in free)
        {
            for (var i = 0; i < row.Count; i++)
            {
                if (row[i] == monster.id)
                {
                    row[i] = 0;
                }
            }
        }
    }

    //given the bottom-right corner of a monster, get the place where
    //it would snap to on the grid, or return null if it's
    //out of bounds or unable to be placed
    //if found, registers the monster as on the ride
    public Vector2 AttemptRide(Monster monster, out bool found)
    {
        Vector2 position = monster.GetBottomLeftCorner();
        var shape = monster.grid;
        foreach (var record in tiles)
        {
            var tile = record.Value;
            if (tile == null)
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

                        if (free[newI][newJ] != 0 && free[newI][newJ] != monster.id)
                        {
                            found = false;
                        }
                    }
                }

                if (found)
                {
                    ClearMonster(monster);
                    if (!passengers.Contains(monster))
                    {
                        passengers.Add(monster);
                    }
                    for (int i = 0; i < shape.Count; i++)
                    {
                        for (int j = 0; j < shape[0].Count; j++)
                        {
                            if (!shape[i][j])
                            {
                                continue;
                            }
                            free[-shape.Count + 1 + i + tileI][j + tileJ] = monster.id;
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
        if (numTiles == 12)
        {
            shape = "XXXX\nXXXX\nXXXX";
        }
        else if (numTiles == 16)
        {
            shape = "XXXX\nXXXX\nXXXX\nXXXX";
        }
        else if (numTiles == 25)
        {
            shape = "XXXXX\nXXXXX\nXXXXX\nXXXXX";
        }
        ResetShape();
    }

    public void ResetShape()
    {
        grid = new();
        free = new();
        foreach (var record in tiles)
        {
            Destroy(record.Value.gameObject);
        }
        tiles = new();
        Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
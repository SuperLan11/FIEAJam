using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	public bool isDragging;
	public Vector2 dragDelta;
	private Vector2 originalPos;

	[TextArea(3, 10)]
	public string shape;
	public List<List<bool>> grid;

	private static int idCounter = 0;
	public int id = 0;

	public void Start()
	{
		id = ++idCounter;
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
			}
			grid.Add(row);
		}
	}

	// variables added to move monsters in line
	private bool isPlaced = false;
	public bool isMoving = false;
	Vector3 movePos = Vector3.zero;
	
	private static float moveAccel = 0.1f;
	private static float monsterSpacing = 2f;

	private AudioSource snapSfx;
	
	private Vector2 GetMousePosition() => MainCamera.instance.camera.ScreenToWorldPoint(Input.mousePosition);
	public void StartDrag()
	{
		isDragging = true;
		originalPos = transform.position;
		Vector2 mousePos = GetMousePosition();
		Vector2 delta = mousePos - (Vector2)transform.position;
		dragDelta = delta;
		GetComponentInChildren<SpriteRenderer>().sortingLayerName = "monsterDragging";
		snapSfx = GetComponent<AudioSource>();
	}

	public void EndDrag()
	{
		isDragging = false;
		GetComponentInChildren<SpriteRenderer>().sortingLayerName = "monster";
		foreach (GridDisplay display in FindObjectsOfType<GridDisplay>())
		{
			if (!display.isTargetable)
			{
				continue;
			}

			Vector2 pos = display.AttemptRide(this, out bool found);
			if (found)
			{
				transform.position = pos;
				isPlaced = true;
				
				if(snapSfx != null)
					snapSfx.Play();

				AdvanceLine();				
				return;
			}
		}

		transform.position = originalPos;
	}

	private void AdvanceLine()
    {		
		Monster[] monsters = FindObjectsOfType<Monster>();
		foreach (Monster monster in monsters)
		{
			if (monster.isPlaced)
				continue;
			
			monster.isMoving = true;
			monster.movePos = monster.transform.position;
			monster.movePos.x += monsterSpacing;
		}
	}


	public void Update()
	{
		if (isDragging)
		{
			transform.position = GetMousePosition() - dragDelta;
		}
        else if (isMoving)
        {            
            transform.position = Vector3.Lerp(transform.position, movePos, moveAccel);

            if (Vector3.Distance(transform.position, movePos) < 0.1f)
                isMoving = false;
        }
    }
}
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

	public void Start()
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

		snapSfx = GetComponent<AudioSource>();
	}

	public void EndDrag()
	{
		isDragging = false;
		foreach (GridDisplay display in FindObjectsOfType<GridDisplay>())
		{
			if (!display.isTargetable)
			{
				continue;
			}

			Vector2 pos = display.AttemptSnap(transform.position, grid, out bool found);
			if (found)
			{
				transform.position = pos;

				// only advance line if the selected piece hasn't been placed
				if (!isPlaced)
				{
					isPlaced = true;
					AdvanceLine();
				}				
				
				if(snapSfx != null)
					snapSfx.Play();
					
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
			Debug.Log("originalPos.x: " + originalPos.x);
			bool monsterOnRight = (!monster.isPlaced && monster.transform.position.x > originalPos.x);
			if (monster.isPlaced || monsterOnRight)
			{
				Debug.Log(monster.name + " should not move");
				continue;
			}
			
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
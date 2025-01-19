using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	private SpriteRenderer sprite;

	private void rotate90()
	{
		List<List<bool>> newGrid = new();

		for (int i = 0; i < grid[0].Count; i++)
		{
			List<bool> row = new();
			for (int j = 0; j < grid.Count; j++)
			{
				row.Add(false);
			}
			newGrid.Add(row);
		}
		
		for (var i = 0; i < grid.Count; i++)
		{
			for (int j = 0; j < grid[0].Count; j++)
			{
				newGrid[j][i] = grid[i][j];
			}
		}

		foreach (var row in newGrid)
		{
			row.Reverse();
		}

		grid = newGrid;
	}

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

		sprite = GetComponentInChildren<SpriteRenderer>();
	}
	
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
				transform.position = pos - GetBottomLeftOffset();
				transform.SetParent(FindObjectOfType<GridDisplay>().transform);				
				
				if (snapSfx != null)
					snapSfx.Play();

				Line.instance.RemoveFromLine(this);
				movementQueue.Clear();
				return;
			}
		}

		transform.position = originalPos;
	}
	
	public void Update()
	{
		if (isDragging)
		{
			transform.position = GetMousePosition() - dragDelta;
			if (Input.GetKeyDown(KeyCode.E) || (Input.GetAxis("Mouse ScrollWheel") > 0f))
			{
				Vector2 mousePos = GetMousePosition();
				transform.RotateAround(mousePos, Vector3.back, 90);
				dragDelta = mousePos - (Vector2)transform.position;
				if (MainCamera.instance.TurnNoise != null) MainCamera.instance.TurnNoise.Play();
				rotate90();

			} else if (Input.GetKeyDown(KeyCode.Q) || (Input.GetAxis("Mouse ScrollWheel") < 0f))
			{
				Vector2 mousePos = GetMousePosition();
				transform.RotateAround(mousePos, Vector3.back, -90);
				dragDelta = mousePos - (Vector2)transform.position;
				if (MainCamera.instance.TurnNoise != null) MainCamera.instance.TurnNoise.Play();
				rotate90();
				rotate90();
				rotate90();
			}
		}
        else if (movementQueue.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementQueue.Peek(), Time.deltaTime*4);

            if (Vector3.Distance(transform.position, movementQueue.Peek()) < 0.01f)
	            movementQueue.Dequeue();
        }
    }

	public Queue<Vector3> movementQueue = new();

	public void QueueMovement(Vector3 pos)
	{
		movementQueue.Enqueue(pos);
	}

	public void UnGray()
	{
		GetComponentInChildren<SpriteRenderer>().color = Color.white;
	}

	private Vector2 GetBottomLeftOffset()
	{
		return Vector2.left * (grid[0].Count-1) * 0.5f + Vector2.down * (grid.Count-1) * 0.5f;
	}

	public Vector2 GetBottomLeftCorner()
	{
		return (Vector2)transform.position + GetBottomLeftOffset();
	}
}
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
	
	private Vector2 GetMousePosition() => MainCamera.instance.camera.ScreenToWorldPoint(Input.mousePosition);
	public void StartDrag()
	{
		isDragging = true;
		originalPos = transform.position;
		Vector2 mousePos = GetMousePosition();
		Vector2 delta = mousePos - (Vector2)transform.position;
		dragDelta = delta;
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
				return;
			}
		}

		transform.position = originalPos;

	}

	public void Update()
	{
		if (isDragging)
		{
			transform.position = GetMousePosition() + dragDelta;
		}
	}
}
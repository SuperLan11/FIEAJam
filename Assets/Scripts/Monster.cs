using UnityEngine;

public class Monster : MonoBehaviour
{
	public bool isDragging;
	public Vector2 dragDelta;
	private Vector2 originalPos;
	
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

			Vector2 pos = display.GetSnapPosition(transform.position, out bool found);
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
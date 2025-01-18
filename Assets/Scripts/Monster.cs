using UnityEngine;

public class Monster : MonoBehaviour
{
	public bool isDragging;
	public Vector2 dragDelta;
	private Vector2 originalPos;

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

			Vector2 pos = display.GetSnapPosition(transform.position, out bool found);
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
			transform.position = GetMousePosition() + dragDelta;
		}
        else if (isMoving)
        {            
            transform.position = Vector3.Lerp(transform.position, movePos, moveAccel);

            if (Vector3.Distance(transform.position, movePos) < 0.1f)
                isMoving = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public bool dragInProgress = false;
    private Monster draggedMonster;
    void Start()
    {
        dragInProgress = false;
    }

    void Update()
    {
        if (dragInProgress)
        {
            if (Input.GetMouseButtonUp(0))
            {                
                draggedMonster.EndDrag();
                dragInProgress = false;
            }
        }
        else
        {
            
            if (Input.GetMouseButtonDown(0))
            {                
                Vector2 pos = MainCamera.instance.camera.ScreenToWorldPoint(Input.mousePosition);
                var colliders = Physics2D.OverlapPointAll(pos);
                foreach (var collider in colliders)
                {
                    var monster = collider.gameObject.GetComponent<Monster>();
                    if (monster != null && Line.instance.isInBoardingZone(monster))
                    {
                        monster.StartDrag();
                        draggedMonster = monster;
                        dragInProgress = true;
                        break;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public class Pickaxe : Ability
{
    public override int GetRange()
    {
        return 5;
    }

    public override void Use()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mouseLocation.x);
            int y = Mathf.RoundToInt(mouseLocation.y);
            int playerX = Mathf.RoundToInt(transform.position.x);
            int playerY = Mathf.RoundToInt(transform.position.y);
            bool notPlayer = (x != playerX || (y != playerY && y != playerY + 1));

            GameObject hit = Get(x, y);
            if (hit)
            {
                inventory.AddItem(PickUp(x, y));
            }
        }
    }
}

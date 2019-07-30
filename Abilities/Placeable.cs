using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public class Placeable : Ability
{
    protected override void Init()
    {
        name = "Placeable";
        range = 1;
        cooldown = 1;
        description = "This object can be placed in the world";
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
            if (inventory.GetSelected() != null && notPlayer)
            {
                if (Empty(x, y)) Place(x, y, inventory.RemoveItem());
            }
        }
    }
}

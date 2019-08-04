using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public class Pickaxe : Ability
{
    protected override void Init()
    {
        name = "Pickaxe";
        range = 1;
        cooldown = 1;
        description = "Destroy and pickup and block within range";
    }

    public void Use2()
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

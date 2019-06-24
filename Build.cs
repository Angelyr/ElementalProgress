using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour
{
    public WorldGen world;
    public Inventory inventory;
    private bool grabing = false;
    private bool placing = false;

    private void FixedUpdate()
    {
        IfClick();
    }

    private void IfClick()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mouseLocation.x);
            int y = Mathf.RoundToInt(mouseLocation.y);
            int playerX = Mathf.RoundToInt(transform.position.x);
            int playerY = Mathf.RoundToInt(transform.position.y);
            bool notPlayer = (x != playerX || (y != playerY && y != playerY + 1));

            GameObject hit = world.Get(x, y);
            if (hit && !placing) grabing = true;
            else if (inventory.GetItem() && notPlayer && !grabing) placing = true;

            if (hit && grabing)
            {
                inventory.AddItem(world.PickUp(x, y));
            }
            else if (inventory.GetItem() && notPlayer && placing)
            {
                GameObject tile = inventory.GetItem();
                if (world.Place(x, y, tile)) inventory.RemoveItem();
            }
        }
        else
        {
            grabing = false;
            placing = false;
        }
        
    }
}

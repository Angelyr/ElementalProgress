using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public class Usable : MonoBehaviour
{
    private Inventory inventory;
    private GameObject player;
    private GameObject laser;

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        laser = (GameObject)Resources.Load("Prefab/Laser", typeof(GameObject));
        player = GameObject.Find("Player");
    }

    private void Pickaxe()
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

    private void Block()
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
            if (inventory.ItemSelected() && notPlayer)
            {
                if (Empty(x, y)) Place(x, y, inventory.RemoveItem());
            }
        }
    }

    private void Laser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject attack = Instantiate(laser);
            attack.SetActive(true);
            attack.transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
            Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseLocation.x = mouseLocation.x - attack.transform.position.x;
            mouseLocation.y = mouseLocation.y - attack.transform.position.y;
            float angle = Mathf.Atan2(mouseLocation.y, mouseLocation.x) * Mathf.Rad2Deg;
            attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            attack.GetComponent<Effects>().Animation();
        }
    }

    public void Use()
    {
        if (gameObject.name.Contains("Pickaxe")) Pickaxe();
        if (gameObject.name.Contains("Block")) Block();
        if (gameObject.name.Contains("Laser")) Laser();
    }
}

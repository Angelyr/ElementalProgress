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

    private int prevMouseX;
    private int prevMouseY;

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        laser = (GameObject)Resources.Load("Prefab/Laser", typeof(GameObject));
        player = GameObject.Find("Player");
    }

    public void Use()
    {
        if (gameObject.name.Contains("Pickaxe")) Pickaxe();
        if (gameObject.name.Contains("Block")) Block();
        if (gameObject.name.Contains("Laser")) Laser();
        if (gameObject.name.Contains("Punch")) Punch();
    }

    public int GetRange()
    {
        if (gameObject.name.Contains("Pickaxe")) return 5;
        if (gameObject.name.Contains("Block")) return 5;
        if (gameObject.name.Contains("Laser")) return 5;
        if (gameObject.name.Contains("Punch")) return 1;
        return 0;
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

    private bool WithInRange(int range, int targetX, int targetY)
    {
        if (Mathf.Abs(targetX - transform.position.x) <= range) return true;
        if (Mathf.Abs(targetY - transform.position.y) <= range) return true;
        return false;
    }

    private void Punch()
    {
        int range = 1;
        int targetX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        int targetY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (!Input.GetMouseButtonDown(0)) return;
        if (!WithInRange(range, targetX, targetY)) return;
        GameObject target = Get(targetX, targetY);
        if (target != null) target.GetComponent<Character>().Attacked();
    }
}

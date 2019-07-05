using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Usable : MonoBehaviour
{
    private WorldGen world;
    private Inventory inventory;
    public GameObject laser;
    public string type;

    private void Awake()
    {
        world = GameObject.Find("World").GetComponent<WorldGen>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
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

            GameObject hit = world.Get(x, y);
            if (hit)
            {
                inventory.AddItem(world.PickUp(x, y));
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

            GameObject hit = world.Get(x, y);
            if (inventory.ItemSelected() && notPlayer)
            {
                if (world.Empty(x, y)) world.Place(x, y, inventory.RemoveItem());
            }
        }
    }

    private void Laser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject attack = Instantiate(laser, transform);
            Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseLocation.x = mouseLocation.x - attack.transform.position.x;
            mouseLocation.y = mouseLocation.y - attack.transform.position.y;
            float angle = Mathf.Atan2(mouseLocation.y, mouseLocation.x) * Mathf.Rad2Deg;
            attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            StartCoroutine(Delete(attack, .5f));
        }
    }

    IEnumerator Delete(GameObject attack, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(attack);
    }

    public void Use()
    {
        if (type == "Pickaxe") Pickaxe();
        if (type == "Block") Block();
        if (type == "Laser") Laser();
    }
}

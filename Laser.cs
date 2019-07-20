using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Laser : Ability
{

    protected GameObject laser;

    protected override void Awake()
    {
        base.Awake();
        laser = (GameObject)Resources.Load("Prefab/Laser", typeof(GameObject));
    }

    public override int GetRange()
    {
        return 5;
    }

    public override void Use()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject attack = Instantiate(laser);
            attack.SetActive(true);
            attack.transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
            Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Mathf.RoundToInt(mouseLocation.x) != player.transform.position.x && Mathf.RoundToInt(mouseLocation.y) != player.transform.position.y)
            {
                Destroy(attack);
                return;
            }
            mouseLocation.x = mouseLocation.x - attack.transform.position.x;
            mouseLocation.y = mouseLocation.y - attack.transform.position.y;
            float angle = Mathf.Atan2((int)mouseLocation.y, (int)mouseLocation.x) * Mathf.Rad2Deg;
            attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            attack.GetComponent<Effects>().Animation();
        }
    }

    public override List<GameObject> GetArea()
    {
        int range = GetRange();
        int playerX = (int)transform.position.x;
        int playerY = (int)transform.position.y;

        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int mouseX = Mathf.RoundToInt(mouseLocation.x);
        int mouseY = Mathf.RoundToInt(mouseLocation.y);
        List<GameObject> area = new List<GameObject>();

        if (Mathf.Abs(mouseX - playerX) > 5 && mouseX > playerX) mouseX = playerX + 5;
        if (Mathf.Abs(mouseX - playerX) > 5 && mouseX < playerX) mouseX = playerX - 5;
        if (Mathf.Abs(mouseY - playerY) > 5 && mouseY > playerY) mouseY = playerY + 5;
        if (Mathf.Abs(mouseY - playerY) > 5 && mouseY < playerY) mouseY = playerY - 5;
        if (mouseX != playerX && mouseY != playerY) return area;

        int dist = 1;
        area.Add(Get(mouseX, mouseY));
        area.Add(GetGround(mouseX, mouseY));
        while (dist < range)
        {
            int changeX = 0;
            int changeY = 0;

            if (mouseX > playerX) changeX = -dist;
            else if (mouseX < playerX) changeX = dist;

            if (mouseY > playerY) changeY = -dist;
            else if (mouseY < playerY) changeY = dist;

            if (mouseX + changeX == playerX && mouseY + changeY == playerY) break;
            area.Add(Get(mouseX + changeX, mouseY + changeY));
            area.Add(GetGround(mouseX + changeX, mouseY + changeY));

            dist += 1;
        }
        return area;
    }
}

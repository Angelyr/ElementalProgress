using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Melee : Ability
{
    protected override void Init()
    {
        name = "Melee";
        range = 1;
        cooldown = 1;
        description = "Deals damage to any target in range";
    }

    public override void Use()
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

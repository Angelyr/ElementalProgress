using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Melee : Ability
{
    public override int GetRange()
    {
        return 1;
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

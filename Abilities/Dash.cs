using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        name = "Dash";
        range = 3;
        cooldown = 5;
        description = "Move in a straight line to any tile in range";
        targetType = "line";
    }

    public override void Use(Vector2Int target)
    {
        if (!WithInRange(target)) return;
        WorldController.GetTile(MyPosition()).GetComponent<Character>().SetPosition(target);
    }
}

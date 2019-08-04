using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override List<GameObject> GetArea(Vector2Int target)
    {

        if (WorldController.GetTile(ClosestPositionInRange(target)) != null) return null;

        List<GameObject> area = new List<GameObject>();
        area.Add(WorldController.GetGround(ClosestPositionInRange(target)));
        return area;
    }

    protected override void Init()
    {
        name = "Dash";
        range = 3;
        cooldown = 5;
        description = "Move in a straight line to any tile in range";
    }

    public override void Use(Vector2Int target)
    {
        WorldController.MoveToWorldPoint(WorldController.GetTile(MyPosition()).transform, ClosestPositionInRange(target));
    }
}

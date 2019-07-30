using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override List<GameObject> GetArea()
    {
        if (WorldController.GetTile(MousePositionInRange()) != null) return null;

        List<GameObject> area = new List<GameObject>();
        area.Add(WorldController.GetGround(MousePositionInRange()));
        return area;
    }

    protected override void Init()
    {
        name = "Dash";
        range = 3;
        cooldown = 5;
        description = "Move in a straight line to any tile in range";
    }

    public override void Use()
    {
        WorldController.MoveToWorldPoint(player.transform, MousePositionInRange());
    }
}

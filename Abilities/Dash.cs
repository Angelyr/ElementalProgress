using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override string GetDescription()
    {
        return "Dash:\nMoves the character in a straight line to a different location\nRange: 3";
    }

    public override List<GameObject> GetArea()
    {
        if (WorldController.GetTile(MousePositionInRange()) != null) return null;

        List<GameObject> area = new List<GameObject>();
        area.Add(WorldController.GetGround(MousePositionInRange()));
        return area;
    }

    public override int GetRange()
    {
        return 3;
    }

    public override void Use()
    {
        WorldController.MoveToWorldPoint(player.transform, MousePositionInRange());
    }
}

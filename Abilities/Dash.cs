using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ShowRange()
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(MyPosition());
        foreach(Vector2Int adjPosition in adjacent)
        {
            Vector2Int position = adjPosition;
            int dist = 1;
            while(dist <= GetRange())
            {
                if (WorldController.GetGround(position) == null) break;
                WorldController.GetGround(position).GetComponent<Entity>().Outline();
                outlinedObjects.Add(WorldController.GetGround(position));
                position = MoveAway(position, MyPosition());
                dist++;
            }
        }
    }

    public override List<GameObject> GetArea(Vector2Int target)
    {
        target = ClosestPositionInRange(target);
        target = Straighten(target);

        if (WorldController.GetTile(target) != null) return null;

        List<GameObject> area = new List<GameObject>();
        area.Add(WorldController.GetGround(target));
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

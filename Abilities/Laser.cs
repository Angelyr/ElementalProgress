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

    protected override void Init()
    {
        name = "Line";
        range = 5;
        cooldown = 5;
        description = "Hits every enemy in a line";
    }

    public override void ShowRange()
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(MyPosition());
        foreach (Vector2Int adjPosition in adjacent)
        {
            Vector2Int position = adjPosition;
            int dist = 1;
            while (dist <= GetRange())
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
        List<GameObject> area = new List<GameObject>();
        Vector2Int position = MyPosition();
        target = Straighten(target);
        position = MoveToward(position, target);
        for(int dist=0; dist < GetRange(); dist++)
        {
            area.AddRange(WorldController.GetAll(position));
            position = MoveToward(position, target);
        }
        return area;
    }
}

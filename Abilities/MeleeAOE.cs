using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAOE : Ability
{
    public override List<GameObject> GetArea()
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(PlayerPosition());
        List<GameObject> area = new List<GameObject>();
        area.AddRange(WorldController.GetAll(PlayerPosition()));
        foreach(Vector2Int position in adjacent)
        {
            area.AddRange(WorldController.GetAll(position));
        }
        return area;
    }

    public override int GetRange()
    {
        return 1;
    }

    public override void Use()
    {
        foreach(GameObject tile in GetArea())
        {
            tile.GetComponent<Entity>().Attacked();
        }
    }
}

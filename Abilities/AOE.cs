using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : Ability
{
    public override List<GameObject> GetArea()
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(GetIntPlayerPosition());
        List<GameObject> area = new List<GameObject>();
        area.Add(player);
        area.Add(WorldController.backBlocks[((int)player.transform.position.x, (int)player.transform.position.y)]);
        foreach(Vector2Int position in adjacent)
        {
            if (WorldController.blocks.ContainsKey((position.x, position.y)))
            {
                area.Add(WorldController.blocks[(position.x, position.y)]);
            }
            if (WorldController.backBlocks.ContainsKey((position.x, position.y)))
            {
                area.Add(WorldController.backBlocks[(position.x, position.y)]);
            }
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

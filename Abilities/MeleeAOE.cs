using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAOE : Ability
{
    public override List<GameObject> GetArea()
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(PlayerPosition());
        List<GameObject> area = new List<GameObject>();
        area.Add(player);
        area.Add(WorldController.floorTiles[((int)player.transform.position.x, (int)player.transform.position.y)]);
        foreach(Vector2Int position in adjacent)
        {
            if (WorldController.tiles.ContainsKey((position.x, position.y)))
            {
                area.Add(WorldController.tiles[(position.x, position.y)]);
            }
            if (WorldController.floorTiles.ContainsKey((position.x, position.y)))
            {
                area.Add(WorldController.floorTiles[(position.x, position.y)]);
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

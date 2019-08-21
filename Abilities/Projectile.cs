using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{
    public override List<GameObject> GetArea(Vector2Int target)
    {
        List<GameObject> area = new List<GameObject>();
        HashSet<GameObject> line = new HashSet<GameObject>();
        Vector2 start = MyPosition();

        while (start != target)
        {
            start = Vector2.MoveTowards(start, target, .05f);
            Vector2Int tile = Vector2Int.RoundToInt(start);

            if (tile == MyPosition()) continue;
            line.Add(WorldController.GetGround(tile));
            line.Add(WorldController.GetTile(tile));
            if (WorldController.GetTile(tile) != null) break;
        }
        foreach (GameObject tile in line)
        {
            area.Add(tile);
        }
        return area;
    }

    public override void Use(Vector2Int target)
    {
        if (!WithInRange(target)) return;
        GameObject affected = WorldController.GetTile(target);

        ApplyEffects(affected);
        if (affected == null) return;
        if (affected.GetComponent<Character>() == null) return;
        affected.GetComponent<Character>().Attacked();
    }

    protected override void Init()
    {
        name = "Projectile";
        range = 10;
        cooldown = 3;
        description = "Fires in a line and deals damage agains the first enemy it hits";
        targetType = "line";
    }

}

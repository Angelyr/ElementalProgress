using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{
    public override List<GameObject> GetArea()
    {
        Vector2Int mouse = MousePositionInRange();
        Vector2Int projectile = PlayerPosition();
        if (projectile.x != mouse.x && projectile.y != mouse.y) return null;

        projectile = MoveToward(projectile, mouse);
        while(projectile.x != mouse.x || projectile.y != mouse.y)
        {
            if (WorldController.GetTile(projectile) != null)
            {
                return WorldController.GetAll(projectile);
            }

            projectile = MoveToward(projectile, mouse);
        }

        return WorldController.GetAll(mouse);
    }

    private Vector2Int MoveToward(Vector2Int start, Vector2Int end)
    {
        if (start.x < end.x) start.x += 1;
        if (start.x > end.x) start.x -= 1;
        if (start.y < end.y) start.y += 1;
        if (start.y > end.y) start.y -= 1;
        return start;
    }

    public override int GetRange()
    {
        return 7;
    }

    public override void Use()
    {
        if (GetArea() == null) return;
        foreach (GameObject tile in GetArea())
        {
            tile.GetComponent<Entity>().Attacked();
        }
    }
}

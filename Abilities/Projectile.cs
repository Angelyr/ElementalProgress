using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{
    public override List<GameObject> GetArea(Vector2Int target)
    {
        Vector2Int closestTarget = ClosestPositionInRange(target);
        Vector2Int projectile = MyPosition();
        if (projectile.x != closestTarget.x && projectile.y != closestTarget.y) return null;

        projectile = MoveToward(projectile, closestTarget);
        while(projectile.x != closestTarget.x || projectile.y != closestTarget.y)
        {
            if (WorldController.GetTile(projectile) != null)
            {
                return WorldController.GetAll(projectile);
            }

            projectile = MoveToward(projectile, closestTarget);
        }

        return WorldController.GetAll(closestTarget);
    }

    private Vector2Int MoveToward(Vector2Int start, Vector2Int end)
    {
        if (start.x < end.x) start.x += 1;
        if (start.x > end.x) start.x -= 1;
        if (start.y < end.y) start.y += 1;
        if (start.y > end.y) start.y -= 1;
        return start;
    }

    protected override void Init()
    {
        name = "Projectile";
        range = 10;
        cooldown = 3;
        description = "Fires in a line and deals damage agains the first enemy it hits";
    }

}

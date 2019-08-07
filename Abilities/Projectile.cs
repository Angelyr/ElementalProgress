using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{
    public override List<GameObject> GetArea(Vector2Int target)
    {
        target = ClosestPositionInRange(target);
        target = Straighten(target);

        Vector2Int projectile = MyPosition();

        projectile = MoveToward(projectile, target);
        while(projectile.x != target.x || projectile.y != target.y)
        {
            if (WorldController.GetTile(projectile) != null)
            {
                return WorldController.GetAll(projectile);
            }

            projectile = MoveToward(projectile, target);
        }

        return WorldController.GetAll(target);
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

    protected override void Init()
    {
        name = "Projectile";
        range = 10;
        cooldown = 3;
        description = "Fires in a line and deals damage agains the first enemy it hits";
        targetType = "line";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAoe : Ability
{
    public override List<GameObject> GetArea(Vector2Int target)
    {
        return WorldController.GetAll(ClosestPositionInRange(target));
    }

    protected override void Init()
    {
        name = "Single Target";
        range = 7;
        cooldown = 3;
        description = "Deals damage to any target in range";
        targetType = "target";
    }
}

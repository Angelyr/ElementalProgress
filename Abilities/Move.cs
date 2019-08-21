using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Ability
{
    protected override void Init()
    {
        name = "Walk";
        range = 0;
        cooldown = 0;
        description = "Move to target tile";
        targetType = "target";
    }
}

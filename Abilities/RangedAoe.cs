using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAoe : Ability
{
    protected override void Init()
    {
        name = "Single Target";
        range = 4;
        cooldown = 3;
        description = "Deals damage to any target in range";
        targetType = "target";
    }
}

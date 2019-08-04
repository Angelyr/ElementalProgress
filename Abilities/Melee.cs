using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Melee : Ability
{
    protected override void Init()
    {
        name = "Melee";
        range = 1;
        cooldown = 1;
        description = "Deals damage to any target in range";
    }

}

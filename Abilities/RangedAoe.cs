using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAoe : Ability
{
    public override List<GameObject> GetArea()
    {
        return WorldController.GetAll(MousePosition());
    }

    protected override void Init()
    {
        name = "Single Target";
        range = 7;
        cooldown = 3;
        description = "Deals damage to any target in range";
    }

    public override void Use()
    {
        foreach (GameObject tile in GetArea())
        {
            tile.GetComponent<Entity>().Attacked();
        }
    }
}

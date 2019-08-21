using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex : Enemy
{
    private Ability rangeAoe;

    protected override string Attack()
    {
        string result = rangeAoe.TryAbility(PlayerPosition());
        if (result == "success") ChangeAP(-1);
        return result;
    }

    protected override void Init()
    {
        rangeAoe = Instantiate(Resources.Load<GameObject>("Prefab/Single").GetComponent<Ability>(), transform);
        name = "Complex";
        health = 3;
        ap = 3;
        spawnChance = 1;
    }
}

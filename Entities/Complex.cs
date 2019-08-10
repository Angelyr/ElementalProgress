using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex : Enemy
{
    private Ability rangeAoe;

    protected override string Attack()
    {
        string result = rangeAoe.TryAbility(PlayerPosition());
        Debug.Log(result);
        if (result == "success") ConsumeAP();
        return result;
    }

    protected override void Init()
    {
        rangeAoe = Instantiate(Resources.Load<GameObject>("Prefab/Single").GetComponent<Ability>(), transform);
        name = "Complex";
        health = 3;
        ap = 3;
    }
}

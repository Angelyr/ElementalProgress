using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Ability melee;

    protected override string Attack()
    {
        string result = melee.TryAbility(PlayerPosition());
        if (result == "success") ConsumeAP();
        return result;
    }


    protected override void Init()
    {
        melee = Instantiate(Resources.Load<GameObject>("Prefab/Melee").GetComponent<Ability>(), transform);
        name = "Slime";
        health = 3;
        ap = 3;
    }

    
}

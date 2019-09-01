using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Ability melee;
    private Effect oil = new Oil();

    protected override string Attack()
    {
        string result = melee.TryAbility(PlayerPosition());
        if (result == "success") ChangeAP(-1);
        return result;
    }

    protected override void Move()
    {
        base.Move();
        oil.Clone().Apply(WorldController.GetGround(MyPosition()));
    }

    protected override void Init()
    {
        melee = Instantiate(Resources.Load<GameObject>("Prefab/Melee").GetComponent<Ability>(), transform);
        name = "Slime";
        health = 3;
        ap = 3;
    }
}

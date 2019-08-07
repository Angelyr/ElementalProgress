using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex : Enemy
{
    private void Start()
    {
        InvokeRepeating("AI", 2, 1);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
        name = "Complex";
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (Attack()) return;
        else if (PathToPlayer()) return;
        //else if (RunAway()) return;
    }

    protected override bool Attack()
    {
        //Try ranged attack
        //Try melee attack
        return false;
    }


    protected override void Init()
    {
        health = 3;
        ap = 3;
    }
}

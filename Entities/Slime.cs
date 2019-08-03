using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private void Start()
    {
        InvokeRepeating("AI", 2, 1);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
        name = "Slime";
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;

        if (Attack()) return;
        else if (PathToPlayer()) return;
        else if (RunAway()) return;
    }

    protected override void Init()
    {
        health = 3;
        ap = 3;
    }

    
}

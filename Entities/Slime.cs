using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Ability melee;

    private void Start()
    {
        InvokeRepeating("AI", 2, 1);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
        name = "Slime";
        melee = Resources.Load<GameObject>("Prefab/Melee").GetComponent<Melee>();
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;

        if (Attack()) return;
        else if (PathToPlayer()) return;
        //else if (RunAway()) return;
    }

    private bool Attack2()
    {

        return false;
    }

    protected override void Init()
    {
        health = 3;
        ap = 3;
    }

    
}

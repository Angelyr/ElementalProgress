using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Ability melee;


    private void Start()
    {
        InvokeRepeating("AI", 2, 1.5f);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
        melee = Instantiate(Resources.Load<GameObject>("Prefab/Melee").GetComponent<Ability>(), transform);
        name = "Slime";
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;

        string result = Attack();
        if (result == "success")
        {
            return;
        }
        else if(result == "outofrange")
        {
            if (PathToPlayer()) return;
        }
        else if(result == "notaligned")
        {
            if (LineUp(PlayerPosition())) return;
        }
        else if (PathToPlayer()) return;

        ConsumeAP();

        //else if (RunAway()) return;
    }


    protected override string Attack()
    {
        ConsumeAP();
        return melee.TryAbility(PlayerPosition());
    }


    protected override void Init()
    {
        health = 3;
        ap = 3;
    }

    
}

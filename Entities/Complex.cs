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


    protected override void Init()
    {
        health = 3;
        ap = 3;
    }
}

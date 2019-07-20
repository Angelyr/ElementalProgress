using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    protected int ap = 5;
    protected int maxAP = 5;
    protected int health = 5;
    protected int maxHealth = 5;
    public GameObject myTurnUI;

    protected int enterTurnDistance = 10;

    public void Init(int ap, int health)
    {
        base.Init();
        this.ap = ap;
        this.maxAP = ap;
        this.health = health;
        this.maxHealth = health;
    }


    public virtual void StartTurn()
    {
        ap = maxAP;
    }

    public abstract void Attacked();

    public abstract void StartConcurrentTurn();

}

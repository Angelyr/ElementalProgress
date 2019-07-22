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

    public virtual void StartTurn()
    {
        ap = maxAP;
    }
    public abstract bool StartConcurrentTurn();

}

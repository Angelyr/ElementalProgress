using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    protected int ap;
    protected int maxAP;
    protected int health;
    protected int maxHealth;
    public GameObject myTurnUI;

    protected override void Awake()
    {
        base.Awake();
        Init();
        maxAP = ap;
        maxHealth = health;
    }

    public int Health()
    {
        return health;
    }

    public int MaxHealth()
    {
        return maxHealth;
    }

    public virtual void StartTurn()
    {
        player.GetComponent<PlayerController>().myCamera.SetCameraFocus(transform);
        ap = maxAP;
    }
    public abstract bool StartConcurrentTurn();

    protected abstract void Init();
}

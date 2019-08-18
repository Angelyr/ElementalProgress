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

    protected Vector2Int targetPosition;
    protected const float moveSpeed = .1f;
    protected bool moving = false;

    protected override void Awake()
    {
        base.Awake();
        Init();
        maxAP = ap;
        maxHealth = health;
        targetPosition = MyPosition();
    }

    private void FixedUpdate()
    {
        MoveAnimation();
    }

    private void MoveAnimation()
    {
        if (targetPosition != (Vector2)transform.position) moving = true;
        if (targetPosition == (Vector2)transform.position && moving)
        {
            Move();
            moving = false;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);
    }

    protected void SetDirection(Vector2Int direction)
    {
        targetPosition = MyPosition() + direction;
    }

    public void SetPosition(Vector2Int position)
    {
        targetPosition = position;
    }

    protected virtual void Move()
    {
        WorldController.MoveToWorldPoint(transform, targetPosition);
        ChangeAP(ap - 1);
    }


    protected virtual void ChangeAP(int newAP)
    {
        if (ap < 1) return;

        ap = newAP;
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
        player.GetComponent<PlayerController>().myCamera.SetPosition(transform);
        ap = maxAP;
    }
    public abstract bool StartConcurrentTurn();

    protected abstract void Init();
}

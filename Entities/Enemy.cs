using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    int enterTurnDistance = 7;

    protected void EnterTurnOrder()
    {
        if (WorldController.GetDistanceFromPlayer(MyPosition()) < enterTurnDistance)
        {
            TurnOrder.AddTurn(gameObject);
        }
        else TurnOrder.AddConcurrentTurn(gameObject);
    }

    public override bool StartConcurrentTurn()
    {
        if (PlayerWithInRange(enterTurnDistance))
        {
            TurnOrder.AddTurn(gameObject);
            player.GetComponent<PlayerUI>().SetMessage("Enemy In Range");
            StartCoroutine(player.GetComponent<PlayerController>().myCamera.PointCamera(transform));
            return false;
        }
        PathToPlayer();
        return true;
    }

    protected void PathToPlayer()
    {
        Vector2Int closest = WorldController.GetClosestTileToPlayer(MyPosition());
        WorldController.MoveToWorldPoint(transform, closest);
    }

    protected void Attack()
    {
        ConsumeAP();
        if (PlayerWithInRange(1)) player.GetComponent<PlayerController>().Attacked();
    }

    protected void MoveRandomly()
    {
        int xMove = Random.Range(-1, 2);
        int yMove = Random.Range(-1, 2);
        Move(xMove, yMove);
    }

    protected void Move(int xMove, int yMove)
    {
        ConsumeAP();
        WorldController.MoveWorldLocation(transform, xMove, yMove);
    }

    protected void ConsumeAP()
    {
        if (ap < 1)
        {
            TurnOrder.EndTurn(gameObject);
            return;
        }
        ap -= 1;
    }

    public override void Attacked()
    {
        health -= 1;
        if(health == 0) WorldController.Kill(gameObject);
    }

    protected abstract override void Init();
}

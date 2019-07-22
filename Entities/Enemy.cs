using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected int enterTurnDistance = 7;

    private void Start()
    {
        InvokeRepeating("AI", 2, 1);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        PathToPlayer();
        Attack();
    }

    private void EnterTurnOrder()
    {
        if (PlayerWithInRange(enterTurnDistance)) TurnOrder.AddTurn(gameObject);
        else TurnOrder.AddConcurrentTurn(gameObject);
    }

    public override bool StartConcurrentTurn()
    {
        if (PlayerWithInRange(enterTurnDistance))
        {
            TurnOrder.AddTurn(gameObject);
            return false;
        }
        PathToPlayer();
        return true;
    }

    private void PathToPlayer()
    {
        Vector2Int closest = WorldController.GetClosestTileToPlayer(GetMyPosition());
        WorldController.MoveToWorldPoint(transform, closest);
    }

    private void PathToPlayer2()
    {
        int playerX = (int)player.transform.position.x;
        int playerY = (int)player.transform.position.y;
        int myX = (int)transform.position.x;
        int myY = (int)transform.position.y;

        if (playerX > myX) Move(1, 0);
        else if (playerX < myX) Move(-1, 0);
        else if (playerY > myY) Move(0, 1);
        else if (playerY < myY) Move(0, -1);
    }

    private void Attack()
    {
        ConsumeAP();
        if (PlayerWithInRange(1)) player.GetComponent<PlayerController>().Attacked();
    }

    private void MoveRandomly()
    {
        int xMove = Random.Range(-1, 2);
        int yMove = Random.Range(-1, 2);
        Move(xMove, yMove);
    }

    //move character after input
    private void Move(int xMove, int yMove)
    {
        ConsumeAP();
        WorldController.MoveWorldLocation(transform, xMove, yMove);
    }

    private void ConsumeAP()
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
        WorldController.Kill(gameObject);
    }
}

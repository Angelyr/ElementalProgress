using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;
using static TurnOrder;

public class Enemy : Character
{
    private void Awake()
    {
        base.Init();
    }

    private void Start()
    {
        InvokeRepeating("AI", 2, 1);
        AddTurn(gameObject);
        AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    private void AI()
    {
        if (!MyTurn(gameObject)) return;
        PathToPlayer();
        Attack();
    }

    private bool EnterTurnOrder()
    {
        if (!PlayerWithInRange(enterTurnDistance)) return false;
        AddTurn(gameObject);
        return true;
    }

    //return false if couldn't start concurrent turn
    public override void StartConcurrentTurn()
    {
        if (PlayerWithInRange(enterTurnDistance))
        {
            AddTurn(gameObject);
            return;
        }
        PathToPlayer();
        EndConcurrentTurn(gameObject);
    }

    private void PathToPlayer()
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
        MoveWorldLocation(transform, xMove, yMove);
    }

    private void ConsumeAP()
    {
        if (ap < 1)
        {
            EndTurn(gameObject);
            return;
        }
        ap -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            Attacked();
        }
    }

    public override void Attacked()
    {
        Kill(gameObject);
    }
}

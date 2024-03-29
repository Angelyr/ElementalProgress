﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected int spawnChance = 2;

    public int SpawnChance()
    {
        return spawnChance;
    }

    //Mono Behavior

    private void Start()
    {
        InvokeRepeating("AI", 2, 1.5f);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    //Private

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap <= 0)
        {
            TurnOrder.EndTurn(gameObject);
            return;
        }

        string result = Attack();
        if (result == "success")
        {
            return;
        }
        else if(result == "selected")
        {
            return;
        }
        else if (result == "outofrange")
        {
            if (PathToPlayer()) return;
        }
        else if (result == "notaligned")
        {
            if (LineUp(PlayerPosition())) return;
        }
        else if (PathToPlayer()) return;

        TurnOrder.EndTurn(gameObject);

        //else if (RunAway()) return;
    }

    protected bool OutOfSight()
    {
        if (WorldController.GetDistanceFromPlayer(MyPosition()) > Settings.sightDistance)
        {
            return true;
        }
        return false;
    }

    protected void EnterTurnOrder()
    {
        if (WorldController.GetDistanceFromPlayer(MyPosition()) < Settings.enterTurnDistance)
        {
            TurnOrder.AddTurn(gameObject);
        }
        else TurnOrder.AddConcurrentTurn(gameObject);
    }

    protected bool PathToPlayer()
    {
        Vector2Int closest = WorldController.GetClosestTileToPlayer(MyPosition());
        if (closest == MyPosition()) return false;
        SetPosition(closest);
        return true;
    }

    protected virtual string Attack()
    {
        if (PlayerWithInRange(1) == false) return "fail";
        if (PlayerWithInRange(1)) player.GetComponent<PlayerController>().Attacked();
        return "success";
    }

    protected void MoveRandomly()
    {
        int x = Random.Range(-1, 2);
        int y = Random.Range(-1, 2);
        SetPosition(MyPosition() + new Vector2Int(x, y));
    }

    protected bool PathToDistance(int distance)
    {
        if (WorldController.GetDistanceFromPlayer(MyPosition()) > distance) PathToPlayer();
        else return false;

        return true;
    }

    protected bool LineUp(Vector2Int target)
    {
        int distX = Mathf.Abs(MyPosition().x - target.x);
        int distY = Mathf.Abs(MyPosition().y - target.y);

        if (distX < distY/2 && MyPosition().x > target.x) SetPosition(MyPosition() + Vector2Int.left);
        else if (distX < distY/2 && MyPosition().x < target.x) SetPosition(MyPosition() + Vector2Int.right);
        else if (distX/2 > distY && MyPosition().y > target.y) SetPosition(MyPosition() + Vector2Int.down);
        else if (distX/2 > distY && MyPosition().y < target.y) SetPosition(MyPosition() + Vector2Int.up);
        else return false;

        return true;
    }

    protected bool RunAway()
    {
        Vector2Int farthest = WorldController.FarthestTileFromPlayer(MyPosition());
        if (farthest == MyPosition()) return false;
        SetPosition(farthest);
        return true;
    }

    //Public

    public override bool StartConcurrentTurn()
    {
        if (PlayerWithInRange(Settings.enterTurnDistance))
        {
            TurnOrder.AddTurn(gameObject);
            player.GetComponent<PlayerUI>().SetMessage("Enemy In Range");
            return false;
        }
        if (OutOfSight())
        {
            MoveRandomly();
        }
        else
        {
            PathToPlayer();
        }
        return true;
    }

    public override void Attacked()
    {
        health -= 1;
        if(health == 0) WorldController.Kill(gameObject);
    }

    //Abstact

    protected abstract override void Init();
}

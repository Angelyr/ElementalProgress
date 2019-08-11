using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected int enterTurnDistance = 7;
    protected int spawnChance = 2;

    public int SpawnChance()
    {
        return spawnChance;
    }

    private void Start()
    {
        InvokeRepeating("AI", 2, 1.5f);
        EnterTurnOrder();
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    private void AI()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap == 0)
        {
            TurnOrder.EndTurn(gameObject);
            return;
        }

        string result = Attack();
        if (result == "success")
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

    protected bool PathToPlayer()
    {
        Vector2Int closest = WorldController.GetClosestTileToPlayer(MyPosition());
        if (closest == MyPosition()) return false;
        MoveTo(closest);
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
        int xMove = Random.Range(-1, 2);
        int yMove = Random.Range(-1, 2);
        Move(xMove, yMove);
    }

    protected void Move(int xMove, int yMove)
    {
        WorldController.MoveWorldLocation(transform, xMove, yMove);
        ConsumeAP();
    }

    protected void MoveTo(Vector2Int target)
    {
        WorldController.MoveToWorldPoint(transform, target);
        ConsumeAP();
    }

    protected void ConsumeAP()
    {
        if (ap < 1) return;

        ap -= 1;
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

        if (distX < distY/2 && MyPosition().x > target.x) Move(-1, 0);
        else if (distX < distY/2 && MyPosition().x < target.x) Move(1, 0);
        else if (distX/2 > distY && MyPosition().y > target.y) Move(0, -1);
        else if (distX/2 > distY && MyPosition().y < target.y) Move(0, 1);
        else return false;

        return true;
    }

    protected bool RunAway()
    {
        Vector2Int farthest = WorldController.FarthestTileFromPlayer(MyPosition());
        if (farthest == MyPosition()) return false;
        MoveTo(farthest);
        return true;
    }

    public override void Attacked()
    {
        health -= 1;
        SetHealthUI();
        if(health == 0) WorldController.Kill(gameObject);
    }

    protected abstract override void Init();
}

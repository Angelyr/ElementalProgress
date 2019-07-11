using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Enemy : MonoBehaviour
{

    private bool myTurn = false;
    private int ap = 5;
    private int maxAp = 5;
    private int initiative = 0;

    private void Start()
    {
        InvokeRepeating("MoveRandomly", 2, 1);
        AddTurn(gameObject);
    }

    private void Update()
    {
    }

    public void StartTurn()
    {
        myTurn = true;
        ap = maxAp;
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
        if (ap < 1)
        {
            EndTurn(gameObject);
            return;
        }
        if (!myTurn) return;
        if (!Empty((int)transform.position.x + xMove, (int)transform.position.y + yMove)) return;
        transform.position = new Vector2(transform.position.x + xMove, transform.position.y + yMove);
        ap -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            RemoveFromTurn(gameObject);
            Destroy(gameObject);
        }
    }
}

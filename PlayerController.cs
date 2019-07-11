using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private int ap = 5;
    private int maxAP = 5;
    private bool myTurn = false;
    private int initiative = 100;

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void Start()
    {
        AddTurn(gameObject);
    }

    public void StartTurn()
    {
        myTurn = true;
        ap = maxAP;
    }

    //Every frame
    private void Update()
    {
        if (Input.GetKeyDown("w")) Move(0, 1);
        if (Input.GetKeyDown("a")) Move(-1, 0);
        if (Input.GetKeyDown("s")) Move(0, -1);
        if (Input.GetKeyDown("d")) Move(1, 0);
        if (Input.GetMouseButton(0)) inventory.UseSelected();
    }

    //move character after input
    private void Move(int xMove, int yMove)
    {
        if (ap < 1) return;
        if (!myTurn) return;
        if (!Empty((int)transform.position.x + xMove, (int)transform.position.y + yMove)) return;

        transform.position = new Vector2(transform.position.x + xMove, transform.position.y + yMove);
        if(!InfinteTurn()) ap -= 1;
    }
}

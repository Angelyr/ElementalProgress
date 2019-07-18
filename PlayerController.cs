using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;
using static TurnOrder;

public class PlayerController : Character
{
    private Inventory inventory;
    private GameObject apUI;
    private GameObject healthUI;

    private void Awake()
    {
        Init(5,5);
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        apUI = GameObject.Find("AP");
        healthUI = GameObject.Find("Health");
    }

    public override void StartTurn()
    {
        ChangeAP(maxAP);
    }

    private void Start()
    {
        AddTurn(gameObject);
        AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    //Every frame
    private void Update()
    {
        if (!MyTurn(gameObject)) return;
        if (ap < 1) return;

        if (Input.GetKeyDown("w")) Move(0, 1);
        if (Input.GetKeyDown("a")) Move(-1, 0);
        if (Input.GetKeyDown("s")) Move(0, -1);
        if (Input.GetKeyDown("d")) Move(1, 0);
        if (Input.GetMouseButton(0)) inventory.UseSelected();
    }

    public int GetRange()
    {
        return inventory.GetSelectedRange();
    }

    public Direction GetDirection()
    {
        return null;
    }

    //move character after input
    private void Move(int xMove, int yMove)
    {
        MoveWorldLocation(transform, xMove, yMove);
        if (!InfinteTurn()) ChangeAP(ap - 1);
    }

    private void ChangeAP(int newAP)
    {
        ap = newAP;
        apUI.GetComponent<UnityEngine.UI.Text>().text = "AP: " + newAP + "/" + maxAP;
    }

    private void ChangeHealth(int newHealth)
    {
        health = newHealth;
        healthUI.GetComponent<UnityEngine.UI.Text>().text = "Health: " + newHealth + "/" + maxHealth;
    }

    public override void Attacked()
    {
        ChangeHealth(health - 1);
    }
}

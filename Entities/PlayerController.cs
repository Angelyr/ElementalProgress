using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    public Inventory inventory;
    private GameObject apUI;
    private GameObject healthUI;

    protected override void Awake()
    {
        base.Awake();
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
        WorldController.SetDistanceFromPlayer();
        TurnOrder.AddTurn(gameObject);
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    //Every frame
    private void Update()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap < 1) return;

        if (Input.GetKeyDown("w")) Move(0, 1);
        if (Input.GetKeyDown("a")) Move(-1, 0);
        if (Input.GetKeyDown("s")) Move(0, -1);
        if (Input.GetKeyDown("d")) Move(1, 0);
        if (Input.GetMouseButtonDown(0)) inventory.UseSelected();
    }

    public int GetRange()
    {
        return inventory.GetSelectedRange();
    }

    public override bool StartConcurrentTurn()
    {
        StartTurn();
        return true;
    }

    //move character after input
    private void Move(int xMove, int yMove)
    {
        WorldController.MoveWorldLocation(transform, xMove, yMove);
        ChangeAP(ap - 1);
        WorldController.SetDistanceFromPlayer();
        TurnOrder.StartConcurrentTurns();
    }

    private void ChangeAP(int newAP)
    {
        if (TurnOrder.ConcurrentTurns()) return;
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

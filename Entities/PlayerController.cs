using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerController : Character
{
    public Inventory inventory;
    private GameObject apUI;
    private GameObject healthUI;
    public CameraScript myCamera;

    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        apUI = GameObject.Find("AP");
        healthUI = GameObject.Find("Health");
        myCamera = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }

    public override void StartTurn()
    {
        myCamera.SetCameraFocus(transform);
        gameObject.GetComponent<PlayerUI>().SetMessage("Your Turn");
        ChangeAP(maxAP);
        inventory.DecreaseCooldowns();
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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) inventory.UseSelected();
    }

    public int GetRange()
    {
        if (inventory.GetSelected() == null) return 0;
        return inventory.GetSelected().GetComponent<Ability>().GetRange();
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
        if (TurnOrder.ConcurrentTurns())
        {
            newAP = maxAP;
            inventory.DecreaseCooldowns();
        }
        ap = newAP;
        apUI.GetComponent<UnityEngine.UI.Text>().text = "AP: " + newAP + "/" + maxAP;
    }

    private void ChangeHealth(int newHealth)
    {
        health = newHealth;
        healthUI.GetComponent<UnityEngine.UI.Text>().text = "Health: " + newHealth + "/" + maxHealth;

        if(health == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void Attacked()
    {
        ChangeHealth(health - 1);
    }
}

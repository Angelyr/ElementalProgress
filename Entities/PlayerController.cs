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

    protected override void Init()
    {
        health = 5;
        ap = 5;
    }

    public override void StartTurn()
    {
        myCamera.SetPosition(transform);
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
 
    private void Update()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap < 1) return;

        MovementInput();
        
        if (Input.GetMouseButtonDown(0) && !HoveringUI()) inventory.UseSelected();
    }

    private void MovementInput()
    {
        if (moving) return;

        if (Input.GetKeyDown("w"))
        {
            SetDirection(Vector2Int.up);
        }
        if (Input.GetKeyDown("a"))
        {
            SetDirection(Vector2Int.left);
        }
        if (Input.GetKeyDown("s"))
        {
            SetDirection(Vector2Int.down);
        }
        if (Input.GetKeyDown("d"))
        {
            SetDirection(Vector2Int.right);
        }
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

    protected override void Move()
    {
        WorldController.MoveToWorldPoint(transform, targetPosition);
        inventory.ReSelect();
        ChangeAP(ap - 1);
        WorldController.SetDistanceFromPlayer();
        TurnOrder.StartConcurrentTurns();
    }

    protected override void ChangeAP(int newAP)
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

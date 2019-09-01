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

    //MonoBehavoir

    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        apUI = GameObject.Find("AP");
        healthUI = GameObject.Find("Health");
        myCamera = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        SetHealth(maxHealth);
    }

    private void Start()
    {
        WorldController.SetDistanceFromPlayer();
        TurnOrder.AddTurn(gameObject);
        WorldController.AddToWorld(gameObject, (int)transform.position.x, (int)transform.position.y);
    }

    protected override void Update()
    {
        base.Update();
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap < 1) return;

        MovementInput();

        if (Input.GetMouseButtonDown(0) && !HoveringUI()) inventory.UseSelected();
    }

    //Private

    //Public

    protected override void Init()
    {
        health = 10;
        ap = 10;
    }

    public override void StartTurn()
    {

        myCamera.SetPosition(transform);
        gameObject.GetComponent<PlayerUI>().SetMessage("Your Turn");
        SetAP(maxAP);
        inventory.DecreaseCooldowns();
        WorldController.EffectTurn();
    }

    

    private void MovementInput()
    {
        if (moving) return;

        if (Input.GetKey("w"))
        {
            SetDirection(Vector2Int.up);
        }
        if (Input.GetKey("a"))
        {
            SetDirection(Vector2Int.left);
        }
        if (Input.GetKey("s"))
        {
            SetDirection(Vector2Int.down);
        }
        if (Input.GetKey("d"))
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
        WorldController.MoveToWorldPoint(transform, mapPosition, targetPosition);
        mapPosition = targetPosition;
        inventory.ReSelect();
        ChangeAP(-1);
        WorldController.SetDistanceFromPlayer();
        TurnOrder.StartConcurrentTurns();
    }

    public override void ChangeAP(int change)
    {
        if (TurnOrder.ConcurrentTurns())
        {
            change = 0;
            inventory.DecreaseCooldowns();
        }
        ap += change;
        apUI.GetComponent<UnityEngine.UI.Text>().text = "AP: " + ap + "/" + maxAP;
    }

    public override void SetAP(int newAP)
    {
        if (TurnOrder.ConcurrentTurns())
        {
            newAP = maxAP;
            inventory.DecreaseCooldowns();
        }
        ap = newAP;
        apUI.GetComponent<UnityEngine.UI.Text>().text = "AP: " + ap + "/" + maxAP;
    }

    private void SetHealth(int newHealth)
    {
        health = newHealth;
        healthUI.GetComponent<UnityEngine.UI.Text>().text = "Health: " + newHealth + "/" + maxHealth;

        if(health == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void ChangeHealth(int change)
    {
        SetHealth(health + change);
    }

    public override void Attacked()
    {
        SetHealth(health - 1);
    }
}

﻿using System.Collections;
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
    
    private Rigidbody2D myRigidBody;
    private Vector2Int direction;
    private const float moveSpeed = .1f;

    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        apUI = GameObject.Find("AP");
        healthUI = GameObject.Find("Health");
        myCamera = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        myRigidBody = GetComponent<Rigidbody2D>();
        direction = Vector2Int.zero;
    }

    protected override void Init()
    {
        health = 5;
        ap = 5;
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
 
    private void Update()
    {
        if (!TurnOrder.MyTurn(gameObject)) return;
        if (ap < 1) return;

        if (Input.GetKeyDown("w") && direction == Vector2Int.zero)
        {
            direction = Vector2Int.up;
        }
        if (Input.GetKeyDown("a") && direction == Vector2Int.zero)
        {
            direction = Vector2Int.left;
        }
        if (Input.GetKeyDown("s") && direction == Vector2Int.zero)
        {
            direction = Vector2Int.down;
        }
        if (Input.GetKeyDown("d") && direction == Vector2Int.zero)
        {
            direction = Vector2Int.right;
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) inventory.UseSelected();
    }

    private void FixedUpdate()
    {
        Move();
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

    private void Move()
    {
        if (direction == Vector2Int.zero) return;

        Vector2 movement = myRigidBody.position + ((Vector2)direction * moveSpeed);
        myRigidBody.MovePosition(movement);

        if (movement == Vector2Int.RoundToInt(myRigidBody.position))
        {
            StopMove();
        }
    }

    private void StopMove()
    {
        WorldController.MoveToWorldPoint(transform, Vector2Int.RoundToInt(myRigidBody.position));
        ChangeAP(ap - 1);
        WorldController.SetDistanceFromPlayer();
        TurnOrder.StartConcurrentTurns();
        direction = Vector2Int.zero;
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

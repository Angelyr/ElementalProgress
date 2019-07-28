using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public abstract class Ability : Thing
{
    protected Inventory inventory;
    protected string description;
    protected int range;
    protected int cooldown;

    private int prevMouseX;
    private int prevMouseY;
    
    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public virtual void Init(string description, int range, int cooldown)
    {

    }

    public abstract void Use();

    public abstract int GetRange();

    public override string GetDescription()
    {
        return name;
    }

    public virtual List<GameObject> GetArea()
    {
        Vector2Int mouse = MousePositionInRange();
        List<GameObject> area = new List<GameObject>();

        area.Add(Get(mouse.x, mouse.y));
        area.Add(GetGround(mouse.x, mouse.y));
        return area;
    }

    protected Vector2Int MousePositionInRange()
    {
        Vector2Int mouse = MousePosition();
        Vector2Int player = PlayerPosition();

        if (Mathf.Abs(mouse.x - player.x) > GetRange() && mouse.x > player.x) mouse.x = player.x + GetRange();
        if (Mathf.Abs(mouse.x - player.x) > GetRange() && mouse.x < player.x) mouse.x = player.x - GetRange();
        if (Mathf.Abs(mouse.y - player.y) > GetRange() && mouse.y > player.y) mouse.y = player.y + GetRange();
        if (Mathf.Abs(mouse.y - player.y) > GetRange() && mouse.y < player.y) mouse.y = player.y - GetRange();

        return mouse;
    }

    protected bool WithInRange(int range, int targetX, int targetY)
    {
        if (Mathf.Abs(targetX - player.transform.position.x) <= range) return true;
        if (Mathf.Abs(targetY - player.transform.position.y) <= range) return true;
        return false;
    }

    public virtual int CoolDown()
    {
        return 5;
    }
}


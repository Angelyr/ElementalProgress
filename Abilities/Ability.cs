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
    protected int currCooldown = 0;

    private int prevMouseX;
    private int prevMouseY;
    
    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        Init();
    }

    protected abstract void Init();

    public int GetRange()
    {
        return range;
    }

    public override string GetDescription()
    {
        return name + ":\n" + description + "\nRange: " + range + "\nCooldown: " +  cooldown;
    }

    public virtual List<GameObject> GetArea(Vector2Int target)
    {
        Vector2Int closestTarget = ClosestPositionInRange(target);
        List<GameObject> area = new List<GameObject>();

        area.Add(Get(closestTarget.x, closestTarget.y));
        area.Add(GetGround(closestTarget.x, closestTarget.y));
        return area;
    }

    public virtual void Use(Vector2Int target)
    {
        if (GetArea(ClosestPositionInRange(target)) == null) return;
        foreach (GameObject affected in GetArea(ClosestPositionInRange(target)))
        {
            if (affected.GetComponent<Character>() == null) continue;
            affected.GetComponent<Character>().Attacked();
        }
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

    protected Vector2Int ClosestPositionInRange(Vector2Int target)
    {
        Vector2Int position = MyPosition();

        if (Mathf.Abs(target.x - position.x) > GetRange() && target.x > position.x) target.x = position.x + GetRange();
        if (Mathf.Abs(target.x - position.x) > GetRange() && target.x < position.x) target.x = position.x - GetRange();
        if (Mathf.Abs(target.y - position.y) > GetRange() && target.y > position.y) target.y = position.y + GetRange();
        if (Mathf.Abs(target.y - position.y) > GetRange() && target.y < position.y) target.y = position.y - GetRange();

        return target;
    }

    protected bool WithInRange(int range, int targetX, int targetY)
    {
        if (Mathf.Abs(targetX - player.transform.position.x) <= range) return true;
        if (Mathf.Abs(targetY - player.transform.position.y) <= range) return true;
        return false;
    }

    public int CoolDown()
    {
        return cooldown;
    }
}


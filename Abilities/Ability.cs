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
    protected int damage;

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

    protected Vector2Int Straighten(Vector2Int target)
    {
        Vector2Int[] options = new Vector2Int[4];
        Vector2Int position = MyPosition();

        options[0] = new Vector2Int(position.x, target.y);
        options[1] = new Vector2Int(target.x, position.y);
        options[2] = new Vector2Int(target.x+position.x, target.x+position.x);
        options[3] = new Vector2Int(target.y+position.y, target.y+position.y);

        Vector2Int closest = options[0];
        int closestDist = Mathf.Abs(options[0].x - target.x);
        closestDist += Mathf.Abs(options[0].y - target.y);
        foreach (Vector2Int point in options)
        {
            int dist = Mathf.Abs(point.x - target.x);
            dist += Mathf.Abs(point.y - target.y);

            if(dist < closestDist)
            {
                closest = point;
                closestDist = dist;
            }
        }

        return closest;
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

    protected Vector2Int MoveToward(Vector2Int start, Vector2Int end)
    {
        if (start.x < end.x) start.x += 1;
        if (start.x > end.x) start.x -= 1;
        if (start.y < end.y) start.y += 1;
        if (start.y > end.y) start.y -= 1;
        return start;
    }

    protected Vector2Int MoveAway(Vector2Int start, Vector2Int origin)
    {
        if (start.x < origin.x) start.x -= 1;
        if (start.x > origin.x) start.x += 1;
        if (start.y < origin.y) start.y -= 1;
        if (start.y > origin.y) start.y += 1;
        return start;
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


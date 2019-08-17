using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Ability : Thing
{
    protected Inventory inventory;
    protected string description;
    protected int range;
    protected int cooldown;
    protected int currCooldown = 0;
    protected int damage;
    protected string targetType;
    protected List<GameObject> outlinedObjects;

    private int prevMouseX;
    private int prevMouseY;
    
    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        outlinedObjects = new List<GameObject>();
        Init();
    }

    protected abstract void Init();

    public string GetTargetType()
    {
        return targetType;
    }

    public int GetRange()
    {
        return range;
    }

    public void ReduceCoolDown(int total)
    {
        currCooldown -= total;
    }

    public override string GetDescription()
    {
        return name + ":\n" + description + "\nRange: " + range + "\nCooldown: " +  cooldown;
    }

    public virtual string TryAbility(Vector2Int target)
    {
        if (currCooldown != 0) return "fail";
        if(GetTargetType() == "target")
        {
            if (!WithInRange(GetRange(), target)) return "outofrange";
            if (!InSight(target)) return "outofsight";
            Use(target);
            return "success";
        }

        if(GetTargetType() == "line")
        {
            if (!WithInRange(GetRange(), target)) return "outofrange";
            if (!Aligned(target)) return "notaligned";
            if (!InSight(target)) return "outofsight";
            Use(target);
            return "success";
        }

        return "fail";
    }

    public virtual void ShowRange()
    {
        List<Vector2Int> area = new List<Vector2Int>();
        for(int x=-GetRange(); x <= GetRange(); x++)
        {
            for(int y=-GetRange(); y <= GetRange(); y++)
            {
                area.Add(new Vector2Int(MyPosition().x + x, MyPosition().y + y));
            }
        }
        foreach(Vector2Int position in area)
        {
            if (WorldController.GetGround(position) == null) continue;
            WorldController.GetGround(position).GetComponent<Entity>().Outline();
            outlinedObjects.Add(WorldController.GetGround(position));
        }
    }

    public virtual void HideRange()
    {
        foreach(GameObject tile in outlinedObjects)
        {
            tile.GetComponent<Entity>().RemoveOutline();
        }
        outlinedObjects.Clear();
    }

    public virtual void Animate(Vector2Int target)
    {

    }

    public virtual List<GameObject> GetArea(Vector2Int target)
    {
        Vector2Int closestTarget = ClosestPositionInRange(target);
        List<GameObject> area = new List<GameObject>();

        area.Add(WorldController.Get(closestTarget.x, closestTarget.y));
        area.Add(WorldController.GetGround(closestTarget.x, closestTarget.y));
        return area;
    }

    public virtual void Use(Vector2Int target)
    {
        if (GetArea(ClosestPositionInRange(target)) == null) return;
        foreach (GameObject affected in GetArea(ClosestPositionInRange(target)))
        {
            if (affected == null) continue;
            if (affected.GetComponent<Character>() == null) continue;
            affected.GetComponent<Character>().Attacked();
        }
    }

    protected Vector2Int Straighten(Vector2Int target)
    {
        Vector2Int[] options = WorldController.GetAdjacent(MyPosition());

        Vector2Int closest = options[0];
        float closestDist = Vector2Int.Distance(options[0], target);
        foreach (Vector2Int point in options)
        {
            float dist = Vector2Int.Distance(point, target);

            if(dist < closestDist)
            {
                closest = point;
                closestDist = dist;
            }
        }

        int dist2 = 1;
        while(dist2 < GetRange())
        {
            if (closest == target) break;
            closest = MoveAway(closest, MyPosition());
            dist2++;
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

    protected bool InSight(Vector2Int target)
    {
        Vector2Int position = MyPosition();
        position = MoveToward(position, target);
        while (position != target)
        {
            if (WorldController.GetTile(position) != null) return false;
            position = MoveToward(position, target);
        }
        return true;
    }

    protected bool WithInRange(int range, int targetX, int targetY)
    {
        if (Mathf.Abs(targetX - player.transform.position.x) <= range) return true;
        if (Mathf.Abs(targetY - player.transform.position.y) <= range) return true;
        return false;
    }

    protected bool WithInRange(int range, Vector2Int target)
    {
        if((int)Vector2.Distance(target, transform.position) <= GetRange()) return true;
        return false;
    }

    protected bool Aligned(Vector2Int target)
    {
        if (target.x == MyPosition().x) return true;
        if (target.y == MyPosition().y) return true;
        if (target.x - MyPosition().x == target.y - MyPosition().y) return true;
        return false;
    }

    public int CoolDown()
    {
        return cooldown;
    }
}


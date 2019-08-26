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
    
    //MonoBehavoir
    protected override void Awake()
    {
        base.Awake();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        outlinedObjects = new List<GameObject>();
        Init();
    }

    //Abstract

    protected abstract void Init();

    //Private

    protected bool Selected()
    {
        if (outlinedObjects.Count == 0) return false;
        return true;
    }

    protected virtual List<Vector2Int> GetAllBlocksInRange()
    {
        List<Vector2Int> area = new List<Vector2Int>();
        for (int x = -GetRange(); x <= GetRange(); x++)
        {
            for (int y = -GetRange(); y <= GetRange(); y++)
            {
                Vector2Int target = MyPosition() + new Vector2Int(x, y);
                if (!WithInRange(target)) continue;
                area.Add(target);
            }
        }
        return area;
    }

    protected void ApplyEffects(GameObject target)
    {
        foreach(Effect effect in myEffects)
        {
            if (target == null || target.GetComponent<Entity>() == null) break;
            target.GetComponent<Entity>().Apply(effect);
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

    protected bool WithInRange(Vector2Int target)
    {
        if(Vector2.Distance(target, MyPosition()) <= GetRange() + .6) return true;
        return false;
    }

    protected bool Aligned(Vector2Int target)
    {
        if (target.x == MyPosition().x) return true;
        if (target.y == MyPosition().y) return true;
        if (target.x - MyPosition().x == target.y - MyPosition().y) return true;
        return false;
    }

    //Public

    public virtual string TryAbility(Vector2Int target)
    {
        if (currCooldown != 0) return "fail";
        if (!WithInRange(target)) return "outofrange";
        if (!InSight(target)) return "outofsight";
        if (!Selected())
        {
            SelectHightlight(target);
            return "selected";
        }
        else ClearHighlight();

        if (GetTargetType() == "target")
        {
            Use(target);
            return "success";
        }

        if (GetTargetType() == "line")
        {
            if (!Aligned(target)) return "notaligned";
            Use(target);
            return "success";
        }

        return "fail";
    }

    public int CoolDown()
    {
        return cooldown;
    }

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
        return name + ":\n" + description + "\nRange: " + range + "\nCooldown: " + cooldown;
    }

    public virtual void Use(Vector2Int target)
    {
        if (GetArea(ClosestPositionInRange(target)) == null) return;
        foreach (GameObject affected in GetArea(ClosestPositionInRange(target)))
        {
            ApplyEffects(affected);
            if (affected == null) continue;
            if (affected.GetComponent<Character>() == null) continue;
            affected.GetComponent<Character>().Attacked();
        }
    }


    public virtual void ShowRange()
    {
        List<Vector2Int> area = GetAllBlocksInRange();

        foreach (Vector2Int position in area)
        {
            if (WorldController.GetTile(position) != null && WorldController.GetTile(position).GetComponent<Block>() != null) continue;
            WorldController.GetGround(position).GetComponent<Entity>().Outline();
            outlinedObjects.Add(WorldController.GetGround(position));
        }
    }

    public virtual void ClearHighlight()
    {
        foreach (GameObject tile in outlinedObjects)
        {
            tile.GetComponent<Entity>().RemoveOutline();
        }
        outlinedObjects.Clear();
    }

    public void SelectHightlight(Vector2Int target)
    {
        foreach (GameObject tile in GetArea(target))
        {
            tile.GetComponent<Entity>().Outline();
            outlinedObjects.Add(tile);
        }
    }

    public virtual List<GameObject> GetArea(Vector2Int target)
    {
        List<GameObject> area = new List<GameObject>();
        if (!WithInRange(target)) return area;

        if (GetTargetType() == "target")
        {
            Vector2Int closestTarget = ClosestPositionInRange(target);

            area.Add(WorldController.Get(closestTarget.x, closestTarget.y));
            area.Add(WorldController.GetGround(closestTarget.x, closestTarget.y));
        }
        else
        {
            HashSet<GameObject> line = new HashSet<GameObject>();
            Vector2 start = MyPosition();
            while (start != target)
            {
                start = Vector2.MoveTowards(start, target, .05f);
                Vector2Int tile = Vector2Int.RoundToInt(start);

                if (tile == MyPosition()) continue;
                line.Add(WorldController.GetGround(tile));
                line.Add(WorldController.GetTile(tile));
            }

            foreach (GameObject tile in line)
            {
                area.Add(tile);
            }
        }

        return area;
    }


}


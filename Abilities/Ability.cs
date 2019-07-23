using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static WorldController;

public abstract class Ability : MonoBehaviour
{
    protected Inventory inventory;
    protected GameObject player;

    private int prevMouseX;
    private int prevMouseY;
    
    protected virtual void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        player = GameObject.Find("Player");
    }

    public abstract void Use();

    public abstract int GetRange();

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

    protected Vector2Int MousePosition()
    {
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int mouseX = Mathf.RoundToInt(mouseLocation.x);
        int mouseY = Mathf.RoundToInt(mouseLocation.y);
        return new Vector2Int(mouseX, mouseY);
    }

    protected Vector2Int PlayerPosition()
    {
        return new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
    }
}


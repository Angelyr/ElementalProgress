using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Entity : MonoBehaviour
{
    private GameObject myHighlight;
    protected GameObject player;

    public virtual void Init()
    {
        myHighlight = transform.Find("Highlight").gameObject;
        player = GameObject.Find("Player");
    }

    public void Highlight()
    {
        myHighlight.SetActive(true);
    }

    public void removeHighlight()
    {
        myHighlight.SetActive(false);
    }

    protected bool PlayerWithInRange(int range)
    {
        int playerX = (int)player.transform.position.x;
        int playerY = (int)player.transform.position.y;

        if (Mathf.Abs(playerX - transform.position.x) > range) return false;
        if (Mathf.Abs(playerY - transform.position.y) > range) return false;
        return true;
    }

    private void OnMouseEnter()
    {
        int range = player.GetComponent<PlayerController>().GetRange();
        if (!PlayerWithInRange(range)) return;
        Highlight();
    }

    public void PropogateHighlight(int up, int down, int left, int right, int range)
    {
        if (range == 0) return;
        Highlight();
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        if(up > 0)
        {
            if(Get(x, y + 1) != null) Get(x, y + 1).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);
            if(GetGround(x, y + 1) != null) GetGround(x, y + 1).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);

        }
        if (down > 0)
        {
            if (Get(x, y-1) != null) Get(x, y - 1).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);
            if (GetGround(x, y - 1) != null) GetGround(x, y - 1).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);

        }
        if (left > 0)
        {
            if (Get(x-1, y) != null) Get(x-1, y).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);
            if (GetGround(x-1, y) != null) GetGround(x-1, y).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);

        }
        if (right > 0)
        {
            if (Get(x+1, y) != null) Get(x+1, y).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);
            if (GetGround(x+1, y) != null) GetGround(x+1, y).GetComponent<Entity>().PropogateHighlight(up, down, left, right, range - 1);

        }
    }

    private void OnMouseExit()
    {
        removeHighlight();   
    }

    public class Direction
    {
        public int up;
        public int down;
        public int right;
        public int left;
    }

}

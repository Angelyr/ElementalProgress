using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Entity : MonoBehaviour
{
    private GameObject myHighlight;
    protected GameObject player;
    private List<GameObject> highlightedObjects;

    public virtual void Init()
    {
        myHighlight = transform.Find("Highlight").gameObject;
        player = GameObject.Find("Player");
        highlightedObjects = new List<GameObject>();
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

        List<GameObject> area = player.GetComponent<PlayerController>().inventory.GetSelected().GetComponent<Usable>().GetArea();
        if (area == null) return;
        foreach(GameObject entity in area)
        {
            if (entity == null) continue;
            entity.GetComponent<Entity>().Highlight();
            highlightedObjects.Add(entity);
        }
    }

    private void OnMouseExit()
    {
        removeHighlight();

        foreach (GameObject entity in highlightedObjects)
        {
            if (entity == null) continue;
            entity.GetComponent<Entity>().removeHighlight();
        }
        highlightedObjects.Clear();
    }

    public class Direction
    {
        public int up;
        public int down;
        public int right;
        public int left;
    }

}

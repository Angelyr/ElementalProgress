using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private GameObject myHighlight;
    protected GameObject player;
    private List<GameObject> highlightedObjects;

    protected virtual void Awake()
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

    public Vector2Int PlayerPosition()
    {
        return new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
    }

    public Vector2Int MyPosition()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public Vector2Int MousePosition()
    {
        int mouseX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        int mouseY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        return new Vector2Int(mouseX, mouseY);
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
        //if (!PlayerWithInRange(range)) return;
        //Highlight();
        if (player.GetComponent<PlayerController>().inventory.GetSelected() == null) return;
        List<GameObject> area = player.GetComponent<PlayerController>().inventory.GetSelected().GetComponent<Ability>().GetArea();
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

    public virtual void Attacked()
    {
    }

}

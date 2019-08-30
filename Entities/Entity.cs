using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : Thing
{
    private GameObject myHighlight;
    protected GameObject outline;
    private List<GameObject> highlightedObjects;

    //MonoBehavoir

    protected override void Awake()
    {
        base.Awake();
        myHighlight = transform.Find("Highlight").gameObject;
        if(transform.Find("Outline") != null) outline = transform.Find("Outline").gameObject;
        player = GameObject.Find("Player");
        highlightedObjects = new List<GameObject>();
    }

    //Private

    protected bool PlayerWithInRange(int range)
    {
        int playerX = (int)player.transform.position.x;
        int playerY = (int)player.transform.position.y;

        if (Mathf.Abs(playerX - transform.position.x) > range) return false;
        if (Mathf.Abs(playerY - transform.position.y) > range) return false;
        return true;
    }

    private void HighlightArea()
    {
        int range = player.GetComponent<PlayerController>().GetRange();
        if (player.GetComponent<PlayerController>().inventory.GetSelected() == null) return;
        List<GameObject> area = player.GetComponent<PlayerController>().inventory.GetSelected().GetComponent<Ability>().GetArea(MousePosition());
        if (area == null) return;
        foreach (GameObject entity in area)
        {
            if (entity == null) continue;
            entity.GetComponent<Entity>().Highlight();
            highlightedObjects.Add(entity);
        }
    }

    private void ClearHighlightedArea()
    {
        removeHighlight();
        foreach (GameObject entity in highlightedObjects)
        {
            if (entity == null) continue;
            entity.GetComponent<Entity>().removeHighlight();
        }
        highlightedObjects.Clear();
    }

    protected string HealthUI()
    {
        int health = GetComponent<Character>().Health();
        int maxHealth = GetComponent<Character>().MaxHealth();
        return health + "/" + maxHealth;
    }

    //Public

    public void Highlight()
    {
        myHighlight.SetActive(true);
    }
    
    public void Outline()
    {
        outline.SetActive(true);
    }

    public void RemoveOutline()
    {
        outline.SetActive(false);
    }

    public void removeHighlight()
    {
        myHighlight.SetActive(false);
    }

    public override void CreateHover()
    {
        if (GetDescription() == "") return;
        currHover = Instantiate(hoverUI);
        currHover.GetComponent<Hover>().Init(transform, GetDescription(), null, HealthUI());
    }

    public void UpdateHover()
    {
        currHover.GetComponent<Hover>().Init(transform, GetDescription(), null, HealthUI());
    }

    public void OnMouseEnter()
    {
        if (HoveringUI()) return;
        HighlightArea();
        CreateHover();
    }

    public void OnMouseExit()
    {
        ClearHighlightedArea();
        DestroyHover();
    }

    public virtual void Attacked()
    {
    }

    public void Apply(Effect effect)
    {
        effect.Apply(gameObject);
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : Thing
{
    private GameObject myHighlight;
    protected GameObject outline;
    private List<GameObject> highlightedObjects;

    protected override void Awake()
    {
        base.Awake();
        myHighlight = transform.Find("Highlight").gameObject;
        if(transform.Find("Outline") != null) outline = transform.Find("Outline").gameObject;
        player = GameObject.Find("Player");
        highlightedObjects = new List<GameObject>();
        hoverUI = Resources.Load<GameObject>("Prefab/CharacterHover");
    }

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

    public override void CreateHover()
    {
        if (GetDescription() == "") return;
        currHover = Instantiate(hoverUI, transform);
        currHover.transform.Find("Description").GetComponent<Text>().text = GetDescription();
        Vector3 newPosition = transform.position;
        newPosition.y += 1;
        currHover.transform.position = newPosition;

        SetHealthUI();
    }

    protected void SetHealthUI()
    {
        if (currHover == null) return;
        int health = GetComponent<Character>().Health();
        int maxHealth = GetComponent<Character>().MaxHealth();
        currHover.transform.Find("HealthBar/Bar/Text").GetComponent<Text>().text = health + "/" + maxHealth;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Thing : MonoBehaviour
{
    protected GameObject hoverUI;
    protected GameObject player;
    protected GameObject currHover;
    private GameObject UI;
    protected List<Effect> myEffects;
    protected string description;

    //MonoBehavior

    protected virtual void Awake()
    {
        hoverUI = Resources.Load<GameObject>("Prefab/Hover");
        player = GameObject.Find("Player");
        UI = GameObject.Find("UI");
        myEffects = new List<Effect>();
    }

    //Vector2Int

    public Vector2Int MyPosition()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public Vector2Int MousePosition()
    {
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int mouseX = Mathf.RoundToInt(mouseLocation.x);
        int mouseY = Mathf.RoundToInt(mouseLocation.y);
        return new Vector2Int(mouseX, mouseY);
    }

    public Vector2Int PlayerPosition()
    {
        return new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
    }

    //UI

    public virtual void CreateHover()
    {
        if (Description() == "") return;
        currHover = Instantiate(hoverUI, transform);
        currHover.GetComponent<Hover>().Init(transform, Name(), Description(), HealthUI());
    }

    public virtual void DestroyHover()
    {
        Destroy(currHover);
        currHover = null;
    }

    protected virtual string HealthUI()
    {
        return null;
    }

    public bool HoveringUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.name == "Outline") return false;
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    //Effects

    public void Add(Effect effect)
    {
        myEffects.Add(effect);
    }

    public List<Effect> GetEffects()
    {
        return new List<Effect>(myEffects);
    }

    public void Remove(Effect effect)
    {
        myEffects.Remove(effect);
    }

    private string EffectDescriptions()
    {
        string result = "";
        foreach(Effect effect in myEffects)
        {
            result += effect.Description();
        }
        return result;
    }

    //Getters and Setters

    public string Name()
    {
        return name;
    }

    public virtual string Description()
    {
        return description + EffectDescriptions();
    }

}

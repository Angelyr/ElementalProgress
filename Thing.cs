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

    protected virtual void Awake()
    {
        hoverUI = Resources.Load<GameObject>("Prefab/HoverUI");
        player = GameObject.Find("Player");
        UI = GameObject.Find("UI");
        myEffects = new List<Effect>();
    }

    public virtual string GetDescription()
    {
        return name;
    }

    public virtual void CreateHover()
    {
        if (GetDescription() == "") return;
        currHover = Instantiate(hoverUI, UI.transform);
        currHover.GetComponentInChildren<Text>().text = GetDescription();
        currHover.GetComponent<HoverUI>().SetTarget(transform);
    }

    public virtual void DestroyHover()
    {
        Destroy(currHover);
        currHover = null;
    }

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

    public void Add(Effect effect)
    {
        myEffects.Add(effect);
    }
}

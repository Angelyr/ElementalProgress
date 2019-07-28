using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Thing : MonoBehaviour
{
    private GameObject hoverUI;
    protected GameObject player;
    private GameObject currHover;
    private GameObject UI;

    protected virtual void Awake()
    {
        hoverUI = Resources.Load<GameObject>("Prefab/HoverUI");
        player = GameObject.Find("Player");
        UI = GameObject.Find("UI");
    }

    public abstract string GetDescription();

    public void CreateHover()
    {
        if (GetDescription() == "") return;
        currHover = Instantiate(hoverUI, UI.transform);
        currHover.GetComponentInChildren<Text>().text = GetDescription();
        currHover.transform.position = transform.position;
    }

    public void DestroyHover()
    {
        Destroy(currHover);
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

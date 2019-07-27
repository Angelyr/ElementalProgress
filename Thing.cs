using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thing : MonoBehaviour
{
    protected string myName = "Test";
    protected string description = "";
    private GameObject hoverUI;
    protected GameObject player;
    private GameObject currHover;

    protected virtual void Awake()
    {
        hoverUI = Resources.Load<GameObject>("Prefab/HoverUI");
        player = GameObject.Find("Player");
        SetDescription();
    }

    protected virtual void SetDescription()
    {
        description = myName + description;
    }

    private void SetHover()
    {
        currHover.GetComponent<Text>().text = description;
    }

    protected virtual void OnMouseEnter()
    {
        currHover = Instantiate(hoverUI, new Vector2(MousePosition().x, MousePosition().y), Quaternion.identity);
    }

    protected virtual void OnMouseExit()
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

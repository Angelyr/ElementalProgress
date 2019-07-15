using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnMouseExit()
    {
        removeHighlight();   
    }

}

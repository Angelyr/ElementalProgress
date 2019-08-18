using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GameObject messageUI;

    private void Awake()
    {
        messageUI = GameObject.Find("Message");
    }

    public void SetMessage(string msg)
    {
        messageUI.GetComponent<Text>().text = msg;
        Invoke("HideMsg", 2);
    }

    private void HideMsg()
    {
        messageUI.GetComponent<Text>().text = "";
    }

    public void EndTurnButton()
    {
        WorldController.SetDistanceFromPlayer();
        TurnOrder.EndTurn(gameObject);
        SetMessage("Enemy Turn"); 
    }
}

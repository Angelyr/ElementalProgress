using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public WorldGen world;

    private List<GameObject>[] inventorySlots = new List<GameObject>[11];
    private int selected = 0;

    private void Start()
    {
        for (int i = 0; i < inventorySlots.Length; i++) inventorySlots[i] = new List<GameObject>();
    }

    public void SelectSlot(int position)
    {
        Color selectedColor = transform.GetChild(selected).GetComponent<UnityEngine.UI.Image>().color;
        transform.GetChild(selected).GetComponent<UnityEngine.UI.Image>().color = transform.GetChild(position).GetComponent<UnityEngine.UI.Image>().color;
        selected = position;
        transform.GetChild(position).GetComponent<UnityEngine.UI.Image>().color = selectedColor;
    }

    public bool AddItem(GameObject pickUp)
    {
        for(int i=0; i<inventorySlots.Length; i++)
        {
            if(inventorySlots[i].Count > 0 && inventorySlots[i][0].GetComponent<SpriteRenderer>().sprite == pickUp.GetComponent<SpriteRenderer>().sprite)
            {
                inventorySlots[i].Add(pickUp);
                int amount = Convert.ToInt32(transform.GetChild(i).Find("Amount").GetComponent<UnityEngine.UI.Text>().text);
                amount += 1;
                transform.GetChild(i).Find("Amount").GetComponent<UnityEngine.UI.Text>().text = amount.ToString();
                return true;
            }
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].Count == 0)
            {
                inventorySlots[i].Add(pickUp);
                transform.GetChild(i).Find("Amount").GetComponent<UnityEngine.UI.Text>().text = "1";
                transform.GetChild(i).Find("Item").GetComponent<UnityEngine.UI.Image>().enabled = true;
                transform.GetChild(i).Find("Item").GetComponent<UnityEngine.UI.Image>().sprite = pickUp.GetComponent<SpriteRenderer>().sprite;
                return true;
            }
        }
        return false;
    }

    public GameObject GetItem()
    {
        if (inventorySlots[selected].Count > 0 && inventorySlots[selected].Count > 0) return inventorySlots[selected][0];
        return null;
    }

    public bool RemoveItem()
    {
        if (inventorySlots[selected].Count > 0)
        {
            inventorySlots[selected].RemoveAt(0);
            int amount = Convert.ToInt32(transform.GetChild(selected).Find("Amount").GetComponent<UnityEngine.UI.Text>().text);
            amount -= 1;
            transform.GetChild(selected).Find("Amount").GetComponent<UnityEngine.UI.Text>().text = amount.ToString();
            if (inventorySlots[selected].Count == 0) transform.GetChild(selected).Find("Item").GetComponent<UnityEngine.UI.Image>().enabled = false;
            return true;
        }
        return false;
    }
}

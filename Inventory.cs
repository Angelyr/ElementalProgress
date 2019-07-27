using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    private InventorySlot selected;

    private void Start()
    {
        selected = transform.GetChild(0).GetComponent<InventorySlot>();
        selected.Select();

        AddItem((GameObject)Resources.Load("Prefab/Laser"));
        AddItem((GameObject)Resources.Load("Prefab/Melee"));
        AddItem((GameObject)Resources.Load("Prefab/AOE"));
        AddItem((GameObject)Resources.Load("Prefab/Single"));
        AddItem((GameObject)Resources.Load("Prefab/Projectile"));
        AddItem((GameObject)Resources.Load("Prefab/Dash"));
    }

    public void SelectSlot(int position)
    {
        selected.DeSelect();
        selected = transform.GetChild(position).GetComponent<InventorySlot>();
        selected.Select();
    }

    public void AddItem(GameObject item)
    {
        foreach (InventorySlot slot in GetComponentsInChildren<InventorySlot>())
        {
            if(slot.GetItem() == null)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public GameObject RemoveItem()
    {
        return null;
    }

    public void UseSelected()
    {
        if (selected.GetItem() == null) return;
        selected.GetItem().GetComponent<Ability>().Use();
    }

    public GameObject GetSelected()
    {
        return selected.GetItem();
    }

    public void DecreaseCooldowns()
    {
        foreach(InventorySlot slot in GetComponentsInChildren<InventorySlot>())
        {
            slot.DecreaseCooldown();
        }
    }
}

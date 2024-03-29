﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class InventorySlot : UI
{
    private GameObject item;
    private bool selected;
    private Color selectedColor = new Color32(255, 237, 0, 100);
    private Color slotColor = new Color32(255, 255, 255, 100);
    private Image itemImage;
    private Text cooldown;

    //MonoBehavoir

    protected override void Awake()
    {
        base.Awake();
        itemImage = transform.Find("Item").GetComponent<Image>();
        cooldown = transform.Find("Cooldown").GetComponent<Text>();
        item = null;
        selected = false;
    }

    //Public

    public void DecreaseCooldown()
    {
        if (cooldown.text == "") return;
         
        int cooldownInt = Convert.ToInt32(cooldown.text);
        cooldown.text = "" + (cooldownInt - 1);
        if (cooldown.text == "0") cooldown.text = "";
    }

    public void Use()
    {
        if (item == null) return;
        if (item.GetComponent<Ability>() == null) return;
        if (cooldown.text != "") return;
        cooldown.text = "" + item.GetComponent<Ability>().CoolDown();
        item.GetComponent<Ability>().Use(MousePosition());
    }

    public void AddItem(GameObject newItem)
    {
        item = Instantiate(newItem, player.transform);
        item.SetActive(false);
        itemImage.sprite = newItem.GetComponent<SpriteRenderer>().sprite;
        itemImage.enabled = true;
    }

    public void Select()
    {
        selected = true;
        transform.GetComponent<Image>().color = selectedColor;
        if (item == null) return;
        item.GetComponent<Ability>().ShowRange();
    }

    public void DeSelect()
    {
        selected = false;
        transform.GetComponent<Image>().color = slotColor;
        if (item == null) return;
        item.GetComponent<Ability>().ClearHighlight();
    }

    public bool IsSelected()
    {
        return selected;
    }

    public GameObject GetItem()
    {
        return item;
    }

    public override string Description()
    {
        if (item == null) return "";
        return item.GetComponent<Thing>().Description();
    }

}

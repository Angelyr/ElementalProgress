using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hover : MonoBehaviour
{
    private Transform target;
    private const float bottomPadding = .1f;
    private const float middlePadding = .1f;
    private float height = 0;
    private int scale = 1;

    //MonoBehavior

    private void Start()
    {
        
    }

    private void Update()
    {
        AlignChildren();
        SetPosition();
    }

    //Private

    private void SetPadding()
    {
        //Set Padding based on if it is on world or ai
    }


    private void AlignChildren()
    {
        float height = 0;
        foreach(RectTransform child in transform)
        {
            if (child.gameObject.activeSelf == false) continue;
            if(scale == 1) child.anchoredPosition = new Vector3(0, height - child.rect.height/2, child.position.z);
            height -= (child.rect.height + middlePadding);
        }
        this.height = height;
    }

    private void SetPosition()
    {
        float targetHeight = 0;
        float myHeight = Mathf.Abs(height);

        if (target.GetComponent<Renderer>() != null)
        {
            targetHeight = target.GetComponent<Renderer>().bounds.size.y / 2;
        }
        else
        {
            targetHeight = target.GetComponent<RectTransform>().rect.height / 2;
            myHeight /= 2;
        }

        Vector2 targetPosition = target.position;
        
        targetPosition.y += targetHeight + myHeight + (bottomPadding);
        GetComponent<RectTransform>().position = targetPosition;
    }

    //Initialization

    private void SetTarget(Transform newTarget)
    {
        target = newTarget;
       
        if (target.GetComponent<RectTransform>() != null)
        {
            scale = 30;
        }

    }

    private void SetName(string name)
    {
        if (transform.Find("Name") == null) return;
        if (name == null)
        {
            transform.Find("Name").gameObject.SetActive(false);
            return;
        }
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;
    }

    private void SetDescription(string description)
    {
        if (description == null)
        {
            transform.Find("Description/Text").gameObject.SetActive(false);
            return;
        }
        transform.Find("Description/Text").GetComponent<TextMeshProUGUI>().text = description;
    }

    private void SetHealth(string health)
    {
        if (transform.Find("HealthBar") == null) return;
        if (health == null)
        {
            transform.Find("HealthBar").gameObject.SetActive(false);
            return;
        }
        transform.Find("HealthBar/Bar/Text").GetComponent<TextMeshProUGUI>().text = health;
    }

    //Public

    public void Init(Transform target, string name, string description, string health)
    {
        SetTarget(target);
        SetName(name);
        SetDescription(description);
        SetHealth(health);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hover : MonoBehaviour
{
    private Transform target;
    private const float bottomPadding = 20;
    private const float middlePadding = .1f;
    private float height = 0;

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

    private void AlignChildren()
    {
        float height = 0;
        foreach(RectTransform child in transform)
        {
            child.anchoredPosition = new Vector3(0, height - child.rect.height/2, child.position.z);
            height -= (child.rect.height + middlePadding);
        }
        this.height = height;
    }

    private void SetPosition()
    {
        float targetHeight = 0;
        if(target.GetComponent<Renderer>() != null) targetHeight = target.GetComponent<Renderer>().bounds.size.y / 2;
        else targetHeight = target.GetComponent<RectTransform>().rect.height / 2; 
        float myHeight = height;
        Vector2 targetPosition = target.position;
        targetPosition.y += targetHeight + myHeight + bottomPadding;
        transform.position = targetPosition;
    }

    //Initialization

    private void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void SetName(string name)
    {
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

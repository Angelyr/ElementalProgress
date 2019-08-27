using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float targetHeight = target.GetComponent<Renderer>().bounds.size.y / 2;
        float myHeight = height;
        Vector2 targetPosition = target.position;
        targetPosition.y += targetHeight + myHeight + bottomPadding;
        transform.position = targetPosition;
    }

    //Public

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetText()
    {

    }

}

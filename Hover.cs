using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    private Transform target;
    private const float bottomPadding = 20;
    private const float middlePadding = 1;

    private void Start()
    {
    }

    private void Update()
    {
        AlignChildren();

    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void AlignChildren()
    {
        float height = 0;
        foreach(RectTransform child in transform)
        {
            child.position = new Vector3(0, height - child.rect.height / 2, child.position.z);
            height -= child.rect.height + middlePadding;
            //Debug.Log(child.rect.height);
        }
    }

    private void SetPosition()
    {
        float targetHeight = target.GetComponent<RectTransform>().rect.height / 2;
        float myHeight = GetComponent<RectTransform>().rect.height / 2;
        Vector2 targetPosition = target.position;
        targetPosition.y += targetHeight + myHeight + bottomPadding;
        transform.position = targetPosition;
    }

    private int GetHeight()
    {
        return 0;
    }
}

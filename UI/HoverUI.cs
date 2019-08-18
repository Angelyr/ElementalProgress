using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverUI : MonoBehaviour
{
    private Transform target;
    private const float padding = 20;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        float targetHeight = target.GetComponent<RectTransform>().rect.height / 2;
        float myHeight = GetComponent<RectTransform>().rect.height / 2;
        Vector2 targetPosition = target.position;
        targetPosition.y += targetHeight + myHeight + padding;
        transform.position = targetPosition;
    }
}

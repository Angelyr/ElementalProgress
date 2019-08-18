using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Outline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.GetComponent<Entity>().OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.GetComponent<Entity>().OnMouseExit();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UI : Thing, IPointerEnterHandler, IPointerExitHandler
{
    protected override void Awake()
    {
        base.Awake();
        hoverUI = Resources.Load<GameObject>("Prefab/UIHover");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CreateHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyHover();
    }
}

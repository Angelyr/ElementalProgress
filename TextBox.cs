using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    private void Awake()
    {
        SetSize();
    }

    private void SetSize()
    {
        TextMesh text = GetComponentInChildren<TextMesh>();
        //GetComponent<RectTransform>().sizeDelta = new Vector2(text., text.preferredWidth);
    }
}

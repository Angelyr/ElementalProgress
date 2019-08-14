using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnUI : Thing, IPointerClickHandler
{
    private GameObject character;

    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        player.GetComponent<PlayerController>().myCamera.SetCameraFocus(character.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAoe : Ability
{
    public override List<GameObject> GetArea()
    {
        return WorldController.GetAll(MousePosition());
    }

    public override int GetRange()
    {
        return 7;
    }

    public override void Use()
    {
        foreach (GameObject tile in GetArea())
        {
            tile.GetComponent<Entity>().Attacked();
        }
    }
}

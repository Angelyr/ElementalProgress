using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private int damage;
    private int duration;
    private Color myColor = Color.red;

    private void Combinations()
    {

    }

    public void Apply(GameObject target)
    {
        if (target.GetComponent<Block>() != null)
        {
            SetColor(target, myColor);
            target.GetComponent<Thing>().Add(this);
        }
    }

    private void SetColor(GameObject target, Color color)
    {
        target.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().Attacked();
    }
}

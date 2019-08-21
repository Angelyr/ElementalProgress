using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected int damage;
    protected int duration;
    protected Color color;

    private void Combinations()
    {

    }

    public void Apply(GameObject target)
    {
        if (target.GetComponent<Block>() != null)
        {
            SetColor(target);
            target.GetComponent<Thing>().Add(this);
        }
    }

    private void SetColor(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = color;
    }

    public virtual void SteppedOn(GameObject target) { }
}

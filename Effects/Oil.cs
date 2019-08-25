using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Effect
{
    public Oil()
    {
        color = Color.black;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Character>().ChangeAP(-1);
    }

    protected override Effect Combinations(Effect effect)
    {
        if (effect is Fire) return new OilFire();
        return null;
    }
}

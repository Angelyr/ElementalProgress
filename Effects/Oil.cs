using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : Effect
{
    public Oil()
    {
        color = Color.black;
        duration = 3;
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

    protected override bool NegatesWith(Effect effect)
    {
        if (effect is OilFire) return true;
        return false;
    }

    protected override Effect SpreadOn(Effect effect)
    {
        if (effect is OilFire) return new OilFire();

        return null;
    }

    public override string Description()
    {
        return "Oil:" + "\n" + "Steals 1 AP" + "\n" + "Duration: " + duration;
    }
}

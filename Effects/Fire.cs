using UnityEngine;
using UnityEditor;

public class Fire : Effect
{
    public Fire()
    {
        color = Color.red;
    }

    protected override Effect Combinations(Effect effect)
    {
        if (effect is Oil) return new OilFire();
        return null;
    }

    protected override Effect SpreadOn(Effect effect)
    {
        if (effect is Oil) return new OilFire();
        return null;
    }

    protected override bool NegatesWith(Effect effect)
    {
        if (effect is OilFire) return true;
        return false;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().Attacked();
    }

    public override string Description()
    {
        return "Fire:" + "\n" + "Deals 1 damage";
    }
}
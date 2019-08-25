using UnityEngine;
using UnityEditor;

public class OilFire : Effect
{
    public OilFire()
    {
        color = Color.blue;
    }

    protected override Effect SpreadOn(Effect effect)
    {
        if(effect is Oil) return new OilFire();
        return null;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().Attacked();
    }
}
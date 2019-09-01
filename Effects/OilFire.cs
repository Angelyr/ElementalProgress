using UnityEngine;
using UnityEditor;

public class OilFire : Effect
{
    public OilFire()
    {
        color = Color.blue;
        duration = 3;
    }

    protected override Effect SpreadOn(Effect effect)
    {
        if(effect is Oil) return new OilFire();
        return null;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().ChangeHealth(-2);
    }

    public override string Description()
    {
        return "OilFire:" + "\n" + "Deals 2 damage" + "\n" + "Duration: " + duration;
    }
}
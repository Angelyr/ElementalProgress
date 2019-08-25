using UnityEngine;
using UnityEditor;

public class OilFire : Effect
{
    public OilFire()
    {
        color = Color.blue;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().Attacked();
    }
}
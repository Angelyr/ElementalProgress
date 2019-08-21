using UnityEngine;
using UnityEditor;

public class Fire : Effect
{
    public Fire()
    {
        color = Color.red;
    }

    public override void SteppedOn(GameObject target)
    {
        target.GetComponent<Entity>().Attacked();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected int damage;
    protected int duration;
    protected Color color;

    private bool Combine(GameObject target)
    {
        List<Effect> tileEffects = target.GetComponent<Thing>().GetEffects();
        if (tileEffects == null) return false;

        foreach(Effect tileEffect in tileEffects)
        {
            if ((tileEffect is Fire && this is Oil) || (tileEffect is Oil && this is Fire))
            {
                target.GetComponent<Thing>().Remove(tileEffect);
                Effect combination = new OilFire();
                combination.Apply(target);
                return true;
            }
        }
        
        return false;
    }

    private bool Spread(GameObject target)
    {

        if(this is OilFire)
        {
            Vector2Int[] adjacent = WorldController.GetAdjacent(target.GetComponent<Thing>().MyPosition());

            foreach(Vector2Int position in adjacent)
            {
                foreach(GameObject tile in WorldController.GetAll(position))
                {
                    foreach(Effect effect in tile.GetComponent<Thing>().GetEffects())
                    {
                        if(effect is Oil)
                        {
                            tile.GetComponent<Thing>().Remove(effect);
                            Effect combination = new OilFire();
                            combination.Apply(tile);
                        }
                    }
                }
            }
        }
        return false;
    }

    public void Apply(GameObject target)
    {
        if (target.GetComponent<Thing>() != null)
        {
            Spread(target);
            if (Combine(target)) return;
            
            SetColor(target);
            target.GetComponent<Thing>().Add(this);
        }
    }

    private void SetColor(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = color;
    }

    public virtual void SteppedOn(GameObject target) { }

    public virtual void StepTaken(GameObject target) { }
}

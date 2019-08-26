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
            Effect combination = Combinations(tileEffect);
            if (combination == null) continue;
            target.GetComponent<Thing>().Remove(tileEffect);    
            combination.Apply(target);
            return true;
            
        }
        
        return false;
    }

    private void Spread(GameObject target)
    {
        Vector2Int[] adjacent = WorldController.GetAdjacent(target.GetComponent<Thing>().MyPosition());
        foreach(Vector2Int position in adjacent)
        {
            foreach(GameObject tile in WorldController.GetAll(position))
            {
                foreach(Effect effect in tile.GetComponent<Thing>().GetEffects())
                {
                    Effect combination = SpreadOn(effect);
                    if (combination == null) continue;
                    tile.GetComponent<Thing>().Remove(effect);
                    combination.Apply(tile);
                }
            }
        }
    }

    private bool Stacks(GameObject target)
    {
        foreach(Effect effect in target.GetComponent<Thing>().GetEffects())
        {
            if (effect.GetType() == this.GetType()) return false;
            if (NegatesWith(effect)) return false;
        }
        return true;
    }

    private void SetColor(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = color;
    }

    //Virtual

    public virtual void SteppedOn(GameObject target) { }

    public virtual void StepTaken(GameObject target) { }

    protected virtual Effect Combinations(Effect effect)
    {
        return null;
    }

    protected virtual Effect SpreadOn(Effect effect)
    {
        return null;
    }

    protected virtual bool NegatesWith(Effect effect)
    {
        return false;
    }

    //Public

    public void Apply(GameObject target)
    {
        if (target.GetComponent<Thing>() != null && Stacks(target))
        {
            Spread(target);
            Combine(target);

            SetColor(target);
            target.GetComponent<Thing>().Add(this);
        }
    }
}

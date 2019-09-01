using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected int damage;
    protected int duration = 3;
    protected Color color;
    protected GameObject target;
    protected Color targetColor;

    //Private 

    private bool Combine(GameObject target)
    {
        List<Effect> tileEffects = target.GetComponent<Thing>().GetEffects();
        if (tileEffects == null) return false;

        foreach(Effect tileEffect in tileEffects)
        {
            Effect combination = Combinations(tileEffect);
            if (combination == null) continue;
            tileEffect.Remove(target);
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
                    effect.Remove(tile);
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

    public virtual string Description()
    {
        return "";
    }

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
        this.target = target;
        targetColor = target.GetComponent<SpriteRenderer>().color;
        WorldController.Add(this);
        if (target.GetComponent<Thing>() != null)
        {
            Spread(target);
            Combine(target);

            if (!Stacks(target)) return;

            SetColor(target);
            target.GetComponent<Thing>().Add(this);
        }
    }

    public void Remove(GameObject target)
    {
        if (target && target.GetComponent<SpriteRenderer>().color == color)
        {
            target.GetComponent<SpriteRenderer>().color = targetColor;
        }
        if(target) target.GetComponent<Thing>().Remove(this);
        WorldController.Remove(this);
    }

    public void Turn()
    {
        duration -= 1;
        if (duration < 1) Remove(target);
    }

    public Color TargetColor()
    {
        return targetColor;
    }

    public Effect Clone()
    {
        return (Effect) MemberwiseClone();
    }
}

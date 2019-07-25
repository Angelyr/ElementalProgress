using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override bool StartConcurrentTurn()
    {
        return true;
    }
}

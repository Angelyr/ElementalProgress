using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Laser : Ability
{

    protected GameObject laser;

    protected override void Awake()
    {
        base.Awake();
        laser = (GameObject)Resources.Load("Prefab/Laser", typeof(GameObject));
    }

    protected override void Init()
    {
        name = "Line";
        range = 5;
        cooldown = 5;
        description = "Hits every enemy in a line";
        targetType = "line";
        Add(new Fire());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public void StartTurn()
    {
        if (gameObject.name.Contains("Player")) gameObject.GetComponent<PlayerController>().StartTurn();
        if (gameObject.tag == "Enemy") gameObject.GetComponent<Enemy>().StartTurn();
    }
}

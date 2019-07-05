using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public WorldGen world;
    public Inventory inventory;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            inventory.UseSelected();
        }
    }
}

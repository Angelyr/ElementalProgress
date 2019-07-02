using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GameObject laser;

    private void FixedUpdate()
    {
        IfClick();
    }

    private void IfClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject attack = Instantiate(laser, transform);
            Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseLocation.x = mouseLocation.x - attack.transform.position.x;
            mouseLocation.y = mouseLocation.y - attack.transform.position.y;
            float angle = Mathf.Atan2(mouseLocation.y, mouseLocation.x) * Mathf.Rad2Deg;
            attack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            StartCoroutine(Delete(attack, .5f));
        }
    }

    IEnumerator Delete(GameObject attack, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(attack);
    }
}

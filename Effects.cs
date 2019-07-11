using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public void Animation()
    {
        StartCoroutine(Delete(gameObject, .5f));
    }

    IEnumerator Delete(GameObject attack, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(attack);
    }
}

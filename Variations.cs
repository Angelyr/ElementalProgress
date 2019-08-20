using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variations : MonoBehaviour
{
    public static Dictionary<Sprite, Sprite[]> variations;

    public Sprite[] grass;

    private static Sprite[] GetRest(Sprite[] sprites)
    {
        Sprite[] result = new Sprite[sprites.Length-1];
        for(int i=1; i<sprites.Length; i++)
        {
            result[i - 1] = sprites[i];
        }
        return result;
    }

    private void Awake()
    {
        variations = new Dictionary<Sprite, Sprite[]>();
        variations.Add(grass[0], GetRest(grass));
    }

    public static Sprite GetRandomVariation(Sprite key)
    {
        Sprite result = key;
        int length = 0;

        if (variations.ContainsKey(key))
        {
            length = variations[key].Length;
            result = variations[key][Random.Range(0, length)];
        }
        return result;
    }
}

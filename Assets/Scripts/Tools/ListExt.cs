using System.Collections.Generic;
using UnityEngine;

public static class ListExt 
{
    public static void Shuffle<T>(this List<T> target) 
    {
        var count = target.Count;
        for (var i = 0; i < count; i++) 
        {
            var r = Random.Range(i, count);
            var tmp = target[i];
            target[i] = target[r];
            target[r] = tmp;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtils
{
    public static void Randomize<T>(this List<T> list)
    {
        int count = list.Count;
        List<T> randomized = new List<T>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            randomized.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }

        list.AddRange(randomized);
    }
}

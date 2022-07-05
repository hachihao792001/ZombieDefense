using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public List<Transform> Allies;

    public float[] GetDistanceToAllies(Vector3 pos)
    {
        float[] distances = new float[Allies.Count];
        for (int i = 0; i < Allies.Count; i++)
        {
            distances[i] = Vector3.Distance(pos, Allies[i].position);
        }
        return distances;
    }
}

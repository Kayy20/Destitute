using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
public class PhysLayerFilter : ContextFilter
{

    public LayerMask mask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform t in original)
        {
            if (mask == (mask | (1 << t.gameObject.layer)))
            {
                CD.Log(CD.Programmers.BEN, "Adding Object: " + t.gameObject.name);
                filtered.Add(t);
            }
        }
        return filtered;
    }
}

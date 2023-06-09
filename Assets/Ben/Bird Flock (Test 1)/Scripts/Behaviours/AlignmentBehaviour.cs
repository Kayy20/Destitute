using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbours, maintain current alignment
        if (context.Count == 0) return agent.transform.forward;

        // Add all points and average
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform t in filteredContext)
        {
            alignmentMove += t.forward;
        }

        alignmentMove /= context.Count;

        return alignmentMove;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbours, return no adjustment
        if (context.Count == 0) return Vector3.zero;

        // Add all points and average
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform t in filteredContext)
        {
            if (Vector3.SqrMagnitude(t.position - agent.transform.position) < flock.SquareAdvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - t.position;
            }

        }

        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove;

    }
}

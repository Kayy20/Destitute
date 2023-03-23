using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set the agent's collider
        agentCollider = GetComponent<Collider>();
    }

    public void Move(Vector3 velo)
    {
        // Move towards the velo
        transform.forward = velo;
        // Constant Movement
        transform.position += velo * Time.deltaTime;
    }


}

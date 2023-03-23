using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{

    public BirdController controller;

    float noiseOffset;

    float additiveVelo = 0;

    float timer = 0;
    bool running;

    Rigidbody body;

    [SerializeField] Vector3 objectAdvoidance;
    [SerializeField] Vector3 alignment;
    [SerializeField] Vector3 cohesion;
    [SerializeField] Vector3 separation;
    [SerializeField] Collider[] nearbyObjects;
    [SerializeField] Collider[] nearbyBoids;

    Vector3 GetSeparationVector(Transform target, float dist)
    {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / dist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        noiseOffset = Random.value * 10.0f;
    }

    void Update()
    {
        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        // Current velocity randomized with noise.
        var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        var velo = controller.velocity * (1.0f + noise * controller.velocityVariation);

        // Initializes the vectors.
         separation = Vector3.zero;
         objectAdvoidance = Vector3.zero;
         alignment = transform.forward;
         cohesion = transform.position;

        // Looks up nearby boids.
         nearbyBoids = Physics.OverlapSphere(currentPosition, controller.neighbourDist, controller.birdLayer);
         nearbyObjects = Physics.OverlapSphere(currentPosition, controller.obsticalDist, controller.obsticalLayer);

        // Accumulates the vectors.
        foreach (var boid in nearbyBoids)
        {
            if (boid.gameObject == gameObject || boid.gameObject == GetComponentInChildren<Transform>().gameObject) continue;
            var t = boid.transform;
            separation += GetSeparationVector(t, controller.neighbourDist);
            alignment += t.forward;
            cohesion += t.position;
        }

        foreach (var c in nearbyObjects)
        {
            objectAdvoidance += GetSeparationVector(c.transform, controller.obsticalDist);
        }

        switch (controller.currentState)
        {
            case BirdController.birdState.Waiting:
                // Check if player coming into contact with collider of the soft area denial area then change to running.
                velo = 0;
                additiveVelo = 0;
                break;
            case BirdController.birdState.Running:
                // Be running for a bit, until they are a certain distance away from the original area, then select new place to land
                additiveVelo = 5f;
                timer += Time.deltaTime;
                break;
            case BirdController.birdState.Landing:
                // Slowing down the closer they get to the landing spot, then move to waiting
                break;

        }

        var avg = 1.0f / nearbyBoids.Length;
        alignment *= avg;
        cohesion *= avg;
        cohesion = (cohesion - currentPosition).normalized;

        // Calculates a rotation from the vectors.
        var direction = objectAdvoidance + separation + alignment + cohesion;
        var rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);

        // Applys the rotation with interpolation.

        if (rotation != currentRotation)
        {
            var ip = Mathf.Exp(-controller.rotationCoeff * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
        }

        
                

        transform.position = currentPosition + transform.forward * ((velo + additiveVelo) * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            controller.currentState = BirdController.birdState.Running;
            timer = 0;
            running = true;
        }
    }

}

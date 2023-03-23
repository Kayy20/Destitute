using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    public GameObject birdPref;
    public int spawnCount = 50;
    public float spawnRadius = 4.0f;
    [Range(0f, 20f)] public float velocity = 6f;
    [Range(0f, 0.9f)] public float velocityVariation = 0.5f;
    [Range(0f, 20f)] public float rotationCoeff = 4.0f;
    [Range(0f, 10f)] public float neighbourDist = 2.0f;
    [Range(0f, 10f)] public float obsticalDist = 2.0f;
    [Range(0f, 5f)] public float playerDist = 2.0f;
    public LayerMask birdLayer;
    public LayerMask obsticalLayer;
    public LayerMask playerLayer;
    public birdState currentState = birdState.Waiting;

    public GameObject[] targetLocations;

    public enum birdState
    {
        Waiting,
        Running,
        Landing
    }

    // Start is called before the first frame update
    void Start()
    {
        int location = Random.Range(0, targetLocations.Length);
        for (int i = 0; i < spawnCount; i++) Spawn(i, location);
    }

    public void Spawn(int i, int location)
    {
        Spawn(targetLocations[location].transform.position + new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * spawnRadius, i);
    }

    public void Spawn(Vector3 position, int i)
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        GameObject bird = Instantiate(birdPref, position, rotation);
        bird.name = $"Bird {i}";
        bird.GetComponent<BirdBehaviour>().controller = this;
        //return bird.GetComponent<BirdBehaviour>();
    }

    Vector3 GetSeparationVector(Transform target, Transform current)
    {
        var diff = current.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / neighbourDist);
        return diff * (scaler / diffLen);
    }

}

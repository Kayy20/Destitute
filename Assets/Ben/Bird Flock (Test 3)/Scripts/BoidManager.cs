using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    public static BoidManager Instance;

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;

    public List<Transform> targetLocations = new List<Transform>();

    bool newTargetSelected = false;

    Boid[] boids;

    public Action alerted;

    public AudioSoundSO sound;

    void Start () {

        Instance = this;

        boids = FindObjectsOfType<Boid> ();
        int count = 0;
        foreach (Boid b in boids) {
            b.Initialize (settings, null, this);
            b.gameObject.name = $"Boid {count}";
            count++;
        }

        PopulateTargetLocations();

    }

    void PopulateTargetLocations()
    {
        BoxCollider[] obj = FindObjectsOfType<BoxCollider>();
        for (int i = 0; i < obj.Length; i++)
            if (obj[i].gameObject.CompareTag("OffLimitLocation"))
            {
                targetLocations.Add(obj[i].gameObject.transform);
            }
    }

    public void CleanUp()
    {
        for (int i = 0; i < boids.Length; i++)
        {
            Destroy(boids[i].gameObject);
        }
    }

    void Update () {
        if (boids != null) {

            int numBoids = boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++) {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
            }

            var boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
            boidBuffer.SetData (boidData);

            compute.SetBuffer (0, "boids", boidBuffer);
            compute.SetInt ("numBoids", boids.Length);
            compute.SetFloat ("viewRadius", settings.perceptionRadius);
            compute.SetFloat ("avoidRadius", settings.avoidanceRadius);

            int threadGroups = Mathf.CeilToInt (numBoids / (float) threadGroupSize);
            compute.Dispatch (0, threadGroups, 1, 1);

            boidBuffer.GetData (boidData);

            for (int i = 0; i < boids.Length; i++) {
                boids[i].avgFlockHeading = boidData[i].flockHeading;
                boids[i].centreOfFlockmates = boidData[i].flockCentre;
                boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

                boids[i].UpdateBoid ();
            }

            boidBuffer.Release ();
        }
    }


    public void SelectNewTarget() // Should only be called once
    {
        if (!newTargetSelected)
        {
            newTargetSelected = true;
            int loc = UnityEngine.Random.Range(0, targetLocations.Count);
            for (int i = 0; i < boids.Length; i++)
            {
                // Set new target
                boids[i].SetTarget(targetLocations[loc]);
            }   
        }
    }

    public void RunFromLocation(Transform t)
    {
        newTargetSelected = false;

        alerted?.Invoke();
        Debug.Log("Birds Alerted");

        for (int i = 0; i < boids.Length; i++)
        {
            // Set new target
            boids[i].targetLocation = new Vector3(t.position.x, boids[i].transform.position.y - 5, t.position.z);
            boids[i].transform.position = new Vector3(boids[i].position.x, boids[i].position.y + 0.5f, boids[i].position.z);
            boids[i].SetTarget(null);
            boids[i].boidSetting = boidSetting.Running;
        }
    }
    

    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int);
            }
        }
    }
}
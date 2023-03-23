using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    BoidSettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;

    [HideInInspector]
    public Vector3 targetLocation;

    //[HideInInspector]
    public boidSetting boidSetting;

    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;
    BoidManager manager;

    float runTimer;
    public float maxRunTimer = 5.0f;
    public float maxVibeTimer = 10.0f;

    public float minGroundDist = 0.5f;

    public Collider waitingCollider;
    public Collider reLocatingCollider;


    void Awake () {
        material = transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
        cachedTransform = transform;
    }

    public void Initialize (BoidSettings settings, Transform target, BoidManager manager) {
        this.target = target;
        this.settings = settings;
        this.manager = manager;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void SetColour (Color col) {
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateBoid () {
        Vector3 acceleration = Vector3.zero;
        try
        {
            if (boidSetting == boidSetting.Waiting)
            {

                velocity = acceleration;
            }
            else if (boidSetting == boidSetting.DescentToGround)
            {
                // Move closer to the ground until they hit the ground, then stop moving

                // Draw ray downwards and see if the length between current pos is within a certain threshold
                if (position.y - minGroundDist > target.position.y)
                {
                    // Move Down
                    velocity += Vector3.down * Time.deltaTime;
                    float speed = velocity.magnitude;
                    Vector3 dir = velocity / speed;
                    speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                    velocity = dir * speed;

                    cachedTransform.position += velocity * Time.deltaTime;
                    position = cachedTransform.position;
                }
                else
                {

                    // Stop moving down and go into waiting
                    //CD.Log($"{gameObject.name} dist: {hit.distance}");
                    boidSetting = boidSetting.Waiting;
                    waitingCollider.enabled = true;
                    reLocatingCollider.enabled = false;
                }
            }
            else
            {
                if (target != null)
                {
                    Vector3 offsetToTarget = (target.position - position);
                    acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
                }

                if (numPerceivedFlockmates != 0)
                {
                    centreOfFlockmates /= numPerceivedFlockmates;

                    Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                    var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                    var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                    var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;


                    acceleration += alignmentForce;
                    acceleration += cohesionForce;
                    acceleration += seperationForce;

                }
                if (boidSetting == boidSetting.Running)
                {
                    var runForce = SteerTowards(position - targetLocation) * settings.runWeight;
                    acceleration += runForce;

                    runTimer += Time.deltaTime;

                    if (runTimer >= maxRunTimer)
                    {
                        runTimer = 0;
                        boidSetting = boidSetting.None;
                    }
                }
                if (boidSetting == boidSetting.None) // Let them vibe for some time before selecting a new space
                {
                    runTimer += Time.deltaTime;

                    if (runTimer >= maxVibeTimer)
                    {
                        runTimer = 0;
                        boidSetting = boidSetting.Relocating;

                        waitingCollider.enabled = false;
                        reLocatingCollider.enabled = true;
                        manager.SelectNewTarget();
                    }
                }

                if (IsHeadingForCollision())
                {
                    Vector3 collisionAvoidDir = ObstacleRays();
                    Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce;
                }

                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                velocity = dir * speed;

                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
            }

        }
        catch { }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays() {
        Vector3[] rayDirections = BoidHelper.directions;
        //try
        //{
            for (int i = 0; i < rayDirections.Length; i++) {
                Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
                Ray ray = new Ray(position, dir);
                if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) {
                    return dir;
                }
            } 
        //}
        //catch { }

        return forward;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Player enters, mark player mark then signal to other boids to run from that spot (ideally away from the ground)
        if (boidSetting == boidSetting.Waiting)
        {
            if (other.CompareTag("Player"))
            {
                //CD.Log(CD.Programmers.BEN, $"{gameObject.name} Player Found While In Waiting By ");
                manager.RunFromLocation(other.transform);
                // Do sound
                SoundController.PlaySound(manager.sound, transform);
                //targetLocation = new Vector3(other.transform.position.x, transform.position.y - 5, other.transform.position.z);
                //transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
                //SetTarget(null);
                //boidSetting = boidSetting.Running;
                runTimer = 0;
            }

        }
        else if (boidSetting == boidSetting.Relocating)
        {
            if (other.gameObject.CompareTag("OffLimitLocation"))
            {
                
                boidSetting = boidSetting.DescentToGround;
                velocity = Vector3.zero;
                //CD.Log(CD.Programmers.BEN, $"{gameObject.name} OffLimitLocation Entered By ");
            }
        }

    }



}

public enum boidSetting
{
    None, // Vibing in the air till time to select new target location
    Waiting, // Wait till the player hits their collider
    Running, // Run from the player
    Relocating, // Will move towards the target Locadtion
    DescentToGround // Will stop moving and only move downwards till they touch ground
}
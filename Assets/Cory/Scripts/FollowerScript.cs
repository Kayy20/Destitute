using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerScript : MonoBehaviour
{
    Transform player;
    float Sanity;
    float distance;
    public bool isHunting;
    public float huntTimer = 300;
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private float timer;
    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav.enabled = true;
        nav.speed = 10;
        isHunting = false;
        StartCoroutine("startTimer");
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        timer = 0;
        wanderTimer = 4f;
        wanderRadius = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // FOLLOWER IS SIMPLY WANDERING AROUND THE MAP

        if (timer >= wanderTimer && isHunting == false)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            nav.SetDestination(newPos);
            timer = 0;
        }

        // FOLLOWER IS NOW HUNTING

        if (isHunting == true)
        {
            transform.LookAt(player);
            nav.SetDestination(player.position);
        }


        // SANITY FEATURE. IF PLAYER IS UNDER 10 SANITY START THE HUNT.

        /*Sanity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().PlayerSanity;
            if (Sanity <= 10)
            {
                isHunting = true;
            } else
            {
                isHunting = false;
            }
        */

        // IF PLAYER IS OVER A CERTAIN DISTANCE AWAY START TO MOVE CLOSER.
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance >= 30)
        {
            transform.LookAt(player);
            nav.SetDestination(player.position);
            timer = 3f;
        }
        else
        {
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(1f);
        if (huntTimer > 0)
        {
            huntTimer = huntTimer - 1;
            StartCoroutine("startTimer");
        }
        else
        {
        }
    }
}
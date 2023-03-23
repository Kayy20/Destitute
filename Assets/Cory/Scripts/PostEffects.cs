using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PostEffects : MonoBehaviour
{

    public Volume v;
    private float weight;
    public GameObject follower;
    public GameObject player;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        v.weight = 0;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, follower.transform.position);
        if (distance > 20)
        {
            v.weight = 0;
        } else {

            v.weight = 1 - (distance / 20);
        }
    }
}

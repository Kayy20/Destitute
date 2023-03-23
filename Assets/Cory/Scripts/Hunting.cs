using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunting : MonoBehaviour
{

    public Transform player;
    private RaycastHit hit;
    private int layerMask = 1 << 5;
    public float huntLengthTimer = 0f;
    private bool startedHunt = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        layerMask = ~layerMask;
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.transform;
            if (Physics.Raycast(GameObject.Find("Follower2").transform.position, (player.position - GameObject.Find("Follower2").transform.position), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform == player && GameObject.Find("Follower2").GetComponent<FollowerObjScript>().huntTimer == 0)
                {
                    Debug.Log("Hunting");
                    GameObject.Find("Follower2").GetComponent<FollowerObjScript>().state = FollowerObjScript.State.Chasing;

                    if (startedHunt == false)
                    {
                        StartCoroutine("huntTimerStart");
                        startedHunt = true;
                    }
                }
            }
        }
    }

    IEnumerator huntTimerStart()
    {
        yield return new WaitForSeconds(1f);
        huntLengthTimer += 1;
        if (huntLengthTimer % 10 == 0)
        {
            Debug.Log("Speed Increased");
            FollowerObjScript.Speed += 1;
            //t.text = "Hunt Timer: " + huntTimer;
        }
        StartCoroutine("huntTimerStart");
    }
}

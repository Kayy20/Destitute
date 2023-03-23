using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{

    public float MapTimer = 300f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("startTimer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(1f);
        if (MapTimer > 0)
        {
            MapTimer = MapTimer - 1;
            StartCoroutine("startTimer");
        }
        else
        {

        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSceneMovement : MonoBehaviour
{
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;

    [SerializeField] private float speed;

    private Vector3 destPos;
    private int goingIndex = 0;

    private void Start()
    {
        destPos = pos1.position;
    }

    private void Update()
    {
        this.transform.position = Vector3.Slerp(transform.position, destPos, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, destPos) < 2)
        {
            if (goingIndex == 0)
            {
                //go to 2
                goingIndex = 1;
                destPos = pos2.transform.position;
                
            }
            else
            {
                //go to 1
                goingIndex = 0;
                destPos = pos1.transform.position;
            }
            
            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.y *= -1;
            transform.rotation = Quaternion.Euler(newRot);
        }
    }
}

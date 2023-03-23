using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClipCamera : MonoBehaviour
{

    Camera cam;
    Vector3 dir;
    Vector2 lookRot=Vector2.zero;

    [SerializeField]
    float sensetivity = 3;
    [SerializeField]
    float speed=5;
    [SerializeField]
    string forwardAxis, rightAxis, upAxis, speedAxis;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        cam =GetComponent<Camera>();
    }

    private void Update()
    {
        dir = transform.forward * Input.GetAxis(forwardAxis);
        dir +=transform.right * Input.GetAxis(rightAxis);
        dir +=transform.up * Input.GetAxis(upAxis);

        speed+=Input.GetAxis(speedAxis)*Time.deltaTime*10;

        lookRot.x -= Input.GetAxis("Mouse Y");
        lookRot.y += Input.GetAxis("Mouse X");
    }


    private void FixedUpdate()
    {

        transform.position += dir * speed * Time.fixedDeltaTime;

        transform.localRotation = Quaternion.Euler(lookRot.x * sensetivity*Time.fixedDeltaTime, lookRot.y * sensetivity*Time.fixedDeltaTime, 0);
    }
}
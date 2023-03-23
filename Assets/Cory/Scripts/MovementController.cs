using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController Instance;
    [SerializeField] private AudioSoundSO walkingSound;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    //public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float stamina;
    private Vector3 lastPosition;
    private bool soundON = false;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public Vector3 Position { get { return transform.position; } }

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        stamina = 500f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lastPosition = transform.position;

    }

    void Update()
    {
        
        if (PauseManager.instance != null)
        {
            if(PauseManager.instance.isPaused)
                return;
        }
        
       // SoundController.PlaySound(walkingSound, this.transform);
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX;
        //Debug.Log(soundON);
        if (lastPosition != transform.position && soundON == false)
        {
            SoundController.PlaySound(walkingSound, this.transform);
            soundON = true;
        } else if (soundON == true && lastPosition == transform.position)
        {
            SoundController.StopSound(walkingSound);
            soundON = false;
        }
        lastPosition = transform.position;

        if (canMove == true)
        {
            if (isRunning == true && stamina > 0)
            {
                stamina -= 1;
                curSpeedX = runningSpeed * Input.GetAxis("Vertical");
            }
            else
            {
                if (stamina < 500)
                {
                    stamina += 1;
                }
                curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
            }
        } else
        {
            if (stamina < 500)
            {
                stamina += 2;
            }
            curSpeedX = 0;
        }

        float curSpeedY;

        if (canMove == true)
        {
            if (isRunning == true && stamina > 0)
            {
                curSpeedY = runningSpeed * Input.GetAxis("Horizontal");
            }
            else
            {
                curSpeedY = walkingSpeed * Input.GetAxis("Horizontal");
            }
        }
        else
        {
            curSpeedY = 0;
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        // JUMPING REMOVED
        
        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            //moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        
        // Gravity Baby
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void enableInput(bool value)
    {
        if (value == false)
        {
            canMove = false;
        }else
        {
            canMove = true;
        }
    }

}
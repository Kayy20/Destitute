using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FollowerObjScript : MonoBehaviour
{

    public static float Speed = 4;
    private Vector3 wayPoint;
    private float Range = 10;
    private float HuntDistance = 10;
    public State state;
    private Rigidbody rb;
    public Transform player;
    private float distance;
    private bool inObject;
    public float maxHuntTimer = 60;
    public float huntTimer = 60;
    public Text t;
    public float huntLengthTimer = 0f;
    public float maxDistanceTimer = 0f;
    private bool startedHunt = false;
    //public float fogDensity = 0f;
    public Animator animator;
    public GameObject birdManager;
    public float rotationSpeed = 90;
    private Quaternion targetRotation;
    private Quaternion newRotation;



    [SerializeField]
    RectTransform timerImg;

    [SerializeField] private AudioSoundSO idleSound;
    [SerializeField] private AudioSoundSO growl;
    [SerializeField] private AudioSoundSO ambient;
    
    //CORY I PUT THERE HERE FOR TESTING
    private bool canKill = true;


    public enum State
    {
        Hunting,
        Chasing,
    }
    // Start is called before the first frame update
    void Start()
    {
        birdManager.GetComponent<BoidManager>().alerted += BirdAlerted;
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(6, 8);
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(8, 7);
        inObject = false;
        wayPoint = new Vector3(this.transform.position.x + Random.Range(-10.0f, 10.0f), this.transform.position.y, this.transform.position.z + Random.Range(-10.0f, 10.0f));
        transform.LookAt(wayPoint);
        Speed = Mathf.Round(Speed / 2);
        StartCoroutine("startTimer");
    }

    void Awake()
    {
        state = State.Hunting;

        huntTimer = maxHuntTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Speed);
        if (PauseManager.instance != null)
        {
            if(PauseManager.instance.isPaused)
                return;
        }

        if (transform.position.y < (float)1.1 || transform.position.y > (float)1.1)
        {
            transform.position = new Vector3(transform.position.x, (float)1.1, transform.position.z);
        }
        switch (state)
        {
            default:
            case State.Hunting:
                if (startedHunt == false)
                {
                    StartCoroutine("maxDistance");
                    startedHunt = true;
                }

                transform.position += transform.forward * Speed * Time.deltaTime;
                distance = Vector3.Distance(player.transform.position, this.transform.position);
                if (distance > 40)
                {
                    wayPoint = player.transform.position;
                }

                if (HuntDistance > distance)
                {
                    animator.SetBool("Hunting", true);
                    state = State.Chasing;
                }
                if (Vector3.Distance(wayPoint, transform.position) < 2)
                {
                    wayPoint = new Vector3(this.transform.position.x + Random.Range(-15.0f, 15.0f), this.transform.position.y, this.transform.position.z + Random.Range(-15.0f, 15.0f));
                }

                targetRotation = Quaternion.LookRotation(wayPoint - transform.position, Vector3.up);
                newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = newRotation;
                break;
            
            
            
            case State.Chasing:
                transform.LookAt(player);
                transform.position += transform.forward * Speed * Time.deltaTime;
                break;


        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canKill)
        {

            //SceneManager.LoadScene("Title Screen");
            // Reset stuff
            BoidManager.Instance.CleanUp(); // Destroy the boids (birds)
            MapGeneration.Instance.ReturnToMap("TestBox");
            // ----- //
            EndScreen.Instance.ShowEndScreen(CauseofDeath.Follower);
        }
        if (inObject == false && other.tag != "Player" && other.tag != "viewDistance" && other.tag != "OffLimitLocation" && Speed > 3)
        {
            inObject = true;
            Speed = Speed - 3;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (inObject == true && Speed < 3)
        {
            inObject = false;
            Speed = Speed + 3;

        }
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(1f);
        if (huntTimer > 0)
        {
            huntTimer = huntTimer - 1;
            timerImg.rotation = Quaternion.Euler(0, 0, 90 + (1 - (huntTimer / maxHuntTimer)) * 270);
            StartCoroutine("startTimer");
        }
    }

    IEnumerator huntTimerStart()
    {
        yield return new WaitForSeconds(1f);
        huntLengthTimer += 1;
        if (huntLengthTimer % 10 == 0)
        {
            Debug.Log("Speed Increased");
            Speed += 1;
        }
        StartCoroutine("huntTimerStart");
    }

    IEnumerator maxDistance()
    {
        yield return new WaitForSeconds(1f);
        maxDistanceTimer += 1;
        if (maxDistanceTimer % 10 == 0)
        {
            Debug.Log("Distance Increased");
            HuntDistance += 5;
        }
        StartCoroutine("maxDistance");
    }

    public void BirdAlerted()
    {
        if (huntTimer > 5)
        {
            huntTimer = huntTimer - 5;
        } else
        {
            huntTimer = 0;
        }
    }
}


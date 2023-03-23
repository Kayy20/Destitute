using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leave3D : MonoBehaviour
{
    public GameObject[] collider = new GameObject[12];
    public Light[] spotLight = new Light[12];
    public GameObject[] exitParticle = new GameObject[12];
    public GameObject follower;


    static public int currLoop = 1;
    static public int currLevelNumber = 0;

    bool chosen = false;

    private int randomSide;

    public void Start()
    {
        for (int i = 0; i <= 11; i++)
        {
            spotLight[i].intensity = 0;
            collider[i].GetComponent<Collider>().isTrigger = false;
            //exitParticle[i].GetComponent<ParticleSystem>().Stop();
            exitParticle[i].SetActive(false);
        }

    }

    public void Update()
    {
        if (follower.GetComponent<FollowerObjScript>().huntTimer == follower.GetComponent<FollowerObjScript>().maxHuntTimer - 5)
        {
            for (int i = 0; i <= 11; i++)
            {
                spotLight[i].intensity = 40;
                collider[i].GetComponent<Collider>().isTrigger = true;
                exitParticle[i].SetActive(true);
            }

        }
        if (chosen == false && follower.GetComponent<FollowerObjScript>().huntTimer == follower.GetComponent<FollowerObjScript>().maxHuntTimer - 6)
        {
            chosen = true;
            for (int i = 0; i < currLoop; i++)
            {
                Debug.Log("Picking Sides");
               randomSide = (int)Random.Range(0f, 11f);
               pickSide();
            }
        }
    }

    public void pickSide()
    {
        Debug.Log("PICKED A SIDE FOR ONCE");
        if (spotLight[randomSide].intensity == 40)
        {
            //randomSide = (int)Random.Range(0f, 11f);
            //pickSide();
            spotLight[randomSide].intensity = 0;
            collider[randomSide].GetComponent<Collider>().isTrigger = false;
            exitParticle[randomSide].SetActive(false);
        }
        //else
        //{
        //    spotLight[randomSide].intensity = 0;
        //    collider[randomSide].GetComponent<Collider>().isTrigger = false;
        //    exitParticle[randomSide].SetActive(false);
        //}
    }
}

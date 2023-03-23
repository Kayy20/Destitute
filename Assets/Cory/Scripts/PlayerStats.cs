using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    // TODO - How are we counting this?
    // Integers? 0 - 100? 0 - 10?
    // Percent?
    public float PlayerSanity;
    public float PlayerHealth;
    public float PlayerStamina;
    public float PlayerHunger;
    public float PlayerHydration;
    public int PlayerMedical;
    public int PlayerSurvival;
    public int PlayerFood; // Dunno if this is the same as Hunger
    public int PlayerWater; // Dunno if this is the same as Hydration
    public int PlayerWounds;

    private static PlayerStats instance;
    public static PlayerStats Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = new PlayerStats();
            }
            return instance; 
        } 
    }

    // Start is called before the first frame update
/*
    void Start()
    {
        StartCoroutine("decreaseSanity");
        PlayerSanity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator decreaseSanity()
    {
        yield return new WaitForSeconds(1.5f);
        if (PlayerSanity >= 0)
        {
            PlayerSanity = PlayerSanity - 1;
        }
        else {

        }
        StartCoroutine("decreaseSanity");
    }
*/
}

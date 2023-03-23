using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClassVariables
{
    public static int Wounds = 0;
    public static int score = 0;
    public static int highScore = 0;
}

public enum CauseofDeath {

    Wounds,
    Hunger,
    Thirst,
    Follower

}

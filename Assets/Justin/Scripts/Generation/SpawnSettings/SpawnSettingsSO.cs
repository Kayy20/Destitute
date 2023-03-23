using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSettingsSO : ScriptableObject
{

    [Range(0.0f,1.0f)]
    public float maxSpawnRate=0.5f;
    public float ValuableChance=0.05f;


}

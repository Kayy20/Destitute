using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSelector : MonoBehaviour
{
    
    GameObject[] layouts;
    [HideInInspector]
    public bool debugMode = false;
    [HideInInspector]
    public GameObject debugLayout;

    private void Awake()
    {
        layouts = new GameObject[transform.childCount];

        for(int i = 0; i < layouts.Length; i++)
        {
            layouts[i] = transform.GetChild(i).gameObject;
            layouts[i].SetActive(false);
        }
    }

    private void Start()
    {
        if (debugMode)
        {
            debugLayout.SetActive(true);
        }
        else
        {
            layouts[Random.Range(0, layouts.Length)].SetActive(true);
        }
    }

}

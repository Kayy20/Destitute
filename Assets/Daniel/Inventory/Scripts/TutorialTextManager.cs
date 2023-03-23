using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextManager : MonoBehaviour
{
    public static TutorialTextManager Instance;
    
    [SerializeField] private Text text;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayText(string s)
    {
        StopCoroutine(TextTimer());
        text.text = s;
        text.gameObject.SetActive(true);
        StartCoroutine(TextTimer());
    }


    IEnumerator TextTimer()
    {
        yield return new WaitForSeconds(20f);
        text.gameObject.SetActive(false);
    }

}

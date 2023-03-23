using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{


    private void OnLevelWasLoaded(int level)
    {
        StaticClassVariables.Wounds = 0;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void LoadScene(string scenename)
    {
        StartCoroutine(fadeIn(scenename));
        
    }

    private IEnumerator fadeIn(string scenename)
    {
        TransitionOpen.fade = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scenename);
    }

}

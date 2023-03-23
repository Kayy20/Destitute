using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    
    public bool isPaused = false;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InventoryController.Instance.IsShowing)
        {
            TogglePause();
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && InventoryController.Instance.IsShowing)
        {
            InventoryController.Instance.ToggleUI(!InventoryController.Instance.IsShowing);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
        
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }
    
    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

}

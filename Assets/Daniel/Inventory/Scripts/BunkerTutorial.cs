using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BunkerTutorial : MonoBehaviour
{
    public static BunkerTutorial Instance;
    
    private bool hasCompleted = false;

    private float timePerTip = 8f;

    public UnityEvent CompletedEvent;

    private bool movement;
    private bool looking;
    private bool pickup;
    private bool inventory;
    private bool rotateItem;
    private bool dropItem;

    private bool completed;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!BunkerTutorialFlag.instance.hasCompleted)
        {
            StartCoroutine(StartSequence());
        }
        
    }

    private IEnumerator StartSequence()
    {
        BunkerTutorialFlag.instance.hasCompleted = true;
        
        //yield return new WaitForSeconds(5f);
        if (!looking && !movement)
        {
            DisplayLookMoveText();
        }
        /*
        yield return new WaitForSeconds(5f);
        if (!movement)
        {
            DisplayMovementText();
        }
        */
        
        yield return new WaitForSeconds(timePerTip);
        if (!pickup)
        {
            DisplayPickUpText();
        }

        yield return new WaitForSeconds(timePerTip);
        if (!inventory)
        {
            DisplayInventoryText();
        }
        
        yield return new WaitForSeconds(timePerTip);
        
        DisplayRotationText();
        
        yield return new WaitForSeconds(timePerTip);
        
        DisplayDropText();
        
    }

    private void Update()
    {
        if (completed)
            return;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            movement = true;
        }
        
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            looking = true;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory = true;
        }
        
        CheckForCompletion();
    }

    public void DisplayMovementText()
    {
        TutorialTextManager.Instance.DisplayText("Use WASD to Move");
    }

    public void DisplayLookText()
    {
        TutorialTextManager.Instance.DisplayText("Move your mouse to Look");
    }

    public void DisplayLookMoveText()
    {
        TutorialTextManager.Instance.DisplayText("Move your mouse to Look and WASD to Move");
    }

    public void DisplayPickUpText()
    {
        TutorialTextManager.Instance.DisplayText("While looking at an Item, press E to pick it up");
    }
    
    public void DisplayInventoryText()
    {
        TutorialTextManager.Instance.DisplayText("Press Tab to open your Inventory");
    }
    
    public void DisplayRotationText()
    {
        TutorialTextManager.Instance.DisplayText("Press R while holding an item to rotate it");
    }
    
    public void DisplayDropText()
    {
        TutorialTextManager.Instance.DisplayText("Press X while hovering over an item to drop it");
    }

    public void PickUpItem()
    {
        pickup = true;
    }

    private void CheckForCompletion()
    {
        if (movement && looking && pickup)
        {
            CompletedTutorial();
        }
    }

    public void CompletedTutorial()
    {
        StopCoroutine(StartSequence());
        completed = true;
        
        Debug.Log("COMPLETED TUTORIAL");
        CompletedEvent?.Invoke();
    }
}

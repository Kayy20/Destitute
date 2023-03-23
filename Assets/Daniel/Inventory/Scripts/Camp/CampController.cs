using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampController : MonoBehaviour
{
    public static CampController Instance;

    public Action SelectedFood;
    public Action SelectedMed;
    public Action SelectedSurv;
    public Action SelectedWater;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //InventoryController.Instance.ToggleCampUI(false);
    }

    public void SelectedItem(ItemSO item)
    {
        switch (item.resType)
        {
            case AnswerType.ResourceType.Water:
                SelectedWater?.Invoke();
                break;
            case AnswerType.ResourceType.Food:
                SelectedFood?.Invoke();
                break;
            case AnswerType.ResourceType.Meat:
                SelectedFood?.Invoke();
                break;
            case AnswerType.ResourceType.Medical:
                SelectedMed?.Invoke();
                break;
            case AnswerType.ResourceType.Survival:
                SelectedSurv?.Invoke();
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    public ItemSO ItemData => item;


    private void Start()
    {
        Outline[] os;

        //set outlines
        switch (item.resType)
        {

            case AnswerType.ResourceType.Food:
            case AnswerType.ResourceType.Water:
            case AnswerType.ResourceType.Meat:
                os = GetComponentsInChildren<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.foodColour);

                os = GetComponents<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.foodColour);
                break;

            case AnswerType.ResourceType.Medical:
                os = GetComponentsInChildren<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.medColour);

                os = GetComponents<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.medColour);
                break;

            case AnswerType.ResourceType.Survival:
            case AnswerType.ResourceType.SharpEdges:
            case AnswerType.ResourceType.GunStuff:
                os = GetComponentsInChildren<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.survColour);

                os = GetComponents<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.survColour);
                break;

            case AnswerType.ResourceType.Valuable:
                os = GetComponentsInChildren<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.valColour);

                os = GetComponents<Outline>();

                for (int i = 0; i < os.Length; i++)
                    os[i].SetColour(ItemManager.Instance.valColour);
                break;

        }
        
    }
}

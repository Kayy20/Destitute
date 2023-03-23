using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class AnswerType
{
    public enum AnsType
    {
        Other,
        Resource,
        Item
    }

    public AnsType Type;
    public int OtherAmount;
    public OtherReward Other;
    public ResourceType Resource;
    public string ItemName;

    public int Chance;

    public enum OtherReward
    {
        Nothing,
        Wound,
        GetAway,
        TopInvItem,
        FoodInvItem,
        WoundCure
    }
    public enum ResourceType
    {
        NULL,
        Food,
        Meat,
        Water,
        GunStuff,
        SharpEdges,
        Survival,
        Medical,
        Valuable
    }

    public enum WhereItHappens
    {
        Both,
        BeforeCamp,
        AfterCamp
    }

    public AnswerType(AnswerTypeSO convert)
    {
        Type = convert.Type;
        OtherAmount = convert.OtherAmount;
        Other = convert.Other;
        Resource = convert.Resource;
        ItemName = convert.ItemName;
        Chance = convert.Chance;
    }

    public AnswerType()
    {
        // do nothing
    }

    public ItemSO GetItem()
    {
        return Resources.Load($"Items/{ItemName}") as ItemSO;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class AnswerTypeSO : ScriptableObject
{
    public AnswerType.AnsType Type;
    public int OtherAmount;
    public AnswerType.OtherReward Other;
    public AnswerType.ResourceType Resource;
    public string ItemName;

    public int Chance;

    public static int NUMANSWERTYPE = 0;
    public int numAnswer;

    public AnswerTypeSO(AnswerType convert)
    {
        Type = convert.Type;
        OtherAmount = convert.OtherAmount;
        Other = convert.Other;
        Resource = convert.Resource;
        ItemName = convert.ItemName;
        Chance = convert.Chance;



#if UNITY_EDITOR

        if (Type != AnswerType.AnsType.Other)
        {
            numAnswer = NUMANSWERTYPE++;

            AssetDatabase.CreateAsset(this, $"Assets/Resources/Encounters/AnswerTypes/AnswerType#{numAnswer}.asset");
            AssetDatabase.SaveAssets();
        }
        else if (Other != AnswerType.OtherReward.Nothing)
        {
            numAnswer = NUMANSWERTYPE++;

            AssetDatabase.CreateAsset(this, $"Assets/Resources/Encounters/AnswerTypes/AnswerType#{numAnswer}.asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public static void CheckLatestNum()
    {
        NUMANSWERTYPE = 0;
        foreach (AnswerTypeSO a in Resources.LoadAll<AnswerTypeSO>("Encounters/AnswerTypes"))
        {
            if (a.numAnswer > NUMANSWERTYPE) NUMANSWERTYPE = a.numAnswer;
        }
        NUMANSWERTYPE += 1;
    }

}
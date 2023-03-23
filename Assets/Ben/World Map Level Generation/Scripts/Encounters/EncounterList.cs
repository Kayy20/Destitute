using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterList
{

    List<EncounterSO> beforeList = new List<EncounterSO>();
    List<EncounterSO> afterList = new List<EncounterSO>();
    List<EncounterSO> bothList = new List<EncounterSO>();

    public EncounterList()
    {
        FillLists();
    }


    private void FillLists()
    {
        EncounterSO[] encounters = Resources.LoadAll<EncounterSO>("Encounters");

        //List<EncounterSO> encounters = EncounterSaveSystem.CurrentEncounters;
        if (encounters == null)
        {
            CD.Log(CD.Programmers.BEN, "NO ENCOUNTERS FOUND, PLEASE MAKE SOME!");
        }
        else
        {
            for (int i = 0; i < encounters.Length; i++)
            {
                encounters[i].OrganizeIncomingLists();
                switch (encounters[i].where)
                {
                    case AnswerType.WhereItHappens.BeforeCamp:
                        beforeList.Add(encounters[i]);
                        break;
                    case AnswerType.WhereItHappens.AfterCamp:
                        afterList.Add(encounters[i]);
                        break;
                    case AnswerType.WhereItHappens.Both:
                        bothList.Add(encounters[i]);
                        break;
                }
            }
        }        
    }


    public EncounterSO CheckEncounterProc(AnswerType.WhereItHappens where = AnswerType.WhereItHappens.Both, int procChance = 80)
    {
        int rand = Mathf.Clamp(Random.Range(0, 101), 0, 100);

        if (rand <= procChance)
        {
            int r;
            switch (where)
            { // TODO ask Brayden if there "Can" be duplicate encounters in case they run into it again
                case AnswerType.WhereItHappens.BeforeCamp:
                    // Pick from before and both list
                    CD.Log(CD.Programmers.BEN, "Encounter Happened Before Camp");
                    Debug.Log("BEFORE LIST");
                    PrintList(beforeList);

                    Debug.Log("BOTH LIST");
                    PrintList(bothList);
                    r = Mathf.Clamp(Random.Range(0, beforeList.Count + bothList.Count), 0, beforeList.Count + bothList.Count);

                    //r = beforeList.Count + bothList.Count - 2;

                    Debug.Log($"r: {r}");

                    if (r >= beforeList.Count)
                    {
                        return bothList[beforeList.Count > 0 ? r - beforeList.Count : r];
                    }
                    else
                    {
                        
                        return beforeList[r];
                    }
                case AnswerType.WhereItHappens.AfterCamp:
                    // Pick from after and both list
                    CD.Log(CD.Programmers.BEN, "Encounter Happened After Camp");
                    Debug.Log("AFter LIST");
                    PrintList(afterList);

                    Debug.Log("BOTH LIST");
                    PrintList(bothList);

                    r = Mathf.Clamp(Random.Range(0, afterList.Count + bothList.Count), 0, afterList.Count + bothList.Count);
                    if (r >= afterList.Count)
                    {
                        return bothList[afterList.Count > 0 ? r - afterList.Count : r];
                    }
                    else
                    {
                        return afterList[r];
                    }
                case AnswerType.WhereItHappens.Both:
                    // Pick from both list
                    r = Mathf.Clamp(Random.Range(0, bothList.Count), 0, bothList.Count);
                    return bothList[r];
            }
        }
        return null;

    }

    private T[] FindObjectsOfType<T>()
    {
        return FindObjectsOfType<T>();
    }

    // Player Clicks answer and gets the rewards that are attached to it
    public void EncounterChoice(EncounterSO encounter, int answer)
    {
        // Deduct the required Resources from inventory/resource pool
        for (int i = 0; i < encounter.answerReq[answer].Count; i++)
        {
            switch (encounter.answerReq[answer][i].Type)
            {
                case AnswerType.AnsType.Resource:
                    switch (encounter.answerReq[answer][i].Resource)
                    {
                        case AnswerType.ResourceType.Food:
                            // Remove Food
                            PlayerStats.Instance.PlayerFood -= 1;
                            break;
                        case AnswerType.ResourceType.Water:
                            PlayerStats.Instance.PlayerWater -= 1;
                            // Remove Water
                            break;
                        case AnswerType.ResourceType.Medical:
                            PlayerStats.Instance.PlayerMedical -= 1;
                            // Remove Medical
                            break;
                        case AnswerType.ResourceType.Survival:
                            PlayerStats.Instance.PlayerSurvival -= 1;
                            // Remove Survival
                            break;
                    }
                    break;
                case AnswerType.AnsType.Item:
                    // Remove Item ---- TODO
                    break;
            }
        }



        // Add the rewarded Resources to inventory/resource pool
        for (int i = 0; i < encounter.answerRew[answer].Count; i++)
        {
            switch (encounter.answerRew[answer][i].Type)
            {
                case AnswerType.AnsType.Resource:
                    switch (encounter.answerRew[answer][i].Resource)
                    {
                        case AnswerType.ResourceType.Food:
                            // Give Food
                            PlayerStats.Instance.PlayerFood += 1;
                            break;
                        case AnswerType.ResourceType.Water:
                            PlayerStats.Instance.PlayerWater += 1;
                            // Give Water
                            break;
                        case AnswerType.ResourceType.Medical:
                            PlayerStats.Instance.PlayerMedical += 1;
                            // Give Medical
                            break;
                        case AnswerType.ResourceType.Survival:
                            PlayerStats.Instance.PlayerSurvival += 1;
                            // Give Survival
                            break;
                    }
                    break;
                case AnswerType.AnsType.Item:
                    // Give Item ---- TODO
                    break;
                case AnswerType.AnsType.Other:
                    // Give whatever they need
                    switch (encounter.answerRew[answer][i].Other)
                    {
                        case AnswerType.OtherReward.WoundCure:
                            // Give Sanity
                            break;
                            break;
                        case AnswerType.OtherReward.Wound:
                            // Give Wound
                            break;
                    }
                    break;
            }
        }
    }



    private void PrintList(List<EncounterSO> list)
    {
        foreach (EncounterSO enc in list)
        {
            CD.Log(CD.Programmers.BEN, $"Encounter Name: {enc.encounterName}");
        }
    }

}

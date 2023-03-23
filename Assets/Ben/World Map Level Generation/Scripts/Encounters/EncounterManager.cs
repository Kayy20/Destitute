using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager
{
    private static EncounterManager instance;
    public static EncounterManager Instance { get { return instance != null ? instance : instance = new EncounterManager(); } }
    // This is for requirements.

    public List<AnswerType.ResourceType> AnswerResourceRequirements = new List<AnswerType.ResourceType>();
    public List<bool> ResourceRequirementsCompleted = new List<bool>();

    public EncounterUI EncounterGO { get; set; }
    private PlayerMap player;
    public PlayerMap Player { get { return player != null ? player : player = GameObject.FindObjectOfType<PlayerMap>(); } }

    private EncounterSO enc;

    public bool goodToGo;

    public bool tutorialShownAlready;

    public void ShowEncounter(EncounterSO encounter, PlayerMap.StopStation station)
    {
        EncounterGO.gameObject.SetActive(true);
        EncounterGO.StopStation = station;
        EncounterGO.DrawEncounter(encounter);
        enc = encounter;
    }

    public void HideEncounter()
    {
        EncounterGO.HideEncounter();
        player.ContinueAfterCheck(enc.where == AnswerType.WhereItHappens.BeforeCamp? PlayerMap.StopStation.EncounterBefore : PlayerMap.StopStation.EncounterAfter);
    }

    public void CompletedItem(AnswerType.ResourceType resource)
    {
        for (int i = 0; i < AnswerResourceRequirements.Count; i++)
        {
            if (!ResourceRequirementsCompleted[i] && AnswerResourceRequirements[i] == resource)
            {
                ResourceRequirementsCompleted[i] = true;
                CheckReady();
            }
        }
    }

    public void CheckReady()
    {
        foreach (bool b in ResourceRequirementsCompleted)
        { // Checks if the player can click the 
            if (!b)
            {
                return;
            }
        }
        goodToGo = true;

    }

}

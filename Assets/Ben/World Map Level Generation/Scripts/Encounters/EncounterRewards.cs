using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterRewards : MonoBehaviour
{

    public PlayerMap.StopStation stopStation;
    public GameObject map;
    public Text t;


    public void SetText(List<EncounterUI.WhatHappenend> whatHappened, List<string> whatString, PlayerMap.StopStation stopStation)
    {
        this.stopStation = stopStation;
        string outputText = "Using ";

        for (int i = 0; i < whatHappened.Count; i++)
        {
            // i = 0 means Requirements
            switch (whatHappened[i])
            {
                case EncounterUI.WhatHappenend.AidKitGone:
                    outputText += "your first aid kit, ";
                    break;
                case EncounterUI.WhatHappenend.FoodGained:
                    outputText += "you gained one(1) food!";
                    break;
                case EncounterUI.WhatHappenend.FoodGained2:
                    outputText += "you gained two(2) food!";
                    break;
                case EncounterUI.WhatHappenend.GotAway:
                    outputText += "you were able to get away saftely";
                    break;
                case EncounterUI.WhatHappenend.Kniff:
                    outputText += "your knife, ";
                    break;
                case EncounterUI.WhatHappenend.LostItem:
                    outputText += "you lost an item, which one? We'll never know.";
                    break;
                case EncounterUI.WhatHappenend.Machettt:
                    outputText += "your machete, ";
                    break;
                case EncounterUI.WhatHappenend.MovedOn:
                    outputText += "you were able to move on.";
                    break;
                case EncounterUI.WhatHappenend.Nothing:
                    outputText = $"You {whatString[i]}, ";
                    break;
                case EncounterUI.WhatHappenend.Pills:
                    outputText += "painkillers, ";
                    break;
                case EncounterUI.WhatHappenend.SharpEdgeUsed:
                    outputText += "your sharp edged blade, ";
                    break;
                case EncounterUI.WhatHappenend.Shot:
                    outputText += "your gun and a bullet, ";
                    break;
                case EncounterUI.WhatHappenend.SoupGone:
                    outputText += "a can of soup, ";
                    break;
                case EncounterUI.WhatHappenend.UsedParacord:
                    outputText += "your paracord, ";
                    break;
                case EncounterUI.WhatHappenend.UsedWater:
                        outputText += "a bottle of water, ";
                    break;
                case EncounterUI.WhatHappenend.WoundsGained:
                    outputText += "you gained one(1) wound.";
                    break;
                case EncounterUI.WhatHappenend.WoundsGained2:
                    outputText += "you gained two(2) wounds.";
                    break;
                case EncounterUI.WhatHappenend.WoundsLost:
                    outputText += "you were able to heal one(1) wound!";
                    break;
                case EncounterUI.WhatHappenend.Valuable:
                    outputText += "you gained a valuable item!";
                    break;
                case EncounterUI.WhatHappenend.WaterSurvGain:
                    outputText += "you gained one(1) water item and one(1) survival item!";
                    break;
                case EncounterUI.WhatHappenend.MedFoodGain:
                    outputText += "you gained one(1) medical item and one(1) food item!";
                    break;
                case EncounterUI.WhatHappenend.SurvMedGain:
                    outputText += "you gained one(1) survival item and one(1) medical item!";
                    break;
            }
        }

        if (whatHappened.Count == 1) // Meaning there was no reward image... meaning no empty text
        {
            outputText += "you were able to move on.";
        }

        t.text = outputText;
    }

    public void ReturnToMap()
    {
        map.SetActive(true);
        PlayerMap player = map.GetComponentInChildren<PlayerMap>();
        player.ContinueAfterCheck(stopStation);
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class ImagesResourceLocation
{
    public static string food = "Ben/Encounter_UI_0";
    public static string water = "Ben/Water";
    public static string knife = "Ben/Knife";
    public static string firstAidKit = "Ben/MedKit";
    public static string painKiller = "Ben/Painkiller";
    public static string soup = "Ben/Soup";
    public static string meat = "Ben/Meat";
    public static string paracord = "Ben/Paracord";
    public static string lockpick = "Ben/Lockpicks";
    public static string gun = "Ben/Mosin";
    public static string bullet = "Ben/Bullets";
    public static string wound = "Ben/Wound";
    public static string woundCure = "Ben/Bandage";
    public static string Machette = "Ben/Machette";
    public static string map = "Ben/Map";
    public static string loseItem = "Ben/Itemloss";
    public static string x = "Ben/Map_Icons_3";
    public static string valuable = "Ben/Valuable";
}

public class EncounterUI : MonoBehaviour
{

    public static EncounterUI Instance;

    [SerializeField] Text titleText;
    [SerializeField] GameObject button;
    [SerializeField] GameObject buttonDropParent;
    [SerializeField] GameObject imageToSpawn;
    [SerializeField] GameObject rewardImageParent;
    [SerializeField] GameObject requiredImageParent;
    [SerializeField] GameObject resultScreen;

    List<GameObject> buttonList = new List<GameObject>();
    List<List<GameObject>> reqImageList = new List<List<GameObject>>();
    List<List<GameObject>> rewImageList = new List<List<GameObject>>();
    List<bool> buttonSelected = new List<bool>();
    //List<List<AnswerType.ResourceType>> requirementsPerButton = new List<List<AnswerType.ResourceType>>();

    public AudioSoundSO sound;

    private bool otherRewardGiven = false;


    private int currentSelectedButton = 0; // 0 = no button selected

    [SerializeField] Image requirementImage;
    [SerializeField] GameObject map;
    [SerializeField] private GameObject rewardInv;
    public GameObject encounterRewards;
    public PlayerMap.StopStation StopStation { get; set; }

    // Tutorial Stuff
    public bool tutorialDone;
    public List<GameObject> tutorialTexts;

    public enum WhatHappenend
    {
        Nothing, // If it's nothing (requirements) use string
        WoundsGained, // Damage
        WoundsGained2, // Double Damage (From bear attack only)
        WoundsLost, // Healed
        FoodGained, // Gain Meat
        FoodGained2, // Gain Meat x2
        SoupGone, // Lost Soup
        Shot, // Lost Ammo
        Pills, // Used Pills
        AidKitGone, // Used First Aid Kit
        WaterGain, // Water Gained
        MedGain, // Medical Gained
        SurvGain, // Survival Gained
        MovedOn, // Basically Nothing but better
        GotAway, // Got away from bear, basically
        LostItem, // Haha you lost an item
        UsedParacord, // Used a paracord
        UsedWater, // Used a water
        SharpEdgeUsed, // Used a sharp edge --> Knife/Machette
        Machettt, // Machete USed
        Kniff, // Knife USed
        Valuable, // Valuable Gained

        // Double up resource Gained
        WaterSurvGain,
        MedFoodGain,
        SurvMedGain,

    }

    List<WhatHappenend> encounterHappening = new List<WhatHappenend>();
    List<string> encounterHappeningingString = new List<string>();

    public List<ItemSO> debugtestitemlist;

    private void Start()
    {
        Instance = this;
    }


    public GameObject backgroundImage;

    public void DrawEncounter(EncounterSO encounter)
    {
        // Set title text
        titleText.text = encounter.encounterProblem;

        //PrintEncounter(encounter);

        // Set Background
        backgroundImage.SetActive(true);
        backgroundImage.GetComponent<Image>().sprite = encounter.backgroundImage;

        // Set Answers w/ buttons
        // Buttons //
        int y = Screen.height / 2;
        for (int i = 0; i < encounter.numAnswers; i++)
        {
            GameObject b = Instantiate(button, new Vector3(Screen.width / 2, y, 0), Quaternion.identity);
            b.transform.SetParent(buttonDropParent.transform);
            b.GetComponent<EncounterButton>().SetStartingString(encounter.encounterAnswers[i]);

            int x = i;

            b.GetComponent<Button>().onClick.AddListener(() => { ChoiceClicked(x); });
            b.GetComponent<Button>().onClick.AddListener(() => { SoundController.PlaySound(sound); });
            buttonSelected.Add(false);

            buttonList.Add(b);

            b.transform.localScale = Vector3.one;

            //requirementsPerButton.Add(new List<AnswerType.ResourceType>());
            //if (encounter.answerReq != null)
            //    for (int j = 0; j > encounter.answerReq.Count; j++)
            //    {
            //        requirementsPerButton[i].Add(encounter.answerReq[i][j].Resource);
            //    }

            // End Buttons //

            //CD.Log(CD.Programmers.BEN, "" + encounter);
            // Requirement Images //
            //for (int j = 0; j < encounter.numRequirements[i]; j++)
            //{

            List<GameObject> reqImages = new List<GameObject>();

            foreach (AnswerType ans in encounter.answerReq[i])
            {
                bool destroyed = false;
                GameObject g = Instantiate(imageToSpawn, new Vector3(Screen.width / 2, y, 0), Quaternion.identity);
                g.transform.SetParent(requiredImageParent.transform.GetChild(i).transform);

                switch (ans.Type)
                {
                    case AnswerType.AnsType.Other:
                        switch (ans.Other)
                        {
                            case AnswerType.OtherReward.Nothing:
                                Destroy(g);
                                destroyed = true;
                                break;
                        }
                        break;
                    case AnswerType.AnsType.Resource:
                        switch (ans.Resource)
                        {
                            case AnswerType.ResourceType.Food:
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.food);
                                break;
                            case AnswerType.ResourceType.Water:
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.water);
                                break;
                            case AnswerType.ResourceType.SharpEdges:
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.knife);
                                break;
                            case AnswerType.ResourceType.Medical:
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.firstAidKit);
                                break;
                            case AnswerType.ResourceType.NULL:
                                Destroy(g);
                                destroyed = true;
                                break;
                        }
                        break;
                    case AnswerType.AnsType.Item:

                        switch (ans.ItemName)
                        {
                            case "Ammo":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.bullet);
                                break;
                            case "Painkillers":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.painKiller);
                                break;
                            case "Soup":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.soup);
                                break;
                            case "Mosin":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.gun);
                                break;
                            case "Machette":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.Machette);
                                break;
                            case "Knife":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.knife);
                                break;
                            case "MedPack":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.firstAidKit);
                                break;
                            case "Paracord":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.paracord);
                                break;
                            case "Map":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.map);
                                break;
                            case "Lockpick":
                                g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.lockpick);
                                break;
                        }
                        break;
                }

                if (!destroyed)
                {
                    g.GetComponent<EncounterImage>().type = ans;

                    g.GetComponent<Image>().color = Color.gray;
                    reqImages.Add(g);

                    g.SetActive(false);
                }


            }
            Debug.Log($"Req Images({reqImageList.Count}): amount in list: {reqImages.Count}");
            reqImageList.Add(reqImages);

            //}

            // End Requirement Images //


            // Reward Images //
            if (encounter.answerRew != null)
            {
                //for (int j = 0; j < encounter.answerRew[i].Length; j++)
                //{
                List<GameObject> rewImages = new List<GameObject>();

                foreach (AnswerType ans in encounter.answerRew[i])
                {
                    bool destroyed = false;
                    GameObject g = Instantiate(imageToSpawn, new Vector3(Screen.width / 2, y, 0), Quaternion.identity);
                    g.transform.SetParent(rewardImageParent.transform.GetChild(i).transform);

                    switch (ans.Type)
                    {
                        case AnswerType.AnsType.Other:
                            switch (ans.Other)
                            {
                                case AnswerType.OtherReward.Nothing:
                                    Destroy(g);
                                    destroyed = true;
                                    break;
                                case AnswerType.OtherReward.FoodInvItem:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.loseItem);
                                    break;
                                case AnswerType.OtherReward.GetAway:
                                    Destroy(g);
                                    destroyed = true;
                                    break;
                                case AnswerType.OtherReward.TopInvItem:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.loseItem);
                                    break;
                                case AnswerType.OtherReward.Wound:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.wound);
                                    otherRewardGiven = false;
                                    break;
                                case AnswerType.OtherReward.WoundCure:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.woundCure);
                                    otherRewardGiven = false;
                                    break;
                            }
                            break;
                        case AnswerType.AnsType.Resource:
                            switch (ans.Resource)
                            {
                                case AnswerType.ResourceType.Meat:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.meat);
                                    break;
                                case AnswerType.ResourceType.Water:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.water);
                                    break;
                                case AnswerType.ResourceType.Survival:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.knife);
                                    break;
                                case AnswerType.ResourceType.Medical:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.firstAidKit);
                                    break;
                                case AnswerType.ResourceType.Valuable:
                                    g.GetComponent<Image>().sprite = Resources.Load<Sprite>(ImagesResourceLocation.valuable);
                                    break;
                            }
                            break;
                        case AnswerType.AnsType.Item: // No Item rewards (I think)
                            break;
                    }

                    if (!destroyed)
                    {
                        g.GetComponent<Image>().color = Color.white;

                        g.GetComponent<EncounterImage>().type = ans;

                        rewImages.Add(g);

                        g.SetActive(false);
                    }
                }
                Debug.Log($"Rew Images({rewImageList.Count}): amount in list: {rewImages.Count}");
                rewImageList.Add(rewImages);
            }

            // End Reward Images //
            y -= 25;

            //}
        }


        if (encounter.sound!= null)
            SoundController.PlaySound(encounter.sound);

    }

    public bool ItemSelected(ItemSO item)
    {
        
        Debug.Log("RECEIVED ITEM");
        // Search to see if item is needed

        for (int i = 0; i < buttonSelected.Count; i++)
        {
            if (buttonSelected[i])
            {
                // Check to see if the images are grayed out
                foreach (GameObject g in reqImageList[i])
                {
                    if (g.GetComponent<Image>().color != Color.white)
                    {
                        // Check if this is the resource/item needed
                        EncounterImage req = g.GetComponent<EncounterImage>();

                        switch (req.type.Type)
                        {
                            case AnswerType.AnsType.Item:
                                // Compare between itemNames
                                if (item.name.Equals(req.type.ItemName))
                                {
                                    g.GetComponent<Image>().color = Color.white;
                                    return true;
                                }
                                break;
                            case AnswerType.AnsType.Resource:
                                // Compare between resource type
                                if (item.resType.Equals(req.type.Resource))
                                {
                                    g.GetComponent<Image>().color = Color.white;
                                    return true;
                                }
                                break;
                        }
                    }
                }
            }
        }
        

        return false;
    }

    void OnEnable()
    {
        InventoryController.Instance.ToggleCampUI(true);
    }

    private void CompareReq(List<GameObject> reqImage, int buttonNum)
    {

        if (reqImage.Count == 0)
        {

            encounterHappeningingString.Add(buttonList[buttonNum].GetComponent<EncounterButton>().GetStartingString());
            encounterHappening.Add(WhatHappenend.Nothing);
        }

        foreach (GameObject g in reqImage)
        {
            switch (g.GetComponent<EncounterImage>().type.Type)
            {
                case AnswerType.AnsType.Resource:
                    switch (g.GetComponent<EncounterImage>().type.Resource)
                    {
                        case AnswerType.ResourceType.Water:
                            encounterHappening.Add(WhatHappenend.UsedWater);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.ResourceType.SharpEdges:
                            encounterHappening.Add(WhatHappenend.SharpEdgeUsed);
                            encounterHappeningingString.Add("");
                            break;
                    }
                    break;
                case AnswerType.AnsType.Item:
                    switch (g.GetComponent<EncounterImage>().type.ItemName)
                    {
                        case "Ammo":
                            encounterHappening.Add(WhatHappenend.Shot);
                            encounterHappeningingString.Add("");
                            break;
                        case "Painkillers":
                            encounterHappening.Add(WhatHappenend.Pills);
                            encounterHappeningingString.Add("");
                            break;
                        case "Soup":
                            encounterHappening.Add(WhatHappenend.SoupGone);
                            encounterHappeningingString.Add("");
                            break;
                        case "Machette":
                            encounterHappening.Add(WhatHappenend.Machettt);
                            encounterHappeningingString.Add("");
                            break;
                        case "Knife":
                            encounterHappening.Add(WhatHappenend.Kniff);
                            encounterHappeningingString.Add("");
                            break;
                        case "MedPack":
                            encounterHappening.Add(WhatHappenend.AidKitGone);
                            encounterHappeningingString.Add("");
                            break;
                        case "Paracord":
                            encounterHappening.Add(WhatHappenend.UsedParacord);
                            encounterHappeningingString.Add("");
                            break;
                    }
                    break;
            }
        }
    }
    // Removed till further notice, cuz found way to do it in GiveRewards();
    private void CompareRew(List<GameObject> reqImage)
    {
        if (reqImage.Count == 0)
        {
            encounterHappeningingString.Add("Moved On");
            encounterHappening.Add(WhatHappenend.Nothing);
        }


        foreach (GameObject g in reqImage)
        {
            switch (g.GetComponent<EncounterImage>().type.Type)
            {
                case AnswerType.AnsType.Resource:
                    switch (g.GetComponent<EncounterImage>().type.Resource)
                    {
                        case AnswerType.ResourceType.Water:
                            encounterHappening.Add(WhatHappenend.WaterGain);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.ResourceType.Survival:
                            encounterHappening.Add(WhatHappenend.SurvGain);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.ResourceType.Meat:
                            encounterHappening.Add(WhatHappenend.FoodGained);
                            encounterHappeningingString.Add("");
                            if (encounterHappening.Contains(WhatHappenend.FoodGained))
                            {
                                encounterHappeningingString.Remove("");
                                encounterHappeningingString.Add("");
                                encounterHappening.Remove(WhatHappenend.FoodGained);
                                encounterHappening.Add(WhatHappenend.FoodGained2);
                            }
                            break;

                    }
                    break;
                case AnswerType.AnsType.Other:
                    switch (g.GetComponent<EncounterImage>().type.Other)
                    {
                        case AnswerType.OtherReward.WoundCure:
                            encounterHappening.Add(WhatHappenend.WoundsLost);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.OtherReward.TopInvItem:
                            encounterHappening.Add(WhatHappenend.LostItem);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.OtherReward.GetAway:
                            encounterHappening.Add(WhatHappenend.GotAway);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.OtherReward.FoodInvItem:
                            encounterHappening.Add(WhatHappenend.LostItem);
                            encounterHappeningingString.Add("");
                            break;
                        case AnswerType.OtherReward.Nothing:
                            encounterHappening.Add(WhatHappenend.MovedOn);
                            encounterHappeningingString.Add("");
                            break;

                    }
                    break;
            }
        }
    }

    public void ChoiceClicked(int button)
    { // Check if the requirements of the choice selected are true
        if (buttonSelected[button]) // This button has already been selected once
        {
            int count = 0;
            foreach (GameObject g in reqImageList[button])
            {
                if (g.GetComponent<Image>().color == Color.white) // Check if players got resource
                {
                    count++;
                }
            }

            if (count == reqImageList[button].Count) // Send the player on their way
            {
                List<EncounterImage> list = new List<EncounterImage>();
                foreach (GameObject g in rewImageList[button])
                {
                    list.Add(g.GetComponent<EncounterImage>());
                }

                // Apply the required stuff to the rewards screen
                CompareReq(reqImageList[button], button);
                //CompareRew(rewImageList[button]);

                GiveRewards(list);
            }
        }
        else // This button has been selected for the first time
        {

            if (!tutorialDone)
                foreach (GameObject g in tutorialTexts)
                    g.SetActive(true);

            // Clear Items
            InventoryController.Instance.ClearSelectedItems();

            for (int i = 0; i < buttonSelected.Count; i++)
            {

                // Reset button text
                buttonList[i].GetComponent<EncounterButton>().RevertToStartingString();

                // Hide all other images
                // REQUIREMENTS //
                if (i < reqImageList.Count)
                    foreach (GameObject g in reqImageList[i])
                    {
                        g.GetComponent<Image>().color = Color.gray;                            
                        g.SetActive(false);
                    
                    }
                // REWARDS //
                if (i < rewImageList.Count)
                    foreach (GameObject g in rewImageList[i])
                    {
                        g.SetActive(false);
                    }

                buttonSelected[i] = false;
                // Show images
                if (i == button)
                { // This is the button that has been clicked
                    buttonSelected[button] = true;

                    // Update this Button's text
                    buttonList[i].GetComponent<EncounterButton>().CurrentString = "Select Items";
                    currentSelectedButton = i + 1;
                    

                    // Show what resources are needed to the drop box

                    //EncounterManager.Instance.CheckReady();
                    if (button < reqImageList.Count)
                        foreach (GameObject g in reqImageList[button])
                        {
                            g.SetActive(true);
                        }

                    if (button < rewImageList.Count)
                        foreach (GameObject g in rewImageList[button])
                        {
                            g.SetActive(true);
                        }

                    //return;
                }
            }
        }
        

    }


    private void GiveRewards(List<EncounterImage> rewards)
    {
        List<ItemSO> itemList = new List<ItemSO>();
        ItemSO[] iList;

        if (!tutorialDone)
        {
            tutorialDone = true;
            foreach (GameObject g in tutorialTexts)
                g.SetActive(false);
        }

        foreach (EncounterImage img in rewards)
        {
            Debug.Log($"img: {img.type.Type}");
            switch (img.type.Type)
            {
                case AnswerType.AnsType.Resource:
                    switch (img.type.Resource)
                    {
                        case AnswerType.ResourceType.Meat:
                            // give random food item...
                            iList = new ItemSO[1];
                            iList[0] = Resources.Load<ItemSO>("Items/Food/MeatItem");
                            itemList.Add(iList[Random.Range(0, iList.Length)]);
                            if (encounterHappening.Contains(WhatHappenend.FoodGained))
                            {
                                // Remove from list
                                encounterHappening.Remove(WhatHappenend.FoodGained);
                                encounterHappeningingString.Remove("");

                                encounterHappening.Add(WhatHappenend.FoodGained2);
                                encounterHappeningingString.Add("");
                            }
                            else
                            {
                                encounterHappening.Add(WhatHappenend.FoodGained);
                                encounterHappeningingString.Add("");
                            }
                            break;
                        case AnswerType.ResourceType.Water:
                            // give random water item...
                            iList = new ItemSO[1];
                            iList[0] = Resources.Load<ItemSO>("Items/Water/WaterItem");
                            itemList.Add(iList[Random.Range(0, iList.Length)]);
                            if (encounterHappening.Contains(WhatHappenend.SurvGain))
                            {
                                encounterHappening.Remove(WhatHappenend.SurvGain);
                                encounterHappening.Add(WhatHappenend.WaterSurvGain);
                                encounterHappeningingString.Add("");
                            }
                            else
                            {
                                encounterHappening.Add(WhatHappenend.WaterGain);
                                encounterHappeningingString.Add("");
                            }
                            break;
                        case AnswerType.ResourceType.Survival:
                            // give random survival item...
                            iList = Resources.LoadAll<ItemSO>("Items/Survival");
                            itemList.Add(iList[Random.Range(0, iList.Length)]);
                            if (encounterHappening.Contains(WhatHappenend.WaterGain))
                            {
                                encounterHappening.Remove(WhatHappenend.WaterGain);
                                encounterHappening.Add(WhatHappenend.WaterSurvGain);
                                encounterHappeningingString.Add("");
                            }
                            else if (encounterHappening.Contains(WhatHappenend.MedGain))
                            {
                                encounterHappening.Remove(WhatHappenend.MedGain);
                                encounterHappening.Add(WhatHappenend.SurvMedGain);
                                encounterHappeningingString.Add("");
                            }
                            else
                            {
                                encounterHappening.Add(WhatHappenend.SurvGain);
                                encounterHappeningingString.Add("");
                            }
                            break;
                        case AnswerType.ResourceType.Medical:
                            // give random medical item...
                            iList = new ItemSO[1];
                            iList[0] = Resources.Load<ItemSO>("Items/Medical/MedItem");
                            itemList.Add(iList[Random.Range(0, iList.Length)]);
                            if (encounterHappening.Contains(WhatHappenend.FoodGained))
                            {
                                encounterHappening.Remove(WhatHappenend.FoodGained);
                                encounterHappening.Add(WhatHappenend.MedFoodGain);
                                encounterHappeningingString.Add("");
                            }
                            else if (encounterHappening.Contains(WhatHappenend.SurvGain))
                            {
                                encounterHappening.Remove(WhatHappenend.SurvGain);
                                encounterHappening.Add(WhatHappenend.SurvMedGain);
                                encounterHappeningingString.Add("");
                            }
                            else
                            {
                                encounterHappening.Add(WhatHappenend.MedGain);
                                encounterHappeningingString.Add("");
                            }
                            break;
                        case AnswerType.ResourceType.Valuable:
                            // give random valuable item...
                            iList = Resources.LoadAll<ItemSO>("Items/Valuables");
                            itemList.Add(iList[Random.Range(0, iList.Length)]);
                            encounterHappening.Add(WhatHappenend.Valuable);
                            encounterHappeningingString.Add("");
                            break;
                    }
                    break;

                case AnswerType.AnsType.Other:
                    switch (img.type.Other)
                    {
                        case AnswerType.OtherReward.Wound:
                            if (!otherRewardGiven)
                                switch (img.type.Other)
                                {// use their "Chance" to see which one they get, prioritize the first one
                                    case AnswerType.OtherReward.Wound:
                                        int rand = Mathf.Clamp(Random.Range(0, 101), 0, 100);
                                        CD.Log(CD.Programmers.BEN, "" + rand);
                                        if (img.type.Chance >= rand)
                                        {
                                            //apply wounds
                                            StaticClassVariables.Wounds += img.type.OtherAmount;
                                            CD.Log(CD.Programmers.BEN, "ADDED WOUNDS");

                                            if (img.type.OtherAmount != 2) encounterHappening.Add(WhatHappenend.WoundsGained);
                                            else encounterHappening.Add(WhatHappenend.WoundsGained2);
                                            encounterHappeningingString.Add("");

                                            otherRewardGiven = true;
                                        }
                                        else
                                        {
                                            encounterHappening.Add(WhatHappenend.MovedOn);
                                            encounterHappeningingString.Add("");
                                        }
                                        break;
                                    case AnswerType.OtherReward.GetAway:
                                        otherRewardGiven = true;
                                        encounterHappening.Add(WhatHappenend.GotAway);
                                        encounterHappeningingString.Add("");
                                        break;
                                    case AnswerType.OtherReward.WoundCure:
                                        if (StaticClassVariables.Wounds > 0)
                                        {
                                            StaticClassVariables.Wounds -= 1;
                                            encounterHappening.Add(WhatHappenend.WoundsLost);
                                            encounterHappeningingString.Add("");
                                        }
                                        else
                                        {
                                            encounterHappening.Add(WhatHappenend.MovedOn);
                                            encounterHappeningingString.Add("");
                                        }
                                        otherRewardGiven = true;
                                        break;
                                    case AnswerType.OtherReward.TopInvItem:
                                        if (InventoryController.Instance.RemoveRandomItem()) 
                                        {
                                            encounterHappening.Add(WhatHappenend.LostItem);
                                            encounterHappeningingString.Add("");
                                        }
                                        else
                                        {
                                            encounterHappening.Add(WhatHappenend.MovedOn);
                                            encounterHappeningingString.Add("");
                                        }
                                        
                                        otherRewardGiven = true;
                                        break;
                                    case AnswerType.OtherReward.FoodInvItem:
                                        int rand1 = Mathf.Clamp(Random.Range(0, 101), 0, 100);
                                        CD.Log(CD.Programmers.BEN, "" + rand1);
                                        if (img.type.Chance >= rand1)
                                            if (InventoryController.Instance.RemoveRandomFoodItem())
                                            {
                                                encounterHappening.Add(WhatHappenend.LostItem);
                                                encounterHappeningingString.Add("");
                                            }
                                            else
                                            {
                                                encounterHappening.Add(WhatHappenend.MovedOn);
                                                encounterHappeningingString.Add("");
                                            }
                                        else
                                        {
                                            encounterHappening.Add(WhatHappenend.MovedOn);
                                            encounterHappeningingString.Add("");
                                        }
                                        otherRewardGiven = true;
                                        break;
                                    case AnswerType.OtherReward.Nothing:
                                        encounterHappening.Add(WhatHappenend.MovedOn);
                                        encounterHappeningingString.Add("");
                                        break;
                                }
                            break;
                    }
                    break;
            }
        }
        // Display Rewards
        DisplayRewards(itemList);
        // Delete Stuff
        InventoryController.Instance.DeleteSelectedItems();

    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    DisplayRewards(debugtestitemlist);
        //}

        if (currentSelectedButton != 0)
        {

            int count = 0;
            foreach (GameObject g in reqImageList[currentSelectedButton - 1])
            {
                if (g.GetComponent<Image>().color == Color.white) // Check if players got resource
                {
                    count++;
                }
            }
            if (count == reqImageList[currentSelectedButton - 1].Count)
            {
                buttonList[currentSelectedButton - 1].GetComponent<EncounterButton>().CurrentString = "Confirm?";
                currentSelectedButton = 0;
                
            }
        }

    }

    private IEnumerator RewardDelay(List<ItemSO> itemlist)
    {
        if (itemlist.Count <= 0)
        {
            HideEncounterUI();
            yield break;
        }

        PlayerMap.Instance.currentStation = PlayerMap.StopStation.EncounterRewards;
        yield return new WaitForSeconds(0.1f);
        rewardInv.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        InventoryController.Instance.AddPickupTiles();
        yield return new WaitForSeconds(0.1f);
        foreach (var itemSo in itemlist)
        {
            InventoryController.Instance.SpawnObject(itemSo, InventoryController.Instance.pickupInv);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DisplayRewards(List<ItemSO> itemList)
    {
        // TODO Daniel do stuff

        StartCoroutine(RewardDelay(itemList));

        // Make button which calls this function once done:
        //HideEncounterUI();
    }

    private void PrintEncounter(EncounterSO encounter)
    {
        foreach (List<AnswerType> gList in encounter.answerReq)
        {
            foreach (AnswerType g in gList)
            {
                CD.Log(CD.Programmers.BEN, $"{g.Resource}");
            }
        }
        
    }

    public void HideEncounter()
    {
        foreach (GameObject g in buttonList)
        {
            Destroy(g);
        }

        foreach (List<GameObject> gList in reqImageList)
            foreach (GameObject g in gList)
                Destroy(g);

        foreach (List<GameObject> gList in rewImageList)
            foreach (GameObject g in gList)
                Destroy(g);

        encounterHappening.Clear();
        encounterHappeningingString.Clear();
        buttonList.Clear();
        buttonSelected.Clear();
        reqImageList.Clear();
        rewImageList.Clear();

        backgroundImage.SetActive(false);


        if (!EncounterManager.Instance.tutorialShownAlready)
            EncounterManager.Instance.tutorialShownAlready = true;

    }

    public void HideEncounterUI()
    {
        InventoryController.Instance.RemovePickupTiles();
        InventoryController.Instance.ClearPickup();
        rewardInv.SetActive(false);
        InventoryController.Instance.ToggleCampUI(false);


        encounterRewards.GetComponent<EncounterRewards>().SetText(encounterHappening, encounterHappeningingString, StopStation);

        // Moving this to the encounter rewards/outcomes screen
        //map.SetActive(true);
        //PlayerMap player = map.GetComponentInChildren<PlayerMap>();
        //player.ContinueAfterCheck(StopStation);
        HideEncounter();
        encounterRewards.SetActive(true);
        gameObject.SetActive(false);

    }

}

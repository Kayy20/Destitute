using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class NewEncounter : EditorWindow
{
    private string encounterProblem;
    private string encounterName;

    private int numAnswers = 0;

    private string[] encounterAnswer = new string[3];
    private int[] numRewards = { 0, 0, 0 };
    private int[] numRequirements = { 0, 0, 0 };

    private AudioSoundSO sound;

    private AnswerType.WhereItHappens where = AnswerType.WhereItHappens.Both;

    private AnswerType[,] answerRewardType = new AnswerType[3, 3];
    private AnswerType[,] answerReqType = new AnswerType[3, 2];

    private ItemSO[,] answerReqItem = new ItemSO[3, 2];
    private ItemSO[,] answerRewItem = new ItemSO[3, 3];

    private int encounterEditing;

    /*
        private OtherReward[,] encounterRequirementOther = new OtherReward[3, 2];
        private OtherReward[,] encounterRewardOther = new OtherReward[3, 2];

        private ResourceType[,] encounterRequirementResource = new ResourceType[3, 2];
        private ResourceType[,] encounterRewardResource = new ResourceType[3, 2];

        private ItemSO[,] encounterRewardItem = new ItemSO[3, 2];
        private ItemSO[,] encounterRequirementItem = new ItemSO[3, 2];
    */

    private enum State
    {
        Start,
        ListOfEncounters,
        EditingEncounter,
        NewEncounter
    }

    private State state = State.Start;

    Vector2 scrollPos;

    [MenuItem("Tools/Edit/Manage Encounters")]
    static void Init()
    {
        NewEncounter window = (NewEncounter)EditorWindow.GetWindow(typeof(NewEncounter));
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true);

        switch (state)
        {
            case State.Start:
                // Base, add button for list current encounters, and new encounter
                ListButton();
                
                GUILayout.Space(10);

                NewButton();

                break;
            case State.ListOfEncounters:

                GUILayout.Label("Encounter list", EditorStyles.boldLabel);

                GUILayout.Space(10);

                ShowEncounters();

                GUILayout.Space(10);

                BackButton();

                break;
            case State.EditingEncounter:
                if (answerRewardType[0, 0] == null || answerReqType[0, 0] == null) InitializeArrays();

                //FillEncounterData(EncounterSaveSystem.CurrentEncounters[encounterEditing]);

                //GUILayout.Label($"Editing Encounter {EncounterSaveSystem.CurrentEncounters[encounterEditing].encounterName}", EditorStyles.largeLabel);

                GUILayout.Space(10);

                GUILayout.Space(10);

                EncounterProblem();

                GUILayout.Space(10);

                EncounterAnswers();

                GUILayout.Space(10);

                EncounterAudio();

                GUILayout.Space(15);

                ConfirmButton(true);

                break;
            case State.NewEncounter:
                if (answerRewardType[0, 0] == null || answerReqType[0, 0] == null) InitializeArrays();
                GUILayout.Label("New Encounter", EditorStyles.largeLabel);

                GUILayout.Space(10);

                EncounterProblem();

                GUILayout.Space(10);

                EncounterAnswers();

                GUILayout.Space(10);

                EncounterAudio();

                GUILayout.Space(15);

                ConfirmButton();

                break;
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }

    private void ShowEncounters()
    {
        //List<EncounterSO> encounterList = EncounterSaveSystem.CurrentEncounters;

        //for (int i = 0; i < encounterList.Count; i++)
        //{
        //    GUILayout.Label($"Encounter Name: {encounterList[i].encounterName}");

        //    if (GUILayout.Button("Edit Encounter"))
        //    {
        //        encounterEditing = i;
        //        state = State.EditingEncounter;
        //    }

        //    if (GUILayout.Button("Delete Encunter"))
        //    {
        //        EncounterSaveSystem.CurrentEncounters.RemoveAt(i);
        //        EncounterSaveSystem.SaveEncounterList(EncounterSaveSystem.CurrentEncounters);
        //    }

        //    GUILayout.Space(10);
        //}
    }

    private void FillEncounterData(EncounterSO encounter) 
    {
        encounterName = encounter.encounterName;
        encounterProblem = encounter.encounterProblem;
        where = encounter.where;

        numAnswers = 0;

        // Check through each encounter answer to see if it's not empty
        foreach (string s in encounter.encounterAnswers)
        {
            if (s.Length > 0) numAnswers++;
        }

        encounterAnswer = encounter.encounterAnswers;

        for (int i = 0; i < numAnswers; i++)
        {

            numRequirements[i] = encounter.numRequirements[i];

            for (int j = 0; j < 2; j++)
            {
                answerReqType[i, j] = encounter.answerReq[i][j];
            }

            numRewards[i] = encounter.numRewards[i];

            for (int j = 0; j < 3; j++)
            {
                answerRewardType[i, j] = encounter.answerRew[i][j];
            }
        }

        sound = encounter.sound;

    }

    private void ListButton()
    {
        if (GUILayout.Button("Show Current Encounters"))
        {
            state = State.ListOfEncounters;
        }
    }

    private void NewButton()
    {
        if (GUILayout.Button("Create New Encounter"))
        {
            state = State.NewEncounter;
        }
    }

    private void BackButton()
    { // Returns to Main Screen
        if (GUILayout.Button("Back"))
        {
            state = State.Start;
        }
    }

    private void InitializeArrays()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                answerRewardType[i, j] = new AnswerType();
                answerReqType[i, j] = new AnswerType(); 
            }
            answerRewardType[i, 2] = new AnswerType();
        }
    }

    private void EncounterProblem()
    {

        encounterName = EditorGUILayout.TextField("Asset Name:", encounterName);

        GUILayout.Space(10);

        encounterProblem = EditorGUILayout.TextField("Encounter Problem", encounterProblem);

        GUILayout.Space(5);

        where = (AnswerType.WhereItHappens)EditorGUILayout.EnumPopup("When?", where);

    }

    private void EncounterAnswers()
    {
        numAnswers = EditorGUILayout.IntField("Number of Answers: ", numAnswers);
        numAnswers = Mathf.Clamp(numAnswers, 1, 3);

        for (int i = 0; i < numAnswers; i++)
        {
            encounterAnswer[i] = EditorGUILayout.TextField("Encounter Answer " + (i + 1) + ":", encounterAnswer[i]);

            GUILayout.Space(10);

            numRequirements[i] = EditorGUILayout.IntField("Number of Requirements: ", numRequirements[i]);
            numRequirements[i] = Mathf.Clamp(numRequirements[i], 1, 2);


            for (int j = 0; j < numRequirements[i]; j++)
            {
                answerReqType[i, j].Type = (AnswerType.AnsType)EditorGUILayout.EnumPopup("Required Type:", answerReqType[i, j].Type);
                switch(answerReqType[i, j].Type)
                {
                    case AnswerType.AnsType.Item:
                        answerReqItem[i, j] = EditorGUILayout.ObjectField("Requirement:", answerReqItem[i, j], typeof(ItemSO), true) as ItemSO;
                        if (answerReqItem[i, j] != null)
                            answerReqType[i, j].ItemName = answerReqItem[i, j].name;
                        break;
                    case AnswerType.AnsType.Resource:
                        answerReqType[i, j].Resource = (AnswerType.ResourceType)EditorGUILayout.EnumPopup("Requirement:", answerReqType[i, j].Resource);
                        break;
                    case AnswerType.AnsType.Other:
                        answerReqType[i, j].Other = (AnswerType.OtherReward)EditorGUILayout.EnumPopup("Requirement:", answerReqType[i, j].Other);
                        break;
                }
                GUILayout.Space(5);
            }

            GUILayout.Space(5);

            numRewards[i] = EditorGUILayout.IntField("Number of Rewards: ", numRewards[i]);
            numRewards[i] = Mathf.Clamp(numRewards[i], 1, 3);

            for (int j = 0; j < numRewards[i]; j++)
            {
                answerRewardType[i, j].Type = (AnswerType.AnsType)EditorGUILayout.EnumPopup("Reward Type:", answerRewardType[i, j].Type);
                switch (answerRewardType[i, j].Type)
                {
                    case AnswerType.AnsType.Item:
                        answerRewItem[i, j] = EditorGUILayout.ObjectField("Reward:", answerRewItem[i, j], typeof(ItemSO), true) as ItemSO;
                        if (answerRewItem[i, j] != null)
                            answerRewardType[i, j].ItemName = answerRewItem[i, j].name;
                        break;
                    case AnswerType.AnsType.Resource:
                        answerRewardType[i, j].Resource = (AnswerType.ResourceType)EditorGUILayout.EnumPopup("Reward:", answerRewardType[i, j].Resource);
                        break;
                    case AnswerType.AnsType.Other:
                        answerRewardType[i, j].Other = (AnswerType.OtherReward)EditorGUILayout.EnumPopup("Reward:", answerRewardType[i, j].Other);
                        answerRewardType[i, j].Chance = EditorGUILayout.IntField("Chance:", answerRewardType[i, j].Chance);
                        answerRewardType[i, j].Chance = Mathf.Clamp(answerRewardType[i, j].Chance, 0, 100);
                        if (answerRewardType[i, j].Other == AnswerType.OtherReward.Wound)
                        {
                            answerRewardType[i, j].OtherAmount = EditorGUILayout.IntField("Amount", answerRewardType[i, j].OtherAmount);
                            answerRewardType[i, j].OtherAmount = Mathf.Clamp(answerRewardType[i, j].OtherAmount, 1, 3);
                        }
                        
                        break;
                }
                GUILayout.Space(5);
            }
            GUILayout.Space(10);
        }
    }

    private void EncounterAudio()
    {
        sound = (AudioSoundSO)EditorGUILayout.ObjectField("Audio Clip", sound, typeof(AudioSoundSO), true);
    }

    private void ConfirmButton(bool editing = false)
    {
        if (GUILayout.Button("Cancel"))
        {
            state = State.ListOfEncounters;
        }

        if (GUILayout.Button("Confirm"))
        {
            EncounterSO encounterSO = CreateEncounter();

            if (!editing)
            {
                //EncounterSaveSystem.CurrentEncounters.Add(encounterSO);
            }
            else
            {
                //EncounterSaveSystem.CurrentEncounters[encounterEditing] = encounterSO;
            }


            encounterSO.OrganizeOutgoingLists();

            AssetDatabase.CreateAsset(encounterSO, $"Assets/Resources/Encounters/{encounterSO.encounterName}.asset");
            AssetDatabase.SaveAssets();

            //EncounterSaveSystem.SaveEncounterList(EncounterSaveSystem.CurrentEncounters);

            

            // Take them to the list of encounters
            state = State.ListOfEncounters;

        }
    }

    private EncounterSO CreateEncounter()
    {
        EncounterSO encounterSO = new EncounterSO();

        encounterSO.encounterName = encounterName;

        encounterSO.numAnswers = numAnswers;

        List<List<AnswerType>> tempReqArray = new List<List<AnswerType>>();
        List<List<AnswerType>> tempRewArray = new List<List<AnswerType>>();

        for (int i = 0; i < numAnswers; i++)
        {
            //CD.LogInt(i, "I", CD.Programmers.BEN);
            tempReqArray.Add(new List<AnswerType>());
            tempRewArray.Add(new List<AnswerType>());
            for (int j = 0; j < numRequirements[i]; j++)
            {
                tempReqArray[i].Add(answerReqType[i, j]);

                //CD.Log(CD.Programmers.BEN, $"Resource Type: {answerReqType[i,j].Resource}");
                //tempReqArray[i][j] = answerReqType[i, j];
            }

            for (int j = 0; j < numRewards[i]; j++)
            {
                tempRewArray[i].Add(answerRewardType[i, j]);
            }
        }


        //foreach (List<AnswerType> list in tempReqArray)
        //{
        //    foreach(AnswerType t in list)
        //    {
        //        CD.Log(CD.Programmers.BEN, "" + t.Type);
        //    }
        //}
        encounterSO.answerReq = tempReqArray;
        encounterSO.answerRew = tempRewArray;

        encounterSO.encounterProblem = encounterProblem;

        encounterSO.encounterAnswers = encounterAnswer;

        encounterSO.numRequirements = numRequirements;
        encounterSO.numRewards = numRewards;

        encounterSO.where = where;

        if (sound != null)
        {
            encounterSO.sound = sound;
        }

        return encounterSO;
    }

}
#endif

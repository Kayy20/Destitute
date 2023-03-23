//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;
//using System;
//using UnityEditor;

//public static class EncounterSaveSystem
//{
//    private static List<EncounterSO> currentEncounters;
//    public static List<EncounterSO> CurrentEncounters
//    {
//        get
//        {
//            if (currentEncounters != null)
//            {
//                return currentEncounters;
//            }
//            else
//            {
//                currentEncounters = ConvertDataToScriptable(LoadEncounters());
//                return currentEncounters;
//            }
//        }
//    }

//    public static void SaveEncounterList(List<EncounterSO> encounterList)
//    {
//        //BinaryFormatter formatter = new BinaryFormatter();
//        string path = Application.dataPath + "/Resources/EncounterList.json";
//        //CD.Log($" SAVE PATH: {path}");
//        //FileStream stream = new FileStream(path, FileMode.Create);

//        EncounterData saveData = ConvertScriptableToData(encounterList);

//        string jsonData = JsonUtility.ToJson(saveData);

//        Debug.Log(jsonData);


//        File.WriteAllText(path, jsonData);

//        //formatter.Serialize(stream, saveData);
//        //stream.Close();

//        //AssetDatabase.ImportAsset(path);
//#if UNITY_EDITOR
//        AssetDatabase.Refresh();
//#endif

//    }

//    private static EncounterData ConvertScriptableToData(List<EncounterSO> encounterList)
//    {

//        List<EncounterSaveData> newList = new List<EncounterSaveData>();

//        foreach (EncounterSO encSO in encounterList)
//        {
//            EncounterSaveData encSD = new EncounterSaveData();
//            // Transfer all data from SD to SO


//            // Requirements //
//            // Iterate through each and assign the int values to the location in the answerReq
//            encSD.answerReq = new AnswerType[3 * 2];
//            encSD.numRequirements = new int[3];
//            int k = 0;
//            for (int i = 0; i < 3; i++) // Which Answer this is for
//            {
//                try
//                {
//                    encSD.numRequirements[i] = encSO.numRequirements[i];
//                }
//                catch
//                {
//                    encSD.numRequirements[i] = 1;
//                }
//                for (int j = 0; j < 2; j++) // Requirements for the answer
//                {
//                    try
//                    {
//                        encSD.answerReq[k] = encSO.answerReq[i][j];
//                    }
//                    catch
//                    {
//                        encSD.answerReq[k] = new AnswerType();
//                    }
//                    k++;
//                }
//            }
//            // End Requirements //

//            // Rewards //
//            encSD.answerRew = new AnswerType[3 * 3];
//            encSD.numRewards = new int[3];
//            k = 0;
//            for (int i = 0; i < 3; i++) // Which Answer this is for
//            {
//                encSD.numRewards[i] = encSO.numRewards[i];
//                for (int j = 0; j < 3; j++) // Requirements for the answer
//                {
//                    try
//                    {
//                        encSD.answerRew[k] = encSO.answerRew[i][j];
//                    }
//                    catch
//                    {
//                        encSD.answerRew[k] = new AnswerType();
//                    }
//                    k++;
//                }
//            }
//            // End Rewards //

//            //encSD.answerReq = encSO.answerReq;
//            //encSD.answerRew = encSO.answerRew;
//            encSD.numAnswers = encSO.numAnswers;
//            encSD.encounterAnswers = encSO.encounterAnswers;
//            encSD.encounterName = encSO.encounterName;
//            encSD.encounterProblem = encSO.encounterProblem;
//            encSD.where = encSO.where;
//            try
//            {
//                encSD.soundName = encSO.sound.soundName;
//            }
//            catch
//            {
//                encSD.soundName = "null";
//            }
            

//            newList.Add(encSD);

//        }

//        return new EncounterData(newList);
//    }

//    private static List<EncounterSO> ConvertDataToScriptable(List<EncounterSaveData> encounterList)
//    {

//        List<EncounterSO> newList = new List<EncounterSO>();

//        foreach (EncounterSaveData encSO in encounterList)
//        {
//            EncounterSO encSD = new EncounterSO();
//            // Transfer all data from SO to SD

//            // Requirements //
//            // Iterate through each and assign the int values to the location in the answerReq


//            encSD.answerReq = new List<List<AnswerType>>();
//            encSD.numRequirements = new int[3];
//            int k = 0;
//            for (int i = 0; i < 3; i++) // Which Answer this is for
//            {
//                try
//                {
//                    encSD.numRequirements[i] = encSO.numRequirements[i];
//                }
//                catch
//                {
//                    encSD.numRequirements[i] = 1;
//                }
//                encSD.answerReq[i] = new List<AnswerType>();
//                for (int j = 0; j < 2; j++) // Requirements for the answer
//                {
//                    //Debug.Log($"i: {i}, j: {j}");
//                    encSD.answerReq[i].Add(encSO.answerReq[k]);
//                    k++;
//                }
//            }
//            // End Requirements //

//            // Rewards //
//            encSD.answerRew = new List<List<AnswerType>>();
//            encSD.numRewards = new int[3];
//            k = 0;
//            for (int i = 0; i < 3; i++) // Which Answer this is for
//            {
//                encSD.numRewards[i] = encSO.numRewards[i];
//                encSD.answerRew[i] = new List<AnswerType>();
//                for (int j = 0; j < 3; j++) // Requirements for the answer
//                {
//                    encSD.answerRew[i].Add(encSO.answerRew[k]);
//                    k++;
//                }
//            }
//            // End Rewards //


//            //encSD.answerReq = encSO.answerReq;
//            //encSD.answerRew = encSO.answerRew;
//            encSD.numAnswers = encSO.numAnswers;
//            encSD.encounterAnswers = encSO.encounterAnswers;
//            encSD.encounterName = encSO.encounterName;
//            encSD.encounterProblem = encSO.encounterProblem;
//            encSD.where = encSO.where;
//            if (encSO.soundName != "null")
//                encSD.sound = Resources.Load<AudioSoundSO>($"Sounds/{encSO.soundName}");
//            else
//                encSD.sound = null;

//            newList.Add(encSD);

//        }

//        return newList;
//    }

//    public static List<EncounterSaveData> LoadEncounters()
//    {
//        string path = Application.dataPath + "/Resources/EncounterList.json";
//        CD.Log($" SAVE PATH: {path}");

//        if (File.Exists(path))
//        {

//            string jsonData = File.ReadAllText(path);

//            //Debug.Log(jsonAsset.ToString());

//            EncounterData saveData = JsonUtility.FromJson<EncounterData>(jsonData);

//            List<EncounterSaveData> loadedData = saveData.List;

//            //CD.Log(CD.Programmers.BEN, $"Loaded Data Length: {loadedData.Count}");

//            return loadedData;
//        }
//        else
//        {
//            CD.Log("NO ENCOUNTER FILE, MAKING BLANK ENCOUNTER FILE.", Color.red);
//            return new List<EncounterSaveData>();
//        }
        

//        //if (File.Exists(path))
//        //{
//        //    BinaryFormatter formatter = new BinaryFormatter();
//        //    FileStream stream = new FileStream(path, FileMode.Open);
//        //    EncounterData data = new EncounterData(new List<EncounterSaveData>());
//        //    try
//        //    {
//        //        data = formatter.Deserialize(stream) as EncounterData;
//        //    }
//        //    catch
//        //    { // file has nothing in it, so return empty data

//        //    }
//        //    stream.Close();

//        //    List<EncounterSaveData> returnList = data.List;

//        //    return returnList;

//        //}

//        //else
//        //{
//        //    CD.Log(CD.Programmers.BEN, "No Encounter File");

//        //    EncounterData data = Resources.Load<EncounterData>(path);

//        //    return new List<EncounterSaveData>();
//        //}

//    }


//    [Serializable]
//    public class EncounterData
//    {
//        public List<EncounterSaveData> List;

//        public EncounterData(List<EncounterSaveData> list)
//        {
//            List = list;
//        }
//    }

//    [Serializable]
//    public class EncounterSaveData
//    {

//        public string encounterName;
//        public string encounterProblem;
//        public AnswerType.WhereItHappens where;
//        public int numAnswers;
//        public string[] encounterAnswers;
//        public int[] numRequirements;
//        public int[] numRewards;
//        public AnswerType[] answerReq;
//        public AnswerType[] answerRew;
//        public string soundName;


//    }

//}



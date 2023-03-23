using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

// C:\Users\Danie\AppData\LocalLow\DefaultCompany\4F03 Capstone - Unnamed
public static class SaveSystem
{
    public static void SaveInventory(List<InventoryController.ItemSpotPair> pairs)
    {
        CD.Log(CD.Programmers.DANIEL,$"STARTING SAVE", Color.blue);
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/inventory.dtinv";
        //CD.Log($" SAVE PATH: {path}");
        FileStream stream = new FileStream(path, FileMode.Create);

        
        foreach (InventoryController.ItemSpotPair itemSpotPair in pairs)
        {
            CD.Log(CD.Programmers.DANIEL,$"SAVING: {itemSpotPair.itemID}", Color.blue);
        }
        
        //CD.Log(CD.Programmers.DANIEL, $"ITEMS BEING SAVED: {pairs.Count}", Color.blue);
        
        InventorySaveData saveData = new InventorySaveData(pairs);
        formatter.Serialize(stream, saveData);
        stream.Close();

    }
    
    public static void DeleteInventory()
    {
        CD.Log(CD.Programmers.DANIEL, "DELETING INV", Color.magenta);
        string path = Application.persistentDataPath + "/inventory.dtinv";
        //CD.Log(CD.Programmers.DANIEL, "DELETING INVENTORY", Color.red);
        
        File.Delete(path);

    }
    
    [Serializable]
    public class InventorySaveData
    {
        private List<InventoryController.ItemSpotPair> _pairs;

        public InventorySaveData(List<InventoryController.ItemSpotPair> pairs)
        {
            _pairs = pairs;
        }

        public List<InventoryController.ItemSpotPair> Pairs => _pairs;
    }

    public static InventorySaveData LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory.dtinv";
        //CD.Log($" SAVE PATH: {path}");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            InventorySaveData saveData = formatter.Deserialize(stream) as InventorySaveData;
            stream.Close();

            return saveData;
        }
        else
        {
            CD.Log(CD.Programmers.DANIEL, "no inventory file");
            return null;
        }
    }


    /* * * * * * * 
     * SCORE
     * * * * * * */

    [Serializable]
    public class ScoreSaveData
    {
        private int _score;

        public ScoreSaveData(int score)
        {
            _score = score;
        }

        public int Score => _score;
    }


    public static void SaveScore(int highscore)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/notHighScore.pkl";

        FileStream stream = new FileStream(path, FileMode.Create);


        ScoreSaveData saveData = new ScoreSaveData(highscore);
        formatter.Serialize(stream, saveData);
        stream.Close();

    }

    public static ScoreSaveData LoadScore()
    {
        string path = Application.persistentDataPath + "/notHighScore.pkl";
        //CD.Log($" SAVE PATH: {path}");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreSaveData saveData = formatter.Deserialize(stream) as ScoreSaveData;
            stream.Close();

            return saveData;
        }
        else
        {
            CD.Log(CD.Programmers.JUSTIN, "no Score");
            return new ScoreSaveData(0);
        }
    }

}

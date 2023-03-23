using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

// C:\Users\Danie\AppData\LocalLow\DefaultCompany\4F03 Capstone - Unnamed
public static class InventorySaveSystem
{
    public static void SaveInventory(List<InventoryController.ItemSpotPair> pairs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/inventory.dtinv";
        //CD.Log($" SAVE PATH: {path}");
        FileStream stream = new FileStream(path, FileMode.Create);

        /*
        foreach (InventoryController.ItemSpotPair itemSpotPair in pairs)
        {
            CD.Log(CD.Programmers.DANIEL,$"SAVING: {itemSpotPair.itemID}", Color.blue);
        }
        */
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

}

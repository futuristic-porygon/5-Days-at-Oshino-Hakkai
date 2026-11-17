using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private Chest[] chests;

    void Start()
    {      
        InitializeComponents();
        LoadGame();
    }
    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        chests = FindObjectsOfType<Chest>();
    }
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            chestSaveData = GetChestState()
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }
    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.chestID,
                isOpened = chest.isOpened,
            };
            chestStates.Add(chestSaveData);
        }

        return chestStates;
    }
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            inventoryController.SetInventoryItems(new List<InventorySaveData>());

            SaveGame();
        }

    }
        
    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.chestID);

            if(chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }

}

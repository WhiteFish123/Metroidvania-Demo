using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private FileDataHandler dataHandler;
    private List<ISaveable> allSaveables;
    private GameData gameData;
    [SerializeField] private string fileName="GameData.json";
    [SerializeField] private bool encrpyData=true;

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);

        gameData=new GameData();
        dataHandler=new FileDataHandler(Application.persistentDataPath,fileName,encrpyData);
        allSaveables=FindISaveables();
        yield return new WaitForSeconds(0.01f);
        LoadData();
    }

    private void LoadData()
    {
        gameData=dataHandler.LoadData();
        if(gameData==null)
        {
            Debug.Log("No data was found. A new game data will be created.");
            gameData=new GameData();
            return;
        }
        foreach(var saveable in allSaveables)
            saveable.LoadData(gameData);

    }

    private void SaveData()
    {
        foreach(var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    [ContextMenu("Delete save data")]
    public void DeleteData()
    {
        dataHandler=new FileDataHandler(Application.persistentDataPath,fileName,encrpyData);
        dataHandler.Delete();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None)
            .OfType<ISaveable>().ToList();
    }
}

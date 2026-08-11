using UnityEngine;
using System.IO;
using System;
public class FileDataHandler
{
    public string fullPath;
    private bool encrpyData;
    private string codeWord="WhiteFish";

    public FileDataHandler(string dataDirPath,string dataFileName,bool encrpyData)
    {
        fullPath=Path.Combine(dataDirPath,dataFileName);
        this.encrpyData=encrpyData;
    }

    public void SaveData(GameData gamedata)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToSave=JsonUtility.ToJson(gamedata,true);

            if(encrpyData)
                dataToSave=EncryptDecrypt(dataToSave);

            using (FileStream stream=new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer=new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: "+fullPath+"\n"+e);
        }
    }
    public GameData LoadData()
    {
        GameData loadData=null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad="";
                using (FileStream stream=new FileStream(fullPath,FileMode.Open))
                {
                    using (StreamReader reader=new StreamReader(stream))
                    {
                        dataToLoad=reader.ReadToEnd();
                    }
                }

                if(encrpyData)
                    dataToLoad=EncryptDecrypt(dataToLoad);

                loadData=JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: "+fullPath+"\n"+e);
            }
        }
        return loadData;
    }
    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData="";
        for(int i=0;i<data.Length;i++)
        {
            modifiedData+= (char)(data[i]^codeWord[i%codeWord.Length]);
        }
        return modifiedData;
    }
}

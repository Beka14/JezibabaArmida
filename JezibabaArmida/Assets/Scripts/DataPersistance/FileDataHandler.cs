using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public SavedData LoadData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        SavedData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(stream))
                    {
                        dataToLoad = r.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<SavedData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
        return loadedData;
    }

    public void SaveData(SavedData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data,true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(stream))
                {
                    w.Write(dataToStore);
                }
            }
        } 
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}

using UnityEngine;
using System;
using System.IO;

public class SaveDataFileWrite
{
    public string saveDataDirectoryPath = "";
    public string savedDataFileName = "";

    public bool CheckToSeeIfSaveDataFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, savedDataFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, savedDataFileName)); 
    }

    public void CreateNewSaveFile(SaveSlotData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, savedDataFileName);

        try
            {
            Debug.Log("Trying to save");
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("File Saved at " + savePath);
            string dataToStore = JsonUtility.ToJson(characterData, true);
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error Saving File at " + savePath + ". Game not saved \n" + e);
        }
    }

    public SaveSlotData LoadSaveFile()
    {
        SaveSlotData saveData = null;

        string loadPath = Path.Combine(saveDataDirectoryPath, savedDataFileName);

        if(File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                saveData = JsonUtility.FromJson<SaveSlotData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error Loading File at " + loadPath + ". Game not loaded \n" + e);
            }
        }
        return saveData;
    }
}

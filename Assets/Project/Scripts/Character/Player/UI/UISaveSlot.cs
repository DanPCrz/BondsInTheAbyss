using UnityEngine;
using TMPro;

public class UISaveSlot : MonoBehaviour
{
    SaveDataFileWrite saveDataFileWrite;

    [Header("Save Slot")]
    public SaveSlot saveSlot;

    [Header("Save Info")]
    public TextMeshProUGUI saveName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;

        if (saveSlot == SaveSlot.Slot1)
        {
            saveDataFileWrite.savedDataFileName = WorldGameSaveManager.instance.DecideSaveFileNameBasedOnSlotBeingUsed(saveSlot);

            if (saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
            {
                saveName.text = WorldGameSaveManager.instance.saveSlot1.saveName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (saveSlot == SaveSlot.Slot2)
        {
            saveDataFileWrite.savedDataFileName = WorldGameSaveManager.instance.DecideSaveFileNameBasedOnSlotBeingUsed(saveSlot);

            if (saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
            {
                saveName.text = WorldGameSaveManager.instance.saveSlot2.saveName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (saveSlot == SaveSlot.Slot3)
        {
            saveDataFileWrite.savedDataFileName = WorldGameSaveManager.instance.DecideSaveFileNameBasedOnSlotBeingUsed(saveSlot);

            if (saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
            {
                saveName.text = WorldGameSaveManager.instance.saveSlot3.saveName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void LoadGameFromSlot()
    {
        WorldGameSaveManager.instance.currentSaveSlotBeingUsed = saveSlot;
        WorldGameSaveManager.instance.LoadGame();
    }

    public void SelectCurrentSlot()
    {
        TitleScreenManager.instance.SelectSaveSlot(saveSlot);
    }
}

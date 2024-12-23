using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WorldGameSaveManager : MonoBehaviour
{
    public static WorldGameSaveManager instance;
    public PlayerManager player;

    [Header("Buttons")]
    [SerializeField] Button onlineButton;

    [Header("Save/Load")]
    [SerializeField] public bool saveGame;
    [SerializeField] public bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int WorldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveDataFileWrite saveDataFileWrite;

    [Header("Current Slot")]
    public SaveSlot currentSaveSlotBeingUsed;
    public SaveSlotData currentSlotData;
    private string saveFileName;

    [Header("Save Slots")]
    public SaveSlotData saveSlot1;
    public SaveSlotData saveSlot2;
    public SaveSlotData saveSlot3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllSaveSlots();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot saveSlot)
    {
        string fileName="";

        switch (saveSlot)
        {
            case SaveSlot.Slot1:
                fileName = "saveSlot1.json";
                break;
            case SaveSlot.Slot2:
                fileName = "saveSlot2.json";
                break;
            case SaveSlot.Slot3:
                fileName = "saveSlot3.json";
                break;
            default:
                break;
        }
        return fileName;
    }

    public void CheckForFreeSlot()
    {
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot1);
        if (!saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
        {
            currentSaveSlotBeingUsed = SaveSlot.Slot1;
            currentSlotData = new SaveSlotData();
            onlineButton.Select();
            return;
        }

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot2);
        if (!saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
        {
            currentSaveSlotBeingUsed = SaveSlot.Slot2;
            currentSlotData = new SaveSlotData();
            onlineButton.Select();
            return;
        }

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot3);
        if (!saveDataFileWrite.CheckToSeeIfSaveDataFileExists())
        {
            currentSaveSlotBeingUsed = SaveSlot.Slot3;
            currentSlotData = new SaveSlotData();
            onlineButton.Select();
            return;
        }

        TitleScreenManager.instance.NoFreeSaveSlotsPopup();
    }


    public void CreateNewGame()
    {
        //saveFileName = DecideSaveFileNameBasedOnSlotBeingUsed(currentSaveSlotBeingUsed);
        player.playerNetworkManager.vitality.Value = 10;
        player.playerNetworkManager.endurance.Value = 10;

        SaveGame();
        StartCoroutine(LoadWorldScene());
    }

    public void LoadGame() 
    {
        saveFileName = DecideSaveFileNameBasedOnSlotBeingUsed(currentSaveSlotBeingUsed);
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;
        saveDataFileWrite.savedDataFileName = saveFileName;
        currentSlotData = saveDataFileWrite.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        saveFileName = DecideSaveFileNameBasedOnSlotBeingUsed(currentSaveSlotBeingUsed);
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;
        saveDataFileWrite.savedDataFileName = saveFileName;
        player.SaveGameToCurrentSlot(ref currentSlotData);
        saveDataFileWrite.CreateNewSaveFile(currentSlotData);
    }

    public void DeleteGame(SaveSlot saveSlot)
    {
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;
        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(saveSlot);
        saveDataFileWrite.DeleteSaveFile();
    }

    public void LoadAllSaveSlots()
    {
        saveDataFileWrite = new SaveDataFileWrite();
        saveDataFileWrite.saveDataDirectoryPath = Application.persistentDataPath;

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot1);
        saveSlot1 = saveDataFileWrite.LoadSaveFile();

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot2);
        saveSlot2 = saveDataFileWrite.LoadSaveFile();

        saveDataFileWrite.savedDataFileName = DecideSaveFileNameBasedOnSlotBeingUsed(SaveSlot.Slot3);
        saveSlot3 = saveDataFileWrite.LoadSaveFile();
    
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        if (player == null)
        {
            Debug.LogError("PlayerManager not found! Unable to load game.");
            yield break;   
        }
        player.LoadGameFromCurrentSlot(ref currentSlotData);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return WorldSceneIndex;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldGameSaveManager : MonoBehaviour
{
    public static WorldGameSaveManager instance;
    [SerializeField] PlayerManager player;

    [Header("Save/Load")]
    [SerializeField] public bool saveGame;
    [SerializeField] public bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int WorldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveDataFileWrite saveFileDataWrite;

    [Header("Current Slot")]
    public SaveSlot currentSaveSlotBeingUsed;
    public CharacterSaveData currentSlotData;
    private string saveFileName;

    [Header("Save Slots")]
    public CharacterSaveData saveSlot1;
    public CharacterSaveData saveSlot2;
    public CharacterSaveData saveSlot3;

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

    private void DecideSaveFileNameBasedOnSlotBeingUsed()
    {
        switch (currentSaveSlotBeingUsed)
        {
            case SaveSlot.Slot1:
                currentSlotData = saveSlot1;
                saveFileName = "saveSlot1";
                break;
            case SaveSlot.Slot2:
                currentSlotData = saveSlot2;
                saveFileName = "saveSlot2";
                break;
            case SaveSlot.Slot3:
                currentSlotData = saveSlot3;
                saveFileName = "saveSlot3";
                break;
        }
    }

    public void CreateNewGame()
    {
        DecideSaveFileNameBasedOnSlotBeingUsed();
        currentSlotData = new CharacterSaveData();
    }

    public void LoadGame() 
    {
        DecideSaveFileNameBasedOnSlotBeingUsed();
        saveFileDataWrite = new SaveDataFileWrite();
        saveFileDataWrite.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWrite.savedDataFileName = saveFileName;
        currentSlotData = saveFileDataWrite.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        DecideSaveFileNameBasedOnSlotBeingUsed();
        saveFileDataWrite = new SaveDataFileWrite();
        saveFileDataWrite.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWrite.savedDataFileName = saveFileName;
        player.SaveGameToCurrentSlot(ref currentSlotData);
        saveFileDataWrite.CreateNewSaveFile(currentSlotData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return WorldSceneIndex;
    }
}
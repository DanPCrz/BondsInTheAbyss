using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    [Header("Pop Ups")]
    [SerializeField] GameObject noFreeSaveSlotsPopup;
    [SerializeField] GameObject deleteSaveSlotPopup;

    [Header("Menus")]
    [SerializeField] GameObject modesMenu;

    [Header("Buttons")]
    [SerializeField] Button noFreeSaveSlotsButton;
    [SerializeField] Button deleteSaveSlotButton;

    [Header("Save Slots")]
    public SaveSlot currenSaveSlotSelected = SaveSlot.NO_SLOT;

    public void Awake()
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
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        WorldGameSaveManager.instance.CreateNewGame();
    }

    public void NoFreeSaveSlotsPopup()
    {
        modesMenu.SetActive(false);
        noFreeSaveSlotsPopup.SetActive(true);
        noFreeSaveSlotsButton.Select();
    }

    public void SelectSaveSlot(SaveSlot saveSlot)
    {
        currenSaveSlotSelected = saveSlot;
    }

    public void SelectNoSlot()
    {
        currenSaveSlotSelected = SaveSlot.NO_SLOT;
    }

    public void AttemptDeleteSaveSlot()
    {
        if (currenSaveSlotSelected != SaveSlot.NO_SLOT)
        {
            deleteSaveSlotPopup.SetActive(true);
            deleteSaveSlotButton.Select();
        }
    }

    public void DeleteSaveSlot()
    {
        deleteSaveSlotPopup.SetActive(false);
        WorldGameSaveManager.instance.DeleteGame(currenSaveSlotSelected);
        WorldGameSaveManager.instance.LoadAllSaveSlots();
    }
}

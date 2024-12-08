using UnityEngine;
using Unity.Netcode;
using TMPro;


public class TittleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI connectionStatusText;
    public GameObject newPrefab;
    public GameObject joinedButton;
    public GameObject readyButton;

    public void StartNetworkAsHost()
    {
        //CharacterNetWorkManager.instance.ListenCallback();
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        StartCoroutine(WorldGameSaveManager.instance.LoadNewGame());
    }
}

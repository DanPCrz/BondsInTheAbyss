using UnityEngine;
using Unity.Netcode;
using TMPro;


public class TittleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI connectionStatusText;
    public GameObject joinedButton;
    public GameObject readyButton;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            connectionStatusText.text = "Second player joined!";
            joinedButton.SetActive(true);
        }
        else
        {
            connectionStatusText.text = "Waiting for second player to join...";
        }
    }

    public void StartNewGame()
    {
        StartCoroutine(WorldGameSaveManager.instance.LoadNewGame());
    }

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNetworkAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}

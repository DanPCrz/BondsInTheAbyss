using UnityEngine;
using Unity.Netcode;
using System;

public class CharacterNetWorkManager : NetworkBehaviour
{
    public static CharacterNetWorkManager instance;

    public NetworkList<PlayerStruct> playerStruct;
    public EventHandler OnPlayerDataNetworkListChanged;

    CharacterManager character;
    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
                                                      NetworkVariableReadPermission.Everyone, 
                                                      NetworkVariableWritePermission.Owner);

    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, 
                                                         NetworkVariableReadPermission.Everyone, 
                                                         NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animation")]
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, 
                                                                NetworkVariableReadPermission.Everyone, 
                                                                NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0,
                                                              NetworkVariableReadPermission.Everyone, 
                                                              NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, 
                                                      NetworkVariableReadPermission.Everyone, 
                                                      NetworkVariableWritePermission.Owner);

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        character = GetComponent<CharacterManager>();

        playerStruct = new NetworkList<PlayerStruct>();
        //playerStruct.OnListChanged += PlayerStruct_OnListChanged;
    }
    //private void PlayerStruct_OnListChanged(NetworkListEvent<PlayerStruct> changeEvent)
    //{
    //    OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    //}

    //public void ListenCallback()
    //{
    //    NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    //}

    //public void NetworkManager_OnClientConnectedCallback(ulong clientId)
    //{
    //    playerStruct.Add(new PlayerStruct { clientID = clientId, });
    //}

    [ServerRpc]
    public void NotifyServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayActionAnimationForEveryoneClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayActionAnimationForEveryoneClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (IsClient)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }
    }

    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        character.animator.CrossFade(animationID, 0.2f);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerStruct.Count;
    }
}

using UnityEngine;
using Unity.Netcode;

public class CharacterNetWorkManager : NetworkBehaviour
{



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
        character = GetComponent<CharacterManager>();
    }

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
}

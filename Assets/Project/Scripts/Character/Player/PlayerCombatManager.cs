using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    
    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        if (player.IsOwner)
        {
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRPC(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }
    }
}
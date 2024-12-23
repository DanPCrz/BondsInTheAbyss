using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>("Player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void SetCharcterActionHand(bool rightHandedAction)
    {
        if (rightHandedAction)
        {
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }
        else
        {
            isUsingRightHand.Value = false;
            isUsingLeftHand.Value = true;
        }
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerHUDManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerHUDManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }

    public void OnCurrentRightHandWeaponIDChange(int oldWeaponID, int newWeaponID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newWeaponID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldWeaponID, int newWeaponID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newWeaponID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldWeaponID, int newWeaponID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newWeaponID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
    }

    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRPC(ulong clientID, int actionID, int weaponID)
    {
        if (IsServer)
        {
            NotifyTheServerOfWeaponActionClientRPC(clientID, actionID, weaponID);
        }
    }

    [ClientRpc]
    private void NotifyTheServerOfWeaponActionClientRPC(ulong clientID, int actionID, int weaponID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformWeaponBasedAction(actionID, weaponID);
        }
    }

    private void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

        if (weaponAction != null)
        {
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponItemByID(weaponID));
        }
        else 
        {
            Debug.LogError("Weapon Action ID: " + actionID + " does not exist in the World Action Manager.");
        }
    }
}
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

    public virtual void DrainStaminaBasedOnAttack()
    {
        if (!player.IsOwner || currentWeaponBeingUsed == null)
            return; 

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack1:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttack1StaminaCostMultiplier;
                break;
            default:
                break;
        }
        Debug.Log("Stamina deducted: " + staminaDeducted);
        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);

    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
        }
    }
}
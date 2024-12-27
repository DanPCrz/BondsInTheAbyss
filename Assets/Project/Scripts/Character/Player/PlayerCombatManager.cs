using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainWeapon;
    //public bool canComboWithOffHandWeapon;

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
            case AttackType.LightAttack2:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttack2StaminaCostMultiplier;
                break;
            case AttackType.LightAttack3:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttack3StaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack1:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttack1StaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack2:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttack2StaminaCostMultiplier;
                break;
            case AttackType.ChargedAttack1:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttack1StaminaCostMultiplier;
                break;
            case AttackType.ChargedAttack2:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttack2StaminaCostMultiplier;
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
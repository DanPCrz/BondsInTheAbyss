using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Macuahuitl Light Attack Action")]
public class MacuahuitlLightAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformMacuahuitlLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformMacuahuitlLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainWeapon = false;

            if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Macuahuitl Light Attack 1")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack2, "Macuahuitl Light Attack 2", true);
            }
            else if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Macuahuitl Light Attack 2")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack3, "Macuahuitl Light Attack 3", true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Macuahuitl Light Attack 1", true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Macuahuitl Light Attack 1", true);
        }
    }
}

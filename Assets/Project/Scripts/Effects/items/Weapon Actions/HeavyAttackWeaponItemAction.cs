using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainWeapon = false;

            if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Heavy Attack Charge 1")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack2, "Heavy Attack Charge 2", true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack1, "Heavy Attack Charge 1", true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack1, "Heavy Attack Charge 1", true);
        }
    }
}

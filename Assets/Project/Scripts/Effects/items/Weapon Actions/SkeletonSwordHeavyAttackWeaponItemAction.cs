using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Skeleton Sword Heavy Attack Action")]
public class SkeletonSwordHeavyAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformSkeletonSwordHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformSkeletonSwordHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainWeapon = false;

            if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Skeleton Sword Heavy Attack Charge 1")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack2, "Skeleton Sword Heavy Attack Charge 2", true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack1, "Skeleton Sword Heavy Attack Charge 1", true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.HeavyAttack1, "Skeleton Sword Heavy Attack Charge 1", true);
        }
    }
}

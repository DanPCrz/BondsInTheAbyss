using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Tepoztopillis Heavy Attack Action")]
public class TepoztopillisHeavyAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformTepoztopillisHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformTepoztopillisHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainWeapon = false;

            if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Tepoztopillis Heavy Attack 1")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack2, "Tepoztopillis Heavy Attack 2", true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Tepoztopillis Heavy Attack 1", true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Tepoztopillis Heavy Attack 1", true);
        }
    }
}

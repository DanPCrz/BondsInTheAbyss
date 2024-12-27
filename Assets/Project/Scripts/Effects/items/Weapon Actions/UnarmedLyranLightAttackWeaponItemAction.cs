using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Unarmed Lyran Light Attack Action")]

public class UnarmedLyranLightAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformUnarmedLyranLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformUnarmedLyranLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainWeapon = false;

            if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == "Unarmed Lyran Light Attack 1")
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack2, "Unarmed Lyran Light Attack 2", true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Unarmed Lyran Light Attack 1", true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Unarmed Lyran Light Attack 1", true);
        }
    }
}

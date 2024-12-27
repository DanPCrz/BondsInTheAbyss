using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Unarmed Lyran Heavy Attack Action")]
public class UnarmedLyranHeavyAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformUnarmedLyranHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformUnarmedLyranHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Unarmed Lyran Heavy Attack Charge", true);
        }
    }
}

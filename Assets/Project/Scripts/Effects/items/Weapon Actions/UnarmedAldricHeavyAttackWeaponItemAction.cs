using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Unarmed Aldric Heavy Attack Action")]
public class UnarmedAldricHeavyAttackWeaponItemAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner || playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        PerformUnarmedAldricHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformUnarmedAldricHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayTargetAttackAnimation(AttackType.LightAttack1, "Unarmed Aldric Heavy Attack Charge", true);
        }
    }
}

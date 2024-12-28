using UnityEditor;
using UnityEngine;

public class PlayerAnimationManager : CharacterAnimationManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnAnimatorMove()
    {
        if (player.playerAnimationManager.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    public override void EnableCanDoCombo()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainWeapon = true;
        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainWeapon = false;
        //player.playerCombatManager.canComboWithOffHandWeapon = false;
    }
}

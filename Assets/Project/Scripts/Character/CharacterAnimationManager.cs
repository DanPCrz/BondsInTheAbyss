using UnityEngine;
using Unity.Netcode;

public class CharacterAnimationManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting, bool isDowned)
    {
        if (isSprinting)
        {
            verticalMovement = 2;
        }

        character.animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetAnimation(string targetAnimation, bool isPerformingAction, 
                                            bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        character.characterNetworkManager.NotifyActionAnimationToServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}

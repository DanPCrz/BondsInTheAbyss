using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;

public class CharacterAnimationManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        character = GetComponent<CharacterManager>();
    }
    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
    {
        character.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, 
                                            bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        character.characterNetWorkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}

using UnityEngine;

public class AICharacterAnimationManager : CharacterAnimationManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {
        if (aiCharacter.IsOwner)
        {
            if (aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.rotation = aiCharacter.animator.deltaRotation;
        }
        else
        {
            if (!aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.position = Vector3.SmoothDamp(aiCharacter.transform.position,
                                                                aiCharacter.animator.rootPosition,
                                                                ref aiCharacter.characterNetworkManager.networkPositionVelocity,
                                                                aiCharacter.characterNetworkManager.networkPositionSmoothTime);

            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
    }
}

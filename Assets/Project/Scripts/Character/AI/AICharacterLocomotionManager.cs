using UnityEngine;
using UnityEngine.TextCore.Text;

public class AICharacterLocomotionManager : CharacterLocomotionManager
{
    public AICharacterManager aiCharacter;

    override protected void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }
}

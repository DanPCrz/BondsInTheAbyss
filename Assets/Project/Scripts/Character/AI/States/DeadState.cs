using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Dead")]
public class DeadState : AIState
{
    //private bool hasPerformedDeath = false;
    //public override AIState Tick(AICharacterManager aiCharacter)
    //{
    //    aiCharacter.aiCharacterCombatManager.currentTarget = null;
    //    if (!hasPerformedDeath)
    //    {
    //        hasPerformedDeath = true;
    //        PerformDeath(aiCharacter);
    //    }
    //    return SwitchState(aiCharacter, aiCharacter.dead);
    //}
    //protected void PerformDeath(AICharacterManager aiCharacter)
    //{
    //    hasPerformedDeath = true;
    //    aiCharacter.aiCharacterAnimationManager.PlayTargetAnimation("Death", true);

    //}
}

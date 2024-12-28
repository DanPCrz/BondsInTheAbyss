using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackState : AIState
{
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flags")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    //[Header("Pivot After Attack")]
    //[SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null || aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            return SwitchState(aiCharacter, aiCharacter.idle);

        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhileAttacking(aiCharacter);

        aiCharacter.characterAnimationManager.UpdateAnimatorMovementParameters(0, 0, false, false);

        if (willPerformCombo && !hasPerformedCombo)
        {
            if (currentAttack.comboAction)
            {
                //hasPerformedCombo = true;
                //currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasPerformedAttack)
        {
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0 || aiCharacter.isPerformingAction)
                return this;

            PerformAttack(aiCharacter);
            return this;
        }

        //if (pivotAfterAttack)
        //{
        //    aiCharacter.aiCharacterLocomotionManager.PivotTowardsAgent(aiCharacter);
        //}

        return SwitchState(aiCharacter, aiCharacter.combatStance);
        
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);
        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);
        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}

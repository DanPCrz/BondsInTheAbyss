using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;
    protected List<AICharacterAttackAction> potentialAttacks;
    private AICharacterAttackAction choosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] public float maxEngagementDistance = 5;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;            

        
            if (aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            //if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            //{
            //    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
            //    {
            //        aiCharacter.aiCharacterLocomotionManager.PivotTowardsAgent(aiCharacter);
            //        return this;
            //    }

            //}

            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maxEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.characterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget ||
                potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget )
                continue;

            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle ||
                potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }
        if (potentialAttacks.Count <= 0)
            return;
        
        var totalWeight = 0;
        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeight = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;
            if (randomWeight <= processedWeight)
            {
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int chance)
    { 
        bool outcomeWillBePerformed = false;
        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < chance)
        {
            outcomeWillBePerformed = true;
        }

        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);
        hasAttack = false;
        hasRolledForComboChance = false;
    }
}

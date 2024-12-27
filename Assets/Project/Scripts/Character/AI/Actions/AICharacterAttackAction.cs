using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attack Action")]
    [SerializeField] private string attackAnimation;

    [Header("Combo Action")]
    public AICharacterAttackAction comboAction;

    [Header("Action Values")]
    [SerializeField] AttackType attackType;
    public int attackWeight = 50;
    public float actionRecoveryTime = 1.5f;
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;

    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimationManager.PlayTargetAttackAnimation(attackType, attackAnimation, true);
    }   
}

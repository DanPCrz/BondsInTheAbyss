using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Recovery Timer")]
    public float actionRecoveryTimer = 0;

    [Header("Target Info")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -40;
    [SerializeField] float maximumDetectionAngle = 40;

    [Header("Attack Variables")]
    public float attackRotationSpeed = 15;

    protected override void Awake()
    {
        base.Awake();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());
        
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter == null || targetCharacter == aiCharacter || targetCharacter.isDead.Value)
                continue;

            if (WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                if (angleOfPotentialTarget < maximumDetectionAngle && angleOfPotentialTarget > minimumDetectionAngle)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.instance.GetEnvironmentLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, 
                            targetCharacter.characterCombatManager.lockOnTransform.position, Color.red);
                        Debug.Log("Blocked");
                    }
                    else
                    {
                        //targetDirection = targetCharacter.transform.position - transform.position;
                        //this.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        //PivotTowardsTarget(aiCharacter);   
                    }
                }
            }
                
        }   
    }

    //private void PivotTowardsTarget(AICharacterManager aiCharacter)
    //{
    //    if (aiCharacter.isPerformingAction)
    //        return;

    //    if (viewableAngle >= 20 && viewableAngle <= 60)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 45", true);
    //    }
    //    else if (viewableAngle <= -20 && viewableAngle >= -60)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 45", true);
    //    }
    //    if (viewableAngle >= 61 && viewableAngle <= 110)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 90", true);
    //    }
    //    else if (viewableAngle <= -61 && viewableAngle >= -110)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 90", true);
    //    }
    //    if (viewableAngle >= 111 && viewableAngle <= 145)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 135   ", true);
    //    }
    //    else if (viewableAngle <= -111 && viewableAngle >= -145)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 135", true);
    //    }
    //    if (viewableAngle >= 146 && viewableAngle <= 180)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 180", true);
    //    }
    //    else if (viewableAngle <= -146 && viewableAngle >= -180)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 180", true);
    //    }
    //}   

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhileAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null || !aiCharacter.aiCharacterLocomotionManager.canRotate || !aiCharacter.isPerformingAction)
            return;

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }
}

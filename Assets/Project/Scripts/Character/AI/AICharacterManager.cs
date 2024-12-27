using UnityEngine;
using UnityEngine.AI;
public class AICharacterManager : CharacterManager
{
    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterAnimationManager aiCharacterAnimationManager;

    [Header("Nav Mesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Current State")]
    [SerializeField] AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;

    protected override void Awake()
    {
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterAnimationManager = GetComponent<AICharacterAnimationManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);

        currentState = idle;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsOwner)
        {
            ProcessStateMachine();
        }
    }

    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);
        if (nextState != null)
        {
            currentState = nextState;
        }

        //navMeshAgent.transform.position = Vector3.zero;
        //navMeshAgent.transform.rotation = Quaternion.identity;

        if (aiCharacterCombatManager.currentTarget != null)
        {
        //    aiCharacterCombatManager.targetDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
        //    aiCharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
            aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
        }

        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                aiCharacterNetworkManager.isMoving.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
        else
        {
            aiCharacterNetworkManager.isMoving.Value = false;
        }
    }
}

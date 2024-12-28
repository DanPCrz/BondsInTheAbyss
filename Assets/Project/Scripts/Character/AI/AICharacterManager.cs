using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AICharacterManager : CharacterManager
{
    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterAnimationManager aiCharacterAnimationManager;
    [HideInInspector] public AICharacterStatsManager aiCharacterStatsManager;
    [HideInInspector] public CharacterController characterControllerAttached;

    [Header("Nav Mesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Current State")]
    [SerializeField] AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attack;
    //public DeadState dead;

    protected override void Awake()
    {
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterAnimationManager = GetComponent<AICharacterAnimationManager>();
        aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        characterControllerAttached = GetComponent<CharacterController>();

        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);
        combatStance = Instantiate(combatStance);
        attack = Instantiate(attack);
        //dead = Instantiate(dead);

        currentState = idle;

    }

    protected override void Update()
    {
        base.Update();
        aiCharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsOwner)
        {
            ProcessStateMachine();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            aiCharacterNetworkManager.vitality.OnValueChanged += aiCharacterNetworkManager.SetNewMaxHealthValue;
        }
            aiCharacterNetworkManager.currentHealth.OnValueChanged += aiCharacterNetworkManager.CheckHP;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        aiCharacterNetworkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.CheckHP;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (characterUIManager.hasFloatingHPBar)
            characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (characterUIManager.hasFloatingHPBar)
            characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
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

    public override IEnumerator ProcessDeathEvent(bool mannualySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            aiCharacterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            if (!mannualySelectDeathAnimation)
            {
                aiCharacterAnimationManager.PlayTargetAnimation("Death", true);
            }
            characterControllerAttached.height = 0;
        }

        yield return new WaitForSeconds(5);
    }
}

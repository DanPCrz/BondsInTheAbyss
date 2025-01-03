using UnityEngine;
using Unity.Netcode;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class CharacterManager : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> isDowned = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector] public NetworkObject networkObject;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimationManager characterAnimationManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
    [HideInInspector] public CharacterUIManager characterUIManager;

    [Header ("Character Group")]
    public CharacterGroup characterGroup;

    [Header("Flags")]
    public bool isPerformingAction = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        networkObject = GetComponent<NetworkObject>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimationManager = GetComponent<CharacterAnimationManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterUIManager = GetComponent<CharacterUIManager>();
    }

    protected virtual void Start()
    {
        IgnoreMyownColliders();
    }

    protected virtual void Update()
    {
        animator.SetBool("IsGrounded", characterLocomotionManager.isGrounded);
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                                                    characterNetworkManager.networkPosition.Value, 
                                                    ref characterNetworkManager.networkPositionVelocity, 
                                                    characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                  characterNetworkManager.networkRotation.Value, 
                                                  characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void OnEnable()
    {
        
    }

    public virtual void OnDisable()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        animator.SetBool("IsMoving", characterNetworkManager.isMoving.Value);
        characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
    }

    public virtual IEnumerator ProcessDeathEvent(bool mannualySelectDeathAnimation = false)
    {
        yield return null;
    }

    public virtual void ReviveCharacter()
    {

    }

    protected virtual void IgnoreMyownColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();

        foreach (var collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }

        ignoreColliders.Add(characterControllerCollider);

        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }
}

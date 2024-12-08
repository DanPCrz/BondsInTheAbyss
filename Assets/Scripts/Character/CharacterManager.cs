using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterNetWorkManager characterNetWorkManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetWorkManager = GetComponent<CharacterNetWorkManager>();
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            characterNetWorkManager.networkPosition.Value = transform.position;
            characterNetWorkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                                                    characterNetWorkManager.networkPosition.Value, 
                                                    ref characterNetWorkManager.networkPositionVelocity, 
                                                    characterNetWorkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                   characterNetWorkManager.networkRotation.Value, 
                                                   characterNetWorkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }
}

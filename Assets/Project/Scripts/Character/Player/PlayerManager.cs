using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimationManager playerAnimationManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;
        
        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
            
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }
}
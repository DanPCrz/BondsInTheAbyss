using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotionManager : CharacterLocomotionManger
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Variables")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float rotationSpeed = 15;

    [Header("Dodge Variables")]
    private Vector3 rollDirection;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetWorkManager.verticalMovement.Value = verticalMovement;
            player.characterNetWorkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetWorkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetWorkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetWorkManager.horizontalMovement.Value;
            moveAmount = player.characterNetWorkManager.moveAmount.Value;

            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;

        GetMovementValues();

        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;
            
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptDodge()
    {
        if (player.isPerformingAction)
            return;

        if (moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimationManager.PlayTargetAnimation("Roll Forward", true);
        }
        else
        {
            player.playerAnimationManager.PlayTargetAnimation("Backstep", true);
        }

    }
}

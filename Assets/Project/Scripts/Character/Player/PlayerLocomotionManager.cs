using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Variables")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float SprintingSpeed = 7f;
    [SerializeField] float runningSpeed = 4.5f;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingStaminaCost = 4;

    [Header("Jump Variables")]
    [SerializeField] int jumpStaminaCost = 10;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 2;
    private Vector3 jumpDirection;

    [Header("Dodge Variables")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 20;

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
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
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

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * SprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }        
    }

    private void HandleJumpingMovement()
    {
        if (player.isJumping)
        {
           player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection;
            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
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

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
        else
        {
            if (player.playerNetworkManager.currentStamina.Value < player.playerNetworkManager.maxStamina.Value)
            {
                player.playerNetworkManager.currentStamina.Value += 1;
            }
        }
    }

    public void AttemptDodge()
    {
        if (player.isPerformingAction || player.playerNetworkManager.currentStamina.Value <= 0)
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

        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }

    public void AttemptJump()
    {
        if (player.isPerformingAction || player.playerNetworkManager.currentStamina.Value <= 0 || player.isJumping || !player.isGrounded)
            return;

        player.playerAnimationManager.PlayTargetAnimation("Jump Start", false);
        player.isJumping = true;
        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.y = 0;

        if (jumpDirection != Vector3.zero)
        {
            if (player.playerNetworkManager.isSprinting.Value)
            {
                jumpDirection *= 1;
            }
            else if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                jumpDirection *= 0.5f;
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                jumpDirection *= 0.25f;
            }
        }
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}

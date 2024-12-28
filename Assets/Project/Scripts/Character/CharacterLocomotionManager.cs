using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check and Jumping")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] float checkSphereRadius = 0.2f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -30;
    [SerializeField] protected float fallStartYVelocity = -7.5f;
    [SerializeField] protected float gravity = -20f;
    protected bool fallingVelocitySet = false;
    protected float airTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.characterLocomotionManager.isGrounded)
        {
            if (yVelocity.y <0)
            {
                airTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!character.characterNetworkManager.isJumping.Value && !fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }
            airTimer += Time.deltaTime;
            character.animator.SetFloat("Air Time", airTimer);
            yVelocity.y += gravity * Time.deltaTime;
        }
        character.characterController.Move(yVelocity * Time.deltaTime);

    }

    protected virtual void HandleGroundCheck()
    {
        character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, checkSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(character.transform.position, checkSphereRadius);
    }

    public void EnableCanRotate()
    {
        canRotate = true;
    }

    public void DisableCanRotate()
    {
        canRotate = false;
    }
}

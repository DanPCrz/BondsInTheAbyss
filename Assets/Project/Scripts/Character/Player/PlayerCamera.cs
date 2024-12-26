using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 0.1f;
    [SerializeField] float leftRightRotationSpeed = 220;
    [SerializeField] float upDownRotationSpeed = 150;
    [SerializeField] float minPivot = -30;
    [SerializeField] float maxPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftRightLookAngle;
    [SerializeField] float upDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 15;
    [SerializeField] float minimumViewableAngle = -60;
    [SerializeField] float maximumViewableAngle = 60;
    [SerializeField] float lockOnFollowSpeed = 0.2f;
    [SerializeField] float unlockedCameraHeight = 1.5f;
    [SerializeField] float lockedCameraHeight = 4;
    [SerializeField] float lockOnHeightSpeed = 1;
    private Coroutine cameraLockOnHeightCoroutine;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager rightLockOnTarget;
    public CharacterManager leftLockOnTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
            HandleLocatingLockOnTargets();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, 
                                                          player.transform.position, 
                                                          ref cameraVelocity, 
                                                          cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnFollowSpeed);

            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotationDirection.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDirection);
            cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnFollowSpeed);

            leftRightLookAngle = transform.eulerAngles.y;
            upDownLookAngle = transform.eulerAngles.x;
        }
        else
        {
            leftRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftRightRotationSpeed * Time.deltaTime;
            upDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upDownRotationSpeed * Time.deltaTime;
            upDownLookAngle = Mathf.Clamp(upDownLookAngle, minPivot, maxPivot);

            Vector3 cameraRotation;
            Quaternion targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.y = leftRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = upDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, 
                               cameraCollisionRadius, 
                               direction, out hit, 
                               Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    public void HandleLocatingLockOnTargets()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
            if (lockOnTarget != null) 
            {
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                if (lockOnTarget.isDead.Value || lockOnTarget.transform.root == player.transform.root)
                    continue;

                if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                        lockOnTarget.characterCombatManager.lockOnTransform.position,
                        out hit, WorldUtilityManager.instance.GetEnvironmentLayers()))
                    {
                        continue;
                    }
                    else
                    {
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            if (availableTargets[k] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromRightTarget = relativeEnemyPosition.x;
                    var distanceFromLeftTarget = relativeEnemyPosition.x;

                    if (availableTargets[k] == player.playerCombatManager.currentTarget)
                        continue;

                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else 
            {
                ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
            }
        }
    }

    public void ClearLockOnTargets()
    {
        nearestLockOnTarget = null;
        rightLockOnTarget = null;
        leftLockOnTarget = null;
        availableTargets.Clear();
    }

    public void SetLockCameraHeight()
    {
        if (cameraLockOnHeightCoroutine != null)
        {
            StopCoroutine(cameraLockOnHeightCoroutine);
        }
        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }

        ClearLockOnTargets();
        HandleLocatingLockOnTargets();

        if (nearestLockOnTarget != null)
        {
            player.playerCombatManager.SetTarget(nearestLockOnTarget);
            player.playerNetworkManager.isLockedOn.Value = true;
        }

        yield return null;
    }

    private IEnumerator SetCameraHeight()
    {
        float duration = 1;
        float timer = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
        Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                                                                                      newLockedCameraHeight, ref velocity, lockOnHeightSpeed);
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnHeightSpeed);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                                                                                      newUnlockedCameraHeight, ref velocity, lockOnHeightSpeed);
                }
            }
            else
            {
            }
            yield return null;
        }

        if (player != null)
        {
            if (player.playerCombatManager.currentTarget != null)
            {
                cameraPivotTransform.transform.localPosition = newLockedCameraHeight;

                cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
            }
        }
        yield return null;
    }
}

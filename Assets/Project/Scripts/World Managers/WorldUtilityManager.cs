using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask environmentLayers;

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

    public LayerMask GetCharacterLayers()
    {
        return characterLayers;
    }

    public LayerMask GetEnvironmentLayers() 
    {
        return environmentLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attacker, CharacterGroup target)
    {
        if (attacker == CharacterGroup.Player)
        {
            switch (target)
            {
                case CharacterGroup.Player:
                    return false;
                case CharacterGroup.AI:
                    return true;
                default:
                    break;
            }
        }
        else if (attacker == CharacterGroup.AI)
        {
            switch (target)
            {
                case CharacterGroup.Player:
                    return true;
                case CharacterGroup.AI:
                    return false;
                default:
                    break;
            }
        }
        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetDirection)
    {
        targetDirection.y = 0;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);

        if (cross.y < 0)
        {
            viewableAngle = -viewableAngle;
        }

        return viewableAngle;
    }
}

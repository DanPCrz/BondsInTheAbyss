using UnityEngine;

public class UtilityDestroyAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilDestroyed = 5;

    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }
}

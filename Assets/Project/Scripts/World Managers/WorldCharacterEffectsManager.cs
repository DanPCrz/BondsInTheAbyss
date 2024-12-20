using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;

    [SerializeField] List<InstantCharacterEffect> instantCharacterEffects;

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

        GenerateEffectID();
    }

    private void GenerateEffectID()
    {
        for (int i = 0; i < instantCharacterEffects.Count; i++)
        {
            instantCharacterEffects[i].instantEffectID = i;
        }
    }
}

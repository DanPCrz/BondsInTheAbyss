using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessInstantEffect(CharacterManager character)
    {

    }
}

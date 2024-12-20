using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Layer")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
            effect.staminaDamage = 10;
            ProcessInstantEffect(effect);
        }
    }
}

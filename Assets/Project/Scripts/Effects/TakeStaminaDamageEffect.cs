using UnityEngine;


[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessInstantEffect(CharacterManager character)
    {
        CalculateStaminaDamge(character);
    }

    private void CalculateStaminaDamge(CharacterManager character)
    {
        if (character.IsOwner)
        {
            Debug.Log("Character is taking: "+ staminaDamage +" Stamina Damage");
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}

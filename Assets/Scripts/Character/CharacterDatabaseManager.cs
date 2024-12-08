
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Characters/Database")]
public class CharacterDatabaseManager : ScriptableObject
{
    [SerializeField] private CharacterAssetData[] characters = new CharacterAssetData[0];

    public CharacterAssetData[] GetAllCharacters() => characters;

    public CharacterAssetData GetCharacterById(int id)
    {
        foreach (var character in characters)
        {
            if (character.Id == id)
            {
                return character;
            }
        }

        return null;
    }

    public bool IsValidCharacterId(int id)
    {
        return characters.Any(x => x.Id == id);
    }
}

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Debug Menu")]
    [SerializeField] bool spawnCharacters = false;
    [SerializeField] bool despawnCharacters = false;

    [Header("Character Prefabs")]
    public GameObject[] characterPrefabs;
    public List<GameObject> spawnedCharacters;  

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
        if (NetworkManager.Singleton.IsServer)
        {
            StartCoroutine(WaitForSceneLoadThenSpawnCharacters());
        }
    }

    private void Update()
    {
        if (spawnCharacters)
        {
            spawnCharacters = false;
            SpawnAllCharacters();
        }

        if (despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }
    }

    private IEnumerator WaitForSceneLoadThenSpawnCharacters()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }
        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach (var character in characterPrefabs)
        {
            GameObject instantiatedCharacter = Instantiate(character);
            instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedCharacters.Add(instantiatedCharacter);
        }
    }

    public void DespawnAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
        spawnedCharacters.Clear();
    }
}

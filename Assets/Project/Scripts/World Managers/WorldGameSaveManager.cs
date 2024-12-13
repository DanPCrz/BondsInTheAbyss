using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldGameSaveManager : MonoBehaviour
{
    public static WorldGameSaveManager instance;

    [SerializeField] int WorldSceneIndex = 1;

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
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return WorldSceneIndex;
    }
}
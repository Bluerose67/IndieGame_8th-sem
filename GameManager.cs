using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Presistent Objects")]
    public GameObject[] presistentObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPresistentObjects();

            // Listen for scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void MarkPresistentObjects()
    {
        foreach (GameObject obj in presistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in presistentObjects)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }

    // Called automatically when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Dungeon")
        {
            // Remove the persistent player to avoid duplicates
            foreach (GameObject obj in presistentObjects)
            {
                if (obj != null && obj.CompareTag("Player") && obj.CompareTag("MainCamera"))
                {
                    Destroy(obj);
                }
            }
        }
    }
}

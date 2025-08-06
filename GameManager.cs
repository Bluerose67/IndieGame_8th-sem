using UnityEngine;

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
        }
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
}

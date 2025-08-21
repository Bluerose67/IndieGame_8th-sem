using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    public void OnEnable()
    {
        SceneManager.LoadScene("SCENE_1", LoadSceneMode.Single);
    }
}

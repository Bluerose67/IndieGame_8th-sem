using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnim;
    public float fadeTime = .5f;
    public Vector2 newPlayerPosition;
    private Transform player;

    [Header("Auto change when enabled? (set true only in Timeline scene)")]
    public bool runOnEnable = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            StartSceneChange();
        }
    }

    private void OnEnable()
    {
        if (runOnEnable)
            StartSceneChange();
    }

    private void StartSceneChange()
    {
        if (fadeAnim != null)
            fadeAnim.Play("FadeToBlack");

        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);

        if (player != null)
            player.position = newPlayerPosition;

        SceneManager.LoadScene(sceneToLoad);
    }
}

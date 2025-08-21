using UnityEngine;
using System.Collections;

public class ShowHints : MonoBehaviour
{
    [Header("Assign the hint UI for this collider (optional)")]
    public GameObject HintContainer;

    [Header("Auto show settings")]
    public bool showOnSceneStart = false; // check this if you want auto-show
    public float showDelay = 2f;          // time after scene load
    public float showDuration = 3f;       // how long to show the hint

    private bool hintedOnce = false;

    private void Start()
    {
        if (showOnSceneStart && HintContainer != null)
        {
            // trigger auto-show coroutine
            StartCoroutine(ShowHintDelayed(HintContainer, showDelay, showDuration));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!hintedOnce && HintContainer != null)
        {
            hintedOnce = true;
            StartCoroutine(ShowHint(HintContainer, showDuration));
        }
    }

    IEnumerator ShowHint(GameObject hintContainer, float duration)
    {
        hintContainer.SetActive(true);
        yield return new WaitForSeconds(duration);
        hintContainer.SetActive(false);
    }

    IEnumerator ShowHintDelayed(GameObject hintContainer, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ShowHint(hintContainer, duration));
    }
}

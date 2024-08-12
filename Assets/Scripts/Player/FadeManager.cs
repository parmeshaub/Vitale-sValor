using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    [SerializeField] public CanvasGroup fadeCanvas;
    [SerializeField] public float defaultFadeDuration = 1.0f; // Default duration for fade in/out

    private void Awake() {
        instance = this;
    }

    private void Start() {
        fadeCanvas.alpha = 1;
        StartFadeOut(); // This will use the default duration
    }

    // Method to start FadeIn Coroutine with optional custom duration
    public void StartFadeIn(float duration = -1f) {
        StartCoroutine(FadeIn(duration >= 0f ? duration : defaultFadeDuration));
    }

    // Method to start FadeOut Coroutine with optional custom duration
    public void StartFadeOut(float duration = -1f) {
        StartCoroutine(FadeOut(duration >= 0f ? duration : defaultFadeDuration));
    }

    // Coroutine to handle fade-in effect
    private IEnumerator FadeIn(float duration) {
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        fadeCanvas.alpha = 1f; // Ensure it's fully visible at the end
    }

    // Coroutine to handle fade-out effect
    private IEnumerator FadeOut(float duration) {
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Clamp01(1f - (elapsedTime / duration));
            yield return null;
        }

        fadeCanvas.alpha = 0f; // Ensure it's fully transparent at the end
    }
}

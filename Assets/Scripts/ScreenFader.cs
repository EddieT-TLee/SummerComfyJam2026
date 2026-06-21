using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SetAlpha(0f);
    }

    public IEnumerator FadeOut() => Fade(0f, 1f); // clear to black
    public IEnumerator FadeIn() => Fade(1f, 0f); // black to clear

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        SetAlpha(from);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / fadeDuration));
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    public void StartFadeIn()
    {
        StartCoroutine(DelayedFadeIn());
    }

    private IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        yield return StartCoroutine(FadeIn());
    }
}
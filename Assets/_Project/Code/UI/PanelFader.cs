using UnityEngine;
using System.Collections;

public class PanelFader : MonoBehaviour
{
    [Tooltip("Duration of fade transitions in seconds.")]
    public float fadeDuration = 0.5f;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(0, 1));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(1, 0));
    }

    IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = endAlpha == 1;
        canvasGroup.blocksRaycasts = endAlpha == 1;
    }

    public float GetAlpha()
    {
        return canvasGroup.alpha;
    }
}

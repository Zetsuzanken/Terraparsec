using UnityEngine;

public class PanelFader : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(0, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(1, 0));
    }

    private System.Collections.IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = (endAlpha == 1);
        canvasGroup.blocksRaycasts = (endAlpha == 1);
    }

    public float GetAlpha()
    {
        return canvasGroup.alpha;
    }
}
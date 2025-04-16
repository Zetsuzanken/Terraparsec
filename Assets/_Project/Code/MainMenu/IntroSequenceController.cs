using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroSequenceController : MonoBehaviour
{
    [Header("UI References")]
    public PanelFader fadeOverlay;
    public PanelFader introPanel;
    public TextMeshProUGUI introText;
    public PanelFader tutorialPanel;
    public Button tutorialCloseButton;
    public Button skipButton;

    [Header("Timing Settings")]
    public float letterDelay = 0.05f;
    public float lineDelay = 0.7f;
    public float postIntroDelay = 2f;

    private readonly string[] introLines = new string[]
    {
        "You are an intrepid wanderer of the stars with an important mission.",
        "Your task… Find a new habitable world for humanity.",
        "But hurry… Time is of the essence."
    };

    private Coroutine introCoroutine;

    private void Start()
    {
        tutorialCloseButton.onClick.RemoveAllListeners();
        tutorialCloseButton.onClick.AddListener(CloseTutorialPanel);

        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipIntro);

        Time.timeScale = 0f;

        if (fadeOverlay.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.blocksRaycasts = true;
        }

        introCoroutine = StartCoroutine(RunIntroSequence());
    }

    private IEnumerator RunIntroSequence()
    {
        yield return new WaitForSecondsRealtime(fadeOverlay.fadeDuration);

        introText.text = "";

        foreach (string line in introLines)
        {
            yield return StartCoroutine(TypeLine(line));
            introText.text += "\n";
            yield return new WaitForSecondsRealtime(lineDelay);
        }

        yield return new WaitForSecondsRealtime(postIntroDelay);

        introPanel.FadeOut();
        FadeOverlayOut();
        yield return new WaitForSecondsRealtime(1);

        tutorialPanel.FadeIn();
    }

    private IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            introText.text += c;
            yield return new WaitForSecondsRealtime(letterDelay);
        }
    }

    public void SkipIntro()
    {
        if (introCoroutine != null)
        {
            StopCoroutine(introCoroutine);
            introCoroutine = null;
        }

        introPanel.FadeOut();
        FadeOverlayOut();

        StartCoroutine(DelayedTutorialPanel());
    }

    private IEnumerator DelayedTutorialPanel()
    {
        yield return new WaitForSecondsRealtime(fadeOverlay.fadeDuration);
        tutorialPanel.FadeIn();
    }

    private void FadeOverlayOut()
    {
        fadeOverlay.FadeOut();
        StartCoroutine(ResetRaycastBlock());
    }

    private IEnumerator ResetRaycastBlock()
    {
        yield return new WaitForSecondsRealtime(fadeOverlay.fadeDuration);
        if (fadeOverlay.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void CloseTutorialPanel()
    {
        tutorialPanel.FadeOut();
        Time.timeScale = 1f;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScorePanelController : MonoBehaviour
{
    public PanelFader scorePanelFader;
    public TextMeshProUGUI scoreValuesText;
    public TextMeshProUGUI scoreTotalText;

    public float descriptorDelay = 0.5f;
    public float letterDelay = 0.03f;

    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => UIManager.Instance.RestartGame());

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => UIManager.Instance.GoToMainMenu());
    }

    public void ShowScorePanel(Planet chosenPlanet, int finalscore)
    {
        StartCoroutine(ShowScoreRoutine(chosenPlanet, finalscore));
    }

    private IEnumerator ShowScoreRoutine(Planet p, int f)
    {
        UIManager.Instance.FadeOverlayIn();
        yield return new WaitForSecondsRealtime(UIManager.Instance.FadeOverlayDuration);

        scorePanelFader.FadeIn();
        yield return new WaitForSecondsRealtime(scorePanelFader.fadeDuration);

        scoreValuesText.text = "";
        string[] paramDescriptors = GetParameterDescriptors(p);

        foreach (string desc in paramDescriptors)
        {
            scoreValuesText.text += desc + "\n";
            yield return new WaitForSecondsRealtime(descriptorDelay);
        }

        int finalScore = PlanetScoreCalculator.ScorePlanet(p, f);
        yield return StartCoroutine(TypeLetters($"TOTAL: {finalScore}", scoreTotalText));
    }

    private string[] GetParameterDescriptors(Planet p)
    {
        int minutes = Mathf.FloorToInt(PlayerResources.Instance.timeRemaining / 60);
        int seconds = Mathf.FloorToInt(PlayerResources.Instance.timeRemaining % 60);
        string time = $"{minutes:00}:{seconds:00}";

        string[] lines = new string[11];
        lines[0] = TierToDescriptor(PlanetScoreCalculator.ScoreOrbit(p.orbitalDistance));
        lines[1] = TierToDescriptor(PlanetScoreCalculator.ScoreRotation(p.rotationPeriod));
        lines[2] = TierToDescriptor(PlanetScoreCalculator.ScoreEcc(p.eccentricity));
        lines[3] = TierToDescriptor(PlanetScoreCalculator.ScoreMass(p.mass));
        lines[4] = TierToDescriptor(PlanetScoreCalculator.ScoreRadius(p.radius));
        lines[5] = TierToDescriptor(PlanetScoreCalculator.ScoreDensity(p));
        lines[6] = TierToDescriptor(PlanetScoreCalculator.ScoreGravity(p));
        lines[7] = TierToDescriptor(PlanetScoreCalculator.ScoreAtmosphere(p));
        lines[8] = TierToDescriptor(PlanetScoreCalculator.ScorePressure(p.surfacePressure));
        lines[9] = TierToDescriptor(PlanetScoreCalculator.ScoreTemperature(p.averageSurfaceTemperature));
        lines[10] = time;

        return lines;
    }

    private string TierToDescriptor(int tierPoints)
    {
        return tierPoints switch
        {
            100 => "Habitable",
            75 => "Likely habitable",
            50 => "Possibly habitable",
            25 => "Probably uninhabitable",
            _ => "Uninhabitable",
        };
    }

    private IEnumerator TypeLetters(string fullText, TextMeshProUGUI targetText)
    {
        targetText.text = "";
        foreach (char c in fullText)
        {
            targetText.text += c;
            yield return new WaitForSecondsRealtime(letterDelay);
        }
    }
}

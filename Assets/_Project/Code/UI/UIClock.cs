using UnityEngine;
using TMPro;
using System.Collections;

public class UIClock : MonoBehaviour
{
    public static UIClock Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI penaltyText;
    public PanelFader penaltyFader;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (PlayerResources.Instance != null)
        {
            PlayerResources.Instance.OnTimeUpdated += UpdateTimerDisplay;
            UpdateTimerDisplay(PlayerResources.Instance.timeRemaining);
        }
    }

    private void OnDestroy()
    {
        if (PlayerResources.Instance != null)
        {
            PlayerResources.Instance.OnTimeUpdated -= UpdateTimerDisplay;
        }
    }

    void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowPenaltyText(float secondsLost)
    {
        penaltyText.text = $"-{secondsLost} Sec";
        StartCoroutine(ShowPenaltyEffect());
    }

    IEnumerator ShowPenaltyEffect()
    {
        penaltyFader.FadeIn();
        yield return new WaitForSeconds(1.5f);
        penaltyFader.FadeOut();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Tooltip("Indicates if a panel is currently open.")]
    public bool isInteracting;

    [Header("Scan & Info Panels")]
    public GameObject scanPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public PanelFader scanPanelFader;
    public PanelFader infoPanelFader;
    public Button scanCloseButton;
    public Button infoCloseButton;

    [Header("Secondary Action Button (Refuel/Mark)")]
    public Button secondaryActionButton;

    [Header("Check Planet Button")]
    public Button checkPlanetButton;

    [Header("Warp Panel & Overlay")]
    public GameObject warpPanel;
    public Transform destinationListParent;
    public GameObject destinationButtonPrefab;
    public PanelFader warpPanelFader;
    public PanelFader fadeOverlayFader;
    public Button warpCloseButton;
    public Sprite placeholderSprite;

    public float FadeOverlayDuration => fadeOverlayFader.fadeDuration;

    [Header("Warning Panel")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public PanelFader warningPanelFader;
    public Button warningCloseButton;
    public Button warningWarpButton;

    [Header("Confirm Panel")]
    public GameObject confirmPanel;
    public TextMeshProUGUI confirmText;
    public PanelFader confirmPanelFader;
    public Button confirmButton;
    public Button confirmCloseButton;

    [Header("End Game Panel")]
    public GameObject endGamePanel;
    public PanelFader endGamePanelFader;
    public TextMeshProUGUI endGameText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Tutorial Panel")]
    public GameObject tutorialPanel;
    public Button tutorialButton;

    [Header("Spaceship Reference")]
    public Transform spaceship;

    [Tooltip("Currently selected object for outline logic.")]
    public GameObject currentlySelectedObject;

    private Transform lastClosestObject;
    private Planet pendingPlanetMark;

    [HideInInspector]
    public bool gameHasEnded = false;

    public SpriteClassifier spriteClassifier;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scanCloseButton.onClick.RemoveAllListeners();
        scanCloseButton.onClick.AddListener(CloseAllPanels);

        infoCloseButton.onClick.RemoveAllListeners();
        infoCloseButton.onClick.AddListener(CloseAllPanels);

        checkPlanetButton.onClick.RemoveAllListeners();
        checkPlanetButton.onClick.AddListener(OnCheckPlanetButtonClicked);

        warpCloseButton.onClick.RemoveAllListeners();
        warpCloseButton.onClick.AddListener(ReturnToClosestObject);

        warningCloseButton.onClick.RemoveAllListeners();
        warningCloseButton.onClick.AddListener(ReturnToClosestObject);

        warningWarpButton.onClick.RemoveAllListeners();
        warningWarpButton.onClick.AddListener(() =>
        {
            CloseWarningPanel();

            if (PlayerDistanceMonitor.Instance.GetClosestWarpable() != null)
            {
                WarpManager.Instance.OpenWarpPanel(PlayerDistanceMonitor.Instance.GetClosestWarpable());
            }
        });

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(ConfirmMarkPlanet);

        confirmCloseButton.onClick.RemoveAllListeners();
        confirmCloseButton.onClick.AddListener(CloseConfirmPanel);

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        tutorialButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.AddListener(OpenTutorialPanel);
    }

    public void OpenScanPanel(GameObject caller, ICelestialObject data)
    {
        if (isInteracting)
        {
            return;
        }

        DeselectPreviousObject();
        currentlySelectedObject = caller;

        if (caller.TryGetComponent<SpriteHoverEffect>(out SpriteHoverEffect hoverEffect))
        {
            hoverEffect.SetSelected(true);
        }

        isInteracting = true;
        Time.timeScale = 0;

        scanPanelFader.FadeIn();
        Button scanBtn = scanPanel.GetComponentInChildren<Button>();
        scanBtn.onClick.RemoveAllListeners();

        if (caller.CompareTag("Finish"))
        {
            scanBtn.onClick.AddListener(() =>
            {
                ShowInfoPanel(data, caller);
            });

            TextMeshProUGUI btnText = secondaryActionButton.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = "End Journey";

            bool planetMarked = PlanetMarker.Instance.chosenPlanet != null;
            secondaryActionButton.interactable = planetMarked;

            secondaryActionButton.onClick.RemoveAllListeners();
            secondaryActionButton.onClick.AddListener(() =>
            {
                OpenConfirmEndJourney();
            });
        }
        else
        {
            scanBtn.onClick.AddListener(() =>
            {
                PlayerResources.Instance.HandleScan();
                ShowInfoPanel(data, caller);
            });

            secondaryActionButton.onClick.RemoveAllListeners();

            if (data is Star)
            {
                TextMeshProUGUI buttonText = secondaryActionButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "Refuel";
                secondaryActionButton.interactable = true;

                secondaryActionButton.onClick.AddListener(() =>
                {
                    PlayerResources.Instance.RefillEnergy();
                });
            }
            else if (data is Planet planetData)
            {
                TextMeshProUGUI buttonText = secondaryActionButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "Mark";
                secondaryActionButton.interactable = true;

                secondaryActionButton.onClick.AddListener(() =>
                {
                    if (PlanetMarker.Instance.chosenPlanet != null)
                    {
                        OpenConfirmPanel(planetData);
                    }
                    else
                    {
                        PlanetMarker.Instance.MarkPlanet(planetData);
                    }
                });
            }
        }
    }

    public void OpenWarpPanel(List<Warpable> warpables)
    {
        if (isInteracting)
        {
            return;
        }

        isInteracting = true;
        Time.timeScale = 0;

        foreach (Transform child in destinationListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Warpable w in warpables)
        {
            GameObject newButtonObj = Instantiate(destinationButtonPrefab, destinationListParent);
            Button button = newButtonObj.GetComponent<Button>();

            Image icon = newButtonObj.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI label = newButtonObj.transform.Find("Label").GetComponent<TextMeshProUGUI>();


            icon.sprite = !w.isStar && !w.visited && !(w.warpName == "Earth") ? placeholderSprite : w.warpIcon;

            label.text = w.warpName;

            button.onClick.AddListener(() =>
            {
                WarpManager.Instance.WarpTo(w);
            });
        }

        warpPanelFader.FadeIn();
    }

    public void OpenTutorialPanel()
    {
        CloseAllPanels();
        Time.timeScale = 0;
        if (tutorialPanel.TryGetComponent(out PanelFader tutorialFader))
        {
            tutorialFader.FadeIn();
        }
    }

    public void ShowInfoPanel(ICelestialObject data, GameObject caller)
    {
        if (data is Planet planetData && !planetData.scanned == true)
        {
            planetData.scanned = true;
            spriteClassifier.AssignSprite(caller);
        }

        infoText.text = data.DisplayInfo;
        scanPanelFader.FadeOut();
        infoPanelFader.FadeIn();
    }

    public void CloseAllPanels()
    {
        if (scanPanelFader.GetAlpha() > 0)
        {
            scanPanelFader.FadeOut();
        }

        if (infoPanelFader.GetAlpha() > 0)
        {
            infoPanelFader.FadeOut();
        }

        if (warpPanelFader.GetAlpha() > 0)
        {
            warpPanelFader.FadeOut();
        }

        if (warningPanelFader.GetAlpha() > 0)
        {
            warningPanelFader.FadeOut();
        }

        Time.timeScale = 1;
        isInteracting = false;

        if (currentlySelectedObject != null)
        {
            if (currentlySelectedObject.TryGetComponent<SpriteHoverEffect>(out SpriteHoverEffect hoverEffect))
            {
                hoverEffect.SetSelected(false);
            }
            currentlySelectedObject = null;
        }
    }

    public void FadeOverlayIn()
    {
        if (fadeOverlayFader.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            canvasGroup.blocksRaycasts = true;
        }
        fadeOverlayFader.FadeIn();
    }

    public void FadeOverlayOut()
    {
        fadeOverlayFader.FadeOut();
        _ = StartCoroutine(ResetRaycastBlock());
    }

    private IEnumerator ResetRaycastBlock()
    {
        yield return new WaitForSecondsRealtime(FadeOverlayDuration);

        if (fadeOverlayFader.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void DeselectPreviousObject()
    {
        if (currentlySelectedObject != null)
        {
            if (currentlySelectedObject.TryGetComponent<SpriteHoverEffect>(out SpriteHoverEffect prevHover))
            {
                prevHover.SetSelected(false);
            }
            currentlySelectedObject = null;
        }
    }

    public void ShowWarningPanel(Transform closestObject)
    {
        lastClosestObject = closestObject;
        isInteracting = true;
        Time.timeScale = 0;

        string objectName = closestObject.name;
        if (closestObject.TryGetComponent<ObjectClickHandler>(out ObjectClickHandler oh) && oh.objectData is ICelestialObject co)
        {
            objectName = co.Name;
        }

        warningText.text = $"You've traveled too far. Return to {objectName} or warp elsewhere.";
        warningPanelFader.FadeIn();
    }

    public void CloseWarningPanel()
    {
        if (warningPanelFader.GetAlpha() > 0)
        {
            warningPanelFader.FadeOut();
        }

        isInteracting = false;
    }

    public void ReturnToClosestObject()
    {
        if (lastClosestObject != null && spaceship != null)
        {
            FadeOverlayIn();
            CloseAllPanels();
            _ = StartCoroutine(ReturnSequence());
        }
    }

    private IEnumerator ReturnSequence()
    {
        yield return new WaitForSecondsRealtime(FadeOverlayDuration);

        if (lastClosestObject != null && spaceship != null)
        {
            spaceship.position = new Vector3(lastClosestObject.position.x, spaceship.position.y, spaceship.position.z);
            if (Camera.main.TryGetComponent<CameraFollow>(out CameraFollow camFollow))
            {
                camFollow.SnapToTarget();
            }

            if (spaceship.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }
        }
        FadeOverlayOut();
        Time.timeScale = 1;
    }

    public void OnCheckPlanetButtonClicked()
    {
        if (warningPanelFader.GetAlpha() > 0f)
        {
            return;
        }

        if (WarpManager.Instance.hasLeftEarth)
        {
            PlayerResources.Instance.DeductTime(5f);
        }

        infoText.text = PlanetMarker.Instance.chosenPlanet == null
            ? "You haven't marked any planet yet!"
            : PlanetMarker.Instance.chosenPlanet.DisplayInfo;

        CloseAllPanels();
        isInteracting = true;
        Time.timeScale = 0;
        infoPanelFader.FadeIn();
    }

    public void OpenConfirmPanel(Planet planetToMark)
    {
        pendingPlanetMark = planetToMark;
        confirmPanelFader.FadeIn();
        isInteracting = true;
    }

    public void CloseConfirmPanel()
    {
        confirmPanelFader.FadeOut();
        isInteracting = false;
    }

    public void ConfirmMarkPlanet()
    {
        PlanetMarker.Instance.MarkPlanet(pendingPlanetMark);
        CloseConfirmPanel();
    }

    public void OpenConfirmEndJourney()
    {
        confirmPanelFader.FadeIn();
        isInteracting = true;

        confirmText.text = "Are you sure you want to end your journey?";

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            EndJourney();
        });

        confirmCloseButton.onClick.RemoveAllListeners();
        confirmCloseButton.onClick.AddListener(CloseConfirmPanel);
    }

    private void EndJourney()
    {
        PlayerResources.Instance.StopTimer();

        CloseConfirmPanel();

        Planet chosen = PlanetMarker.Instance.chosenPlanet;
        int bonus = Mathf.FloorToInt(PlayerResources.Instance.remainingTime) / 10;

        ScorePanelController scoreCtrl = FindObjectOfType<ScorePanelController>();
        scoreCtrl.ShowScorePanel(chosen, bonus);
    }

    public void TriggerGameOver(string reason)
    {
        gameHasEnded = true;

        _ = PlayerResources.Instance.SetTime(0f);
        PlayerResources.Instance.StopTimer();

        CloseAllPanels();
        FadeOverlayIn();
        _ = StartCoroutine(ShowEndGame(reason));
    }

    private IEnumerator ShowEndGame(string reason)
    {
        yield return new WaitForSecondsRealtime(FadeOverlayDuration);

        endGameText.text = reason;
        endGamePanelFader.FadeIn();
        isInteracting = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}

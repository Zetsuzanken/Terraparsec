using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Warp Panel & Overlay")]
    public GameObject warpPanel;
    public Transform destinationListParent;
    public GameObject destinationButtonPrefab;
    public PanelFader warpPanelFader;
    public PanelFader fadeOverlayFader;
    public Button warpCloseButton;
    public float FadeOverlayDuration => fadeOverlayFader.fadeDuration;

    [Header("Warning Panel")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public PanelFader warningPanelFader;
    public Button warningCloseButton;
    public Button warningWarpButton;

    [Header("Spaceship Reference")]
    public Transform spaceship;

    [Tooltip("Currently selected object for outline logic.")]
    public GameObject currentlySelectedObject;

    private Transform lastClosestObject;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        scanCloseButton.onClick.RemoveAllListeners();
        scanCloseButton.onClick.AddListener(CloseAllPanels);

        infoCloseButton.onClick.RemoveAllListeners();
        infoCloseButton.onClick.AddListener(CloseAllPanels);

        warpCloseButton.onClick.RemoveAllListeners();
        warpCloseButton.onClick.AddListener(ReturnToClosestObject);

        warningCloseButton.onClick.RemoveAllListeners();
        warningCloseButton.onClick.AddListener(ReturnToClosestObject);

        warningWarpButton.onClick.RemoveAllListeners();
        warningWarpButton.onClick.AddListener(() =>
        {
            CloseWarningPanel();
            WarpManager.Instance.OpenWarpPanel();
        });
    }

    public void OpenScanPanel(GameObject caller, ICelestialObject data)
    {
        if (isInteracting) return;

        DeselectPreviousObject();
        currentlySelectedObject = caller;
        
        if (caller.TryGetComponent<SpriteHoverEffect>(out var hoverEffect))
        {
            hoverEffect.SetSelected(true);
        }

        isInteracting = true;
        Time.timeScale = 0;

        scanPanelFader.FadeIn();
        var scanBtn = scanPanel.GetComponentInChildren<Button>();
        scanBtn.onClick.RemoveAllListeners();
        scanBtn.onClick.AddListener(() =>
        {
            PlayerResources.Instance.HandleScan();
            ShowInfoPanel(data);
        });

        var refuelButton = scanPanel.transform.Find("RefuelButton").GetComponent<Button>();
        refuelButton.gameObject.SetActive(data is Star);

        refuelButton.onClick.RemoveAllListeners();
        refuelButton.onClick.AddListener(() =>
        {
            PlayerResources.Instance.RefillEnergy();
        });
    }

    public void OpenWarpPanel(List<WarpDestination> warpDestinations)
    {
        if (isInteracting) return;

        isInteracting = true;
        Time.timeScale = 0;

        PopulateWarpPanel(warpDestinations);
        warpPanelFader.FadeIn();
    }

    public void ShowInfoPanel(ICelestialObject data)
    {
        infoText.text = data.GetDisplayInfo();
        scanPanelFader.FadeOut();
        infoPanelFader.FadeIn();
    }

    public void CloseAllPanels()
    {
        if (scanPanelFader.GetAlpha() > 0) scanPanelFader.FadeOut();
        if (infoPanelFader.GetAlpha() > 0) infoPanelFader.FadeOut();
        if (warpPanelFader.GetAlpha() > 0) warpPanelFader.FadeOut();
        if (warningPanelFader.GetAlpha() > 0) warningPanelFader.FadeOut();

        Time.timeScale = 1;
        isInteracting = false;

        if (currentlySelectedObject != null)
        {
            if (currentlySelectedObject.TryGetComponent<SpriteHoverEffect>(out var hoverEffect))
            {
                hoverEffect.SetSelected(false);
            }
            currentlySelectedObject = null;
        }
    }

    void PopulateWarpPanel(List<WarpDestination> warpDestinations)
    {
        foreach (Transform child in destinationListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var dest in warpDestinations)
        {
            var newButtonObj = Instantiate(destinationButtonPrefab, destinationListParent);
            var button = newButtonObj.GetComponent<Button>();

            var icon = newButtonObj.transform.Find("Icon").GetComponent<Image>();
            var label = newButtonObj.transform.Find("Label").GetComponent<TextMeshProUGUI>();

            icon.sprite = dest.destinationSprite;
            label.text = dest.destinationName;

            button.onClick.AddListener(() =>
            {
                WarpManager.Instance.WarpTo(dest);
            });
        }
    }

    public void FadeOverlayIn()
    {
        if (fadeOverlayFader.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.blocksRaycasts = true;
        }
        fadeOverlayFader.FadeIn();
    }

    public void FadeOverlayOut()
    {
        fadeOverlayFader.FadeOut();
        StartCoroutine(ResetRaycastBlock());
    }

    private IEnumerator ResetRaycastBlock()
    {
        yield return new WaitForSecondsRealtime(FadeOverlayDuration);

        if (fadeOverlayFader.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    void DeselectPreviousObject()
    {
        if (currentlySelectedObject != null)
        {
            if (currentlySelectedObject.TryGetComponent<SpriteHoverEffect>(out var prevHover))
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
        warningText.text = $"You've traveled too far. Return to {closestObject.name} or warp elsewhere.";
        warningPanelFader.FadeIn();
    }

    public void CloseWarningPanel()
    {
        if (warningPanelFader.GetAlpha() > 0) warningPanelFader.FadeOut();
        isInteracting = false;
    }

    public void ReturnToClosestObject()
    {
        if (lastClosestObject != null && spaceship != null)
        {
            FadeOverlayIn();
            // CloseWarningPanel();
            CloseAllPanels();
            StartCoroutine(ReturnSequence());
        }
    }

    IEnumerator ReturnSequence()
    {
        yield return new WaitForSecondsRealtime(FadeOverlayDuration);

        if (lastClosestObject != null && spaceship != null)
        {
            spaceship.position = new Vector3(lastClosestObject.position.x, spaceship.position.y, spaceship.position.z);
            if (Camera.main.TryGetComponent<CameraFollow>(out var camFollow)) camFollow.SnapToTarget();

            if (spaceship.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }
        }
        FadeOverlayOut();
        Time.timeScale = 1;
    }
}

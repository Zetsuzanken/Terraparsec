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

    [Tooltip("Currently selected object for outline logic.")]
    public GameObject currentlySelectedObject;

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
        warpCloseButton.onClick.AddListener(CloseAllPanels);
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
        scanBtn.onClick.AddListener(() => ShowInfoPanel(data));
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
}

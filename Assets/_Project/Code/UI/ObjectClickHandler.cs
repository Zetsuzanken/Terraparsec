using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectClickHandler : MonoBehaviour
{
    public ScriptableObject objectData;

    public GameObject scanPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    public Button scanCloseButton;
    public Button infoCloseButton;

    private PanelFader scanPanelFader;
    private PanelFader infoPanelFader;

    void Start()
    {
        scanPanelFader = scanPanel.GetComponent<PanelFader>();
        infoPanelFader = infoPanel.GetComponent<PanelFader>();

        scanCloseButton.onClick.RemoveAllListeners();
        scanCloseButton.onClick.AddListener(OnCloseButtonClicked);

        infoCloseButton.onClick.RemoveAllListeners();
        infoCloseButton.onClick.AddListener(OnCloseButtonClicked);
    }

    void OnMouseDown()
    {
        if (objectData is ICelestialObject celestialObject)
        {
            scanPanelFader.FadeIn();
            Time.timeScale = 0;
            Button scanBtn = scanPanel.GetComponentInChildren<Button>();
            scanBtn.onClick.RemoveAllListeners();
            scanBtn.onClick.AddListener(OnScanButtonClicked);
        }
    }

    public void OnScanButtonClicked()
    {
        if (objectData is ICelestialObject celestialObject)
        {
            infoText.text = celestialObject.GetDisplayInfo();
            scanPanelFader.FadeOut();
            infoPanelFader.FadeIn();
        }
    }

    public void OnCloseButtonClicked()
    {
        if (scanPanelFader.GetAlpha() > 0)
            scanPanelFader.FadeOut();

        if (infoPanelFader.GetAlpha() > 0)
            infoPanelFader.FadeOut();

        Time.timeScale = 1;
    }
}

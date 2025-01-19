using UnityEngine;
using TMPro;

public class StarClickHandler : MonoBehaviour
{
    public Star starData;
    public GameObject scanPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    private PanelFader scanPanelFader;
    private PanelFader infoPanelFader;

    void Start()
    {
        scanPanelFader = scanPanel.GetComponent<PanelFader>();
        infoPanelFader = infoPanel.GetComponent<PanelFader>();
    }

    void OnMouseDown()
    {
        if (starData != null)
        {
            scanPanelFader.FadeIn();
            Time.timeScale = 0;
        }
    }

    public void OnScanButtonClicked()
    {
        if (starData != null)
        {
            infoText.text = $"Name: {starData.objectName}\n" +
                            $"Type: {starData.starType}\n" +
                            $"Mass: {starData.mass} solar masses\n" +
                            $"Radius: {starData.radius} solar radii\n" +
                            $"Temperature: {starData.surfaceTemperature}K\n" +
                            $"Luminosity: {starData.luminosity} times the Sun's luminosity\n" +
                            $"Age: {starData.age} billion years\n" +
                            $"Lifecycle Stage: {starData.lifecycleStage}\n" +
                            $"Color: {starData.color}\n" +
                            $"Description: {starData.description}";

            scanPanelFader.FadeOut();
            infoPanelFader.FadeIn();
        }
    }

    public void OnCloseButtonClicked()
    {
        if (scanPanelFader.GetAlpha() > 0)
        {
            scanPanelFader.FadeOut();
        }

        if (infoPanelFader.GetAlpha() > 0)
        {
            infoPanelFader.FadeOut();
        }

        Time.timeScale = 1;
    }
}

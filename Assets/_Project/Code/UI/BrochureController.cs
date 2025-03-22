using UnityEngine;
using UnityEngine.UI;

public class BrochureController : MonoBehaviour
{
    [Header("References to the Brochure UI")]
    public GameObject brochurePanel;
    public PanelFader brochurePanelFader;
    public GameObject[] pages;

    [Header("Navigation Buttons")]
    public Button buttonPrevious;
    public Button buttonNext;
    public Button buttonClose;

    [Header("Button to open the Brochure")]
    public Button openBrochureButton;

    private int currentPageIndex = 0;
    private bool isOpen = false;

    private void Start()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        if (buttonPrevious)
        {
            buttonPrevious.onClick.RemoveAllListeners();
            buttonPrevious.onClick.AddListener(OnPreviousClicked);
        }

        if (buttonNext)
        {
            buttonNext.onClick.RemoveAllListeners();
            buttonNext.onClick.AddListener(OnNextClicked);
        }

        if (buttonClose)
        {
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(CloseBrochure);
        }

        if (openBrochureButton)
        {
            openBrochureButton.onClick.RemoveAllListeners();
            openBrochureButton.onClick.AddListener(OpenBrochure);
        }
    }

    public void OpenBrochure()
    {
        if (WarpManager.Instance.hasLeftEarth)
        {
            PlayerResources.Instance.DeductTime(5f);
        }

        if (isOpen)
        {
            return;
        }

        isOpen = true;
        Time.timeScale = 0;

        currentPageIndex = 0;
        ShowPage(currentPageIndex);

        brochurePanelFader.FadeIn();
    }

    public void CloseBrochure()
    {
        if (!isOpen)
        {
            return;
        }

        brochurePanelFader.FadeOut();

        isOpen = false;
        Time.timeScale = 1;
    }

    private void OnNextClicked()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    private void OnPreviousClicked()
    {
        if (currentPageIndex > 0)
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    private void ShowPage(int index)
    {
        if (index < 0 || index >= pages.Length)
        {
            return;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[index].SetActive(true);
    }
}

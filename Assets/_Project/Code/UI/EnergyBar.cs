using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public static EnergyBar Instance;

    public Image energyBarFill;
    public TextMeshProUGUI energyText;

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
        if (PlayerResources.Instance != null)
        {
            float maxEnergy = PlayerResources.Instance.maxEnergy;
            UpdateEnergyUI(maxEnergy);

            PlayerResources.Instance.OnEnergyUpdated += UpdateEnergyUI;
        }
    }

    private void OnDestroy()
    {
        if (PlayerResources.Instance != null)
        {
            PlayerResources.Instance.OnEnergyUpdated -= UpdateEnergyUI;
        }
    }

    public void UpdateEnergyUI(float newEnergy)
    {
        StopAllCoroutines();
        _ = StartCoroutine(AnimateEnergyChange(newEnergy));
    }

    private IEnumerator AnimateEnergyChange(float targetEnergy)
    {
        float currentEnergy = float.Parse(energyText.text.Replace("%", "")) / 100 * PlayerResources.Instance.maxEnergy;

        while (Mathf.Abs(currentEnergy - targetEnergy) > 0.1f)
        {
            currentEnergy = Mathf.Lerp(currentEnergy, targetEnergy, 10f * Time.unscaledDeltaTime);
            float fillAmount = currentEnergy / PlayerResources.Instance.maxEnergy;
            energyBarFill.fillAmount = Mathf.Clamp01(fillAmount);
            energyText.text = $"{Mathf.FloorToInt(fillAmount * 100)}%";

            yield return null;
        }

        energyBarFill.fillAmount = targetEnergy / PlayerResources.Instance.maxEnergy;
        energyText.text = $"{Mathf.FloorToInt(energyBarFill.fillAmount * 100)}%";
    }
}

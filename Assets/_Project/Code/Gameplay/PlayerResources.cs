using System;
using System.Collections;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;

    public float maxEnergy = 100f;
    public float remainingEnergy;
    public float scanEnergyCost = 5f;
    public float warpEnergyCost = 15f;

    public float maxTime = 600f;
    public float remainingTime;
    public float warpTimePenalty = 5f;

    private bool isTimeRunning = false;

    public event Action<float> OnTimeUpdated;
    public event Action<float> OnEnergyUpdated;

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
        remainingEnergy = maxEnergy;

        remainingTime = maxTime;
    }

    public float SetTime(float newTime)
    {
        remainingTime = newTime;
        OnTimeUpdated?.Invoke(remainingTime);
        return remainingTime;
    }

    public void StartTimer()
    {
        if (!isTimeRunning)
        {
            isTimeRunning = true;
            _ = StartCoroutine(CountdownTimer());
        }
    }

    public void StopTimer()
    {
        isTimeRunning = false;
    }

    private IEnumerator CountdownTimer()
    {
        while (isTimeRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.unscaledDeltaTime;
                OnTimeUpdated?.Invoke(remainingTime);
            }
            else
            {
                remainingTime = 0;
                isTimeRunning = false;
                HandleTimeExpired();
            }

            yield return null;
        }
    }

    public void UseEnergy(float amount)
    {
        remainingEnergy -= amount;
        if (remainingEnergy < 0)
        {
            remainingEnergy = 0;
        }

        OnEnergyUpdated?.Invoke(remainingEnergy);

        if (remainingEnergy <= 0)
        {
            UIManager.Instance.TriggerGameOver("You ran out of energy and must now spend an eternity in the cold vacuum of space!");
        }
    }

    public void DeductTime(float amount)
    {
        remainingTime -= amount;
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        OnTimeUpdated?.Invoke(remainingTime);

        if (UIClock.Instance != null)
        {
            UIClock.Instance.ShowPenaltyText(amount);
        }
    }

    public void HandleScan()
    {
        UseEnergy(scanEnergyCost);
    }

    public void HandleWarp()
    {
        UseEnergy(warpEnergyCost);
        DeductTime(warpTimePenalty);
    }

    private void HandleTimeExpired()
    {
        UIManager.Instance.TriggerGameOver("You ran out of time! Earth may not survive...");
    }

    public void RefillEnergy()
    {
        remainingEnergy = maxEnergy;

        OnEnergyUpdated?.Invoke(remainingEnergy);
    }
}

using UnityEngine;
using System.Collections;
using System;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float energy;
    public float scanEnergyCost = 10f;
    public float warpEnergyCost = 15f;

    [Header("Time Settings")]
    public float maxTime = 600f;
    public float timeRemaining;
    public float warpTimePenalty = 5f;

    private bool isTimeRunning = true;

    public event Action<float> OnTimeUpdated;
    public event Action<float> OnEnergyUpdated;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        energy = maxEnergy;
        timeRemaining = maxTime;
        StartCoroutine(CountdownTimer());
    }

    IEnumerator CountdownTimer()
    {
        while (isTimeRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.unscaledDeltaTime;
                OnTimeUpdated?.Invoke(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimeRunning = false;
                HandleTimeExpired();
            }
            yield return null;
        }
    }

    public bool CanScan() => energy >= scanEnergyCost;

    public bool CanWarp() => energy >= warpEnergyCost;

    public void UseEnergy(float amount)
    {
        energy -= amount;
        if (energy < 0) energy = 0;

        OnEnergyUpdated?.Invoke(energy);
    }

    public void DeductTime(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0) timeRemaining = 0;

        OnTimeUpdated?.Invoke(timeRemaining);

        if (UIClock.Instance != null)
        {
            UIClock.Instance.ShowPenaltyText(amount);
        }
    }

    public void HandleScan()
    {
        if (CanScan())
        {
            UseEnergy(scanEnergyCost);
        }
        else
        {
            Debug.LogWarning("Not enough energy to scan!");
        }
    }

    public void HandleWarp()
    {
        if (CanWarp())
        {
            UseEnergy(warpEnergyCost);
            DeductTime(warpTimePenalty);
        }
        else
        {
            Debug.LogWarning("Not enough energy to warp!");
        }
    }

    private void HandleTimeExpired()
    {
        Debug.LogWarning("Time is up! Implement game over logic here.");
    }

    public void RefillEnergy()
    {
        energy = maxEnergy;
        OnEnergyUpdated?.Invoke(energy);
    }
}

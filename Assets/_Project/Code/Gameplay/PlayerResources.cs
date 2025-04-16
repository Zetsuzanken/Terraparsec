using UnityEngine;
using System.Collections;
using System;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float energy;
    public float scanEnergyCost = 5f;
    public float warpEnergyCost = 15f;

    [Header("Time Settings")]
    public float maxTime = 600f;
    public float timeRemaining;
    public float warpTimePenalty = 5f;

    private bool isTimeRunning = false;

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
    }

    public void StartTimer()
    {
        if (!isTimeRunning)
        {
            isTimeRunning = true;
            StartCoroutine(CountdownTimer());
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

    public void UseEnergy(float amount)
    {
        energy -= amount;
        if (energy < 0) energy = 0;

        OnEnergyUpdated?.Invoke(energy);

        if (energy <= 0)
        {
            UIManager.Instance.TriggerGameOver("You ran out of energy and must now spend an eternity in the cold vacuum of space!");
        }
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
        energy = maxEnergy;
        OnEnergyUpdated?.Invoke(energy);
    }

    public bool WouldBeStrandedAfterUse(float useAmount)
    {
        return (energy - useAmount < warpEnergyCost);
    }
}

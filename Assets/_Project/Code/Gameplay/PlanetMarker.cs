using UnityEngine;

public class PlanetMarker : MonoBehaviour
{
    public static PlanetMarker Instance;

    [Tooltip("The planet the player chose as most habitable.")]
    public Planet chosenPlanet;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MarkPlanet(Planet planet)
    {
        chosenPlanet = planet;
    }
}

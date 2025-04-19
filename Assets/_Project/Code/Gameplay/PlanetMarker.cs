using UnityEngine;

public class PlanetMarker : MonoBehaviour
{
    public static PlanetMarker Instance;
    public Planet chosenPlanet;

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

    public void MarkPlanet(Planet planet)
    {
        chosenPlanet = planet;
    }
}

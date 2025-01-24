using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanet", menuName = "Celestial Object/Planet", order = 1)]
public class Planet : ScriptableObject, ICelestialObject
{
    public string planetName;
    public string planetType;
    public string description;
    public float orbitalDistance;
    public float orbitalPeriod;
    public float eccentricity;
    public float mass;
    public float radius;
    public float density;
    public float surfaceGravity;
    public bool hasAtmosphere;
    public string atmosphereComposition;
    public float surfacePressure;
    public float averageTemperature;

    public string GetName()
    {
        return planetName;
    }

    public string GetDisplayInfo()
    {
        return $"Name: {planetName}\n" +
               $"Type: {planetType}\n" +
               $"Orbital Distance: {orbitalDistance} AU\n" +
               $"Orbital Period: {orbitalPeriod} Earth days\n" +
               $"Eccentricity: {eccentricity}\n" +
               $"Mass: {mass} Earth masses\n" +
               $"Radius: {radius} Earth radii\n" +
               $"Density: {density} g/cm^3\n" +
               $"Surface Gravity: {surfaceGravity} m/s^2\n" +
               $"Has Atmosphere: {hasAtmosphere}\n" +
               $"Atmospheric Composition: {atmosphereComposition}\n" +
               $"Surface Pressure: {surfacePressure} atm\n" +
               $"Average Temperature: {averageTemperature}K\n" +
               $"Description: {description}";
    }
}

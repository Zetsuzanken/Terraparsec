using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanet", menuName = "Celestial Object/Planet", order = 1)]
public class Planet : ScriptableObject, ICelestialObject
{
    [Header("General")]
    public string planetName;
    public bool generatedAsHabitable = false;

    [Header("Orbital Parameters")]
    public float orbitalDistance;
    public float rotationPeriod;
    public float eccentricity;

    [Header("Physical Characteristics")]
    public float mass;
    public float radius;

    public float Density => (radius <= 0) ? 0 : 5.51f * (mass / (radius * radius * radius));
    public float SurfaceGravity => (radius <= 0) ? 0 : 9.81f * (mass / (radius * radius));

    [Header("Atmosphere")]
    public bool hasAtmosphere;
    public string atmosphericComposition;
    public float surfacePressure;

    [Header("Temperature")]
    public float averageSurfaceTemperature;

    public string Name => planetName;

    public string DisplayInfo => $"Name: {planetName}\n" +
            $"Orbital Distance: {orbitalDistance} AU\n" +
            $"Rotation Period: {rotationPeriod} hours\n" +
            $"Eccentricity: {eccentricity}\n" +
            $"Mass: {mass} Earth masses\n" +
            $"Radius: {radius} Earth radii\n" +
            $"Density: {Density:F2} g/cm^3\n" +
            $"Surface Gravity: {SurfaceGravity:F2} m/s^2\n" +
            $"Atmospheric Composition: {atmosphericComposition}\n" +
            $"Surface Pressure: {surfacePressure} atm\n" +
            $"Average Surface Temperature: {averageSurfaceTemperature} K\n";
}

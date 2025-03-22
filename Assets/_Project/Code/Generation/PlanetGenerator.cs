using UnityEngine;

[System.Serializable]
public struct AtmosphereCombo
{
    public string gas1;
    public string gas2;
    public string gas3;

    public AtmosphereCombo(string g1, string g2, string g3)
    {
        gas1 = g1;
        gas2 = g2;
        gas3 = g3;
    }
}

public class PlanetGenerator : MonoBehaviour
{
    [Header("Orbital Distance (AU)")]
    public float minOrbitalDistance = 0.01f;
    public float maxOrbitalDistance = 1000f;

    [Header("Rotation Period (hours)")]
    public float minRotationPeriod = 4f;
    public float maxRotationPeriod = 1000f;

    [Header("Eccentricity")]
    public float minEccentricity = 0f;
    public float maxEccentricity = 1f;

    [Header("Atmosphere Chance")]
    [Range(0f, 1f)]
    public float atmosphereChance = 0.7f;

    [Header("Mass & Radius (Earth Units)")]
    public float minMass = 0.01f;
    public float maxMass = 4000f;
    public float minRadius = 0.3f;
    public float maxRadius = 20f;

    [Header("Atmosphere Combos")]
    public AtmosphereCombo[] possibleAtmospheres =
{
        new("H2", "He", "CH4"),
        new("N2", "O2", "Ar"),
        new("CO2", "N2", "Ar"),
    };

    [Header("Surface Pressure (atm)")]
    public float minPressure = 0f;
    public float maxPressure = 100f;

    [Header("Temperature (K)")]
    public float minTemperature = 3f;
    public float maxTemperature = 4700f;

    private int planetCount = 0;

    private Planet CreateUniquePlanetData()
    {
        planetCount++;

        if (planetCount % 3 == 0)
        {
            return CreateHabitablePlanetData();
        }
        else if (planetCount % 2 == 0 && Random.value < 0.25f)
        {
            return CreateHabitablePlanetData();
        }
        else
        {
            return CreateRandomPlanetData();
        }
    }

    private Planet CreateRandomPlanetData()
    {
        Planet newPlanet = ScriptableObject.CreateInstance<Planet>();

        newPlanet.planetName = GeneratePlanetName();
        newPlanet.orbitalDistance = LogDistribution(minOrbitalDistance, maxOrbitalDistance);

        float ecc = WeightedRange(minEccentricity, maxEccentricity, 1.2f);
        newPlanet.eccentricity = Mathf.Round(ecc * 10000f) / 10000f;

        newPlanet.rotationPeriod = LogDistribution(minRotationPeriod, maxRotationPeriod);
        newPlanet.mass = LogDistribution(minMass, maxMass);
        newPlanet.radius = LogDistribution(minRadius, maxRadius);

        bool hasAtmo = (Random.value < atmosphereChance);
        newPlanet.hasAtmosphere = hasAtmo;
        if (hasAtmo)
        {
            int comboIndex = Random.Range(0, possibleAtmospheres.Length);
            AtmosphereCombo chosenCombo = possibleAtmospheres[comboIndex];
            newPlanet.atmosphericComposition = GenerateAtmosphericComposition(chosenCombo);
            newPlanet.surfacePressure = Random.Range(minPressure, maxPressure);
        }
        else
        {
            newPlanet.atmosphericComposition = "The planet lacks a stable, substantial atmosphere.";
            newPlanet.surfacePressure = 0f;
        }

        newPlanet.averageSurfaceTemperature = LogDistribution(minTemperature, maxTemperature);

        return newPlanet;
    }

    private Planet CreateHabitablePlanetData()
    {
        Planet newPlanet = ScriptableObject.CreateInstance<Planet>();
        newPlanet.generatedAsHabitable = true;

        newPlanet.planetName = GeneratePlanetName();
        newPlanet.orbitalDistance = Round2(Random.Range(0.5f, 3.4f));
        newPlanet.rotationPeriod = Round2(Random.Range(12f, 48f));
        newPlanet.eccentricity = Mathf.Round(Random.Range(0.0017f, 0.116f) * 10000f) / 10000f;
        newPlanet.mass = Round2(Random.Range(0.25f, 20f));
        newPlanet.radius = Round2(Random.Range(0.4f, 4.4f));
        newPlanet.hasAtmosphere = true;
        newPlanet.atmosphericComposition = GenerateHabitableAtmosphere();
        newPlanet.surfacePressure = Round2(Random.Range(0.5f, 2f));
        newPlanet.averageSurfaceTemperature = Round2(Random.Range(273.15f, 323.15f));

        return newPlanet;
    }

    private float Round2(float val)
    {
        return Mathf.Round(val * 100f) / 100f;
    }

    public void AssignPlanetData(GameObject planetGO)
    {
        if (planetGO.TryGetComponent(out ObjectClickHandler handler))
        {
            Planet planetData = CreateUniquePlanetData();
            handler.objectData = planetData;

            if (planetGO.TryGetComponent(out Warpable warpable))
            {
                warpable.UpdateWarpableNameFromObjectData();
            }
        }
    }

    private float WeightedRange(float minVal, float maxVal, float exponent)
    {
        float rand01 = Random.value;
        float weighted = Mathf.Pow(rand01, exponent);
        return minVal + (maxVal - minVal) * weighted;
    }

    private float LogDistribution(float minVal, float maxVal)
    {
        float logMin = Mathf.Log10(minVal);
        float logMax = Mathf.Log10(maxVal);

        float rLog = Random.Range(logMin, logMax);
        float rawValue = Mathf.Pow(10f, rLog);

        float rounded = Mathf.Round(rawValue * 100f) / 100f;
        return rounded;
    }

    private string GeneratePlanetName()
    {
        char letter1 = (char)('A' + Random.Range(0, 26));
        char letter2 = (char)('A' + Random.Range(0, 26));

        int digitsCount = Random.Range(3, 5);
        int randNumber = Random.Range(0, (int)Mathf.Pow(10, digitsCount));
        string numberStr = randNumber.ToString($"D{digitsCount}");

        char lastLetter = (char)('a' + Random.Range(0, 26));

        return $"{letter1}{letter2}-{numberStr}{lastLetter}";
    }

    private string GenerateAtmosphericComposition(AtmosphereCombo combo)
    {
        float x1 = Random.Range(0f, 100f);
        float x2 = Random.Range(0f, 100f - x1);
        float x3 = 100f - x1 - x2;

        float p1 = Mathf.Round(x1 * 10f) / 10f;
        float p2 = Mathf.Round(x2 * 10f) / 10f;
        float p3 = Mathf.Round(x3 * 10f) / 10f;

        return $"{combo.gas1} ({p1}%), {combo.gas2} ({p2}%), {combo.gas3} ({p3}%)";
    }

    private string GenerateHabitableAtmosphere()
    {
        float baseN2 = 78.1f;
        float baseO2 = 21.0f;
        float baseAr = 0.9f;

        float devRange = 2.5f;

        float devN2 = Random.Range(-devRange, devRange);
        float devO2 = Random.Range(-devRange, devRange);
        float devAr = Random.Range(-devRange, devRange);

        float n2 = baseN2 + devN2;
        float o2 = baseO2 + devO2;
        float ar = baseAr + devAr;

        float sum = n2 + o2 + ar;
        float scale = 100f / sum;

        n2 *= scale;
        o2 *= scale;
        ar *= scale;

        n2 = Mathf.Round(n2 * 10f) / 10f;
        o2 = Mathf.Round(o2 * 10f) / 10f;
        ar = Mathf.Round(ar * 10f) / 10f;

        return $"N2 ({n2}%), O2 ({o2}%), Ar ({ar}%)";
    }
}

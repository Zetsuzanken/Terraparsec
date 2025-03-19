using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStar", menuName = "Celestial Object/Star", order = 1)]
public class Star : ScriptableObject, ICelestialObject
{
    public string starSystemID;
    public string starName;

    private static readonly string[] starNames = new string[]
    {
        "Sirius", "Canopus", "Arcturus", "Vega", "Capella", "Rigel", "Procyon", "Betelgeuse",
        "Altair", "Aldebaran", "Spica", "Antares", "Pollux", "Fomalhaut", "Deneb", "Regulus",
        "Castor", "Mirach", "Algol", "Alpheratz", "Bellatrix", "Elnath", "Alnitak", "Saiph"
    };

    private static readonly HashSet<string> usedNames = new();

    public void AssignRandomName()
    {
        if (!string.IsNullOrEmpty(starName))
        {
            return;
        }

        while (true)
        {
            if (!usedNames.Contains(starNames[Random.Range(0, starNames.Length)]))
            {
                usedNames.Add(starNames[Random.Range(0, starNames.Length)]);
                starName = starNames[Random.Range(0, starNames.Length)];
                break;
            }
        }
    }

    public string Name => starName;

    public string DisplayInfo => $"Name: {starName}" + "\n" + (CountHabitablePlanetsInSystem() == 1
                ? "This star system contains 1 potentially habitable exoplanet."
                : $"This star system contains {CountHabitablePlanetsInSystem()} potentially habitable exoplanets.");

    private int CountHabitablePlanetsInSystem()
    {
        int count = 0;

        foreach (Warpable w in FindObjectsOfType<Warpable>())
        {
            if (w.starSystemID != starSystemID)
            {
                continue;
            }

            if (w.TryGetComponent(out ObjectClickHandler handler) &&
                handler.objectData is Planet planet &&
                planet.generatedAsHabitable)
            {
                count++;
            }
        }
        return count;
    }
}

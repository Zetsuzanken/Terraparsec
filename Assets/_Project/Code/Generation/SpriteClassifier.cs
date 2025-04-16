using UnityEngine;

public class SpriteClassifier : MonoBehaviour
{
    [Header("Planet Sprites")]
    public Sprite[] drySprites;
    public Sprite[] gasGiantSprites;
    public Sprite[] habitableSprites;
    public Sprite[] iceWorldSprites;
    public Sprite[] lavaWorldSprites;
    public Sprite[] noAtmosphereSprites;
    public Sprite blackSphere;

    [Header("Star Sprites")]
    public Sprite[] sunSprites;

    private Sprite PickRandom(Sprite[] spriteArray)
    {
        if (spriteArray == null || spriteArray.Length == 0)
        {
            return null;
        }

        return spriteArray[Random.Range(0, spriteArray.Length)];
    }

    public void AssignSprite(GameObject targetGO)
    {
        if (targetGO.CompareTag("Finish"))
        {
            if (targetGO.TryGetComponent(out SpriteRenderer sr) &&
                targetGO.TryGetComponent(out Warpable w))
            {
                w.warpIcon = sr.sprite;
            }
            return;
        }

        if (!targetGO.TryGetComponent(out ObjectClickHandler handler))
        {
            return;
        }

        if (!targetGO.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            return;
        }

        if (handler.objectData is Star)
        {
            spriteRenderer.sprite = PickRandom(sunSprites);
        }
        else if (handler.objectData is Planet planetData)
        {
            spriteRenderer.sprite = ChoosePlanetSprite(planetData);
        }

        if (targetGO.TryGetComponent(out Warpable warpable))
        {
            warpable.warpIcon = spriteRenderer.sprite;
        }
    }

    private Sprite ChoosePlanetSprite(Planet planet)
    {
        if (!planet.scanned && !PlanetNameIsEarth(planet))
        {
            return blackSphere;
        }

        if (!planet.hasAtmosphere)
        {
            return PickRandom(noAtmosphereSprites);
        }

        if (planet.generatedAsHabitable)
        {
            return PickRandom(habitableSprites);
        }

        if (planet.atmosphericComposition.Contains("H2")
            && planet.atmosphericComposition.Contains("He")
            && planet.atmosphericComposition.Contains("CH4"))
        {
            return PickRandom(gasGiantSprites);
        }

        if (planet.averageSurfaceTemperature < 273.15f)
        {
            return PickRandom(iceWorldSprites);
        }

        if (planet.averageSurfaceTemperature > 500f)
        {
            return PickRandom(lavaWorldSprites);
        }

        if (planet.atmosphericComposition.Contains("CO2")
            && planet.atmosphericComposition.Contains("N2")
            && planet.atmosphericComposition.Contains("Ar"))
        {
            return PickRandom(drySprites);
        }

        return PickRandom(drySprites);
    }

    private bool PlanetNameIsEarth(Planet planet)
    {
        return planet.planetName == "Earth";
    }
}

using UnityEngine;

public class SpriteClassifier : MonoBehaviour
{
    public Sprite[] drySprites;
    public Sprite[] gasGiantSprites;
    public Sprite[] habitableSprites;
    public Sprite[] iceWorldSprites;
    public Sprite[] lavaWorldSprites;
    public Sprite[] noAtmosphereSprites;
    public Sprite[] sunSprites;

    public Sprite blackSphere;

    private Sprite PickRandom(Sprite[] spriteArray)
    {
        return spriteArray == null || spriteArray.Length == 0 ? null : spriteArray[Random.Range(0, spriteArray.Length)];
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
        return !planet.scanned && !PlanetNameIsEarth(planet)
            ? blackSphere
            : !planet.hasAtmosphere
            ? PickRandom(noAtmosphereSprites)
            : planet.generatedAsHabitable
            ? PickRandom(habitableSprites)
            : planet.atmosphericComposition.Contains("H2")
            && planet.atmosphericComposition.Contains("He")
            && planet.atmosphericComposition.Contains("CH4")
            ? PickRandom(gasGiantSprites)
            : planet.averageSurfaceTemperature < 273.15f
            ? PickRandom(iceWorldSprites)
            : planet.averageSurfaceTemperature > 500f
            ? PickRandom(lavaWorldSprites)
            : planet.atmosphericComposition.Contains("CO2")
            && planet.atmosphericComposition.Contains("N2")
            && planet.atmosphericComposition.Contains("Ar")
            ? PickRandom(drySprites)
            : PickRandom(drySprites);
    }

    private bool PlanetNameIsEarth(Planet planet)
    {
        return planet.planetName == "Earth";
    }
}

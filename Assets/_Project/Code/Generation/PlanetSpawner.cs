using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public PlanetGenerator generator;
    public SpriteClassifier spriteClassifier;
    public GameObject[] planetObjects;

    private void Start()
    {
        foreach (GameObject planetGO in planetObjects)
        {
            generator.AssignPlanetData(planetGO);
            spriteClassifier.AssignSprite(planetGO);
        }
    }
}

using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [Tooltip("Reference to the PlanetGenerator in the scene.")]
    public PlanetGenerator generator;

    [Tooltip("A list of planet GameObjects that already exist in the scene.")]
    public GameObject[] planetObjects;

    void Start()
    {
        foreach (GameObject planetGO in planetObjects)
        {
            generator.AssignPlanetData(planetGO);
        }
    }
}

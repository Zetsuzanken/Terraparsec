using UnityEngine;

[CreateAssetMenu(fileName = "NewStar", menuName = "Celestial Object/Star", order = 1)]
public class Star : ScriptableObject
{
    public string objectName;
    public string starType;
    public float mass;
    public float radius;
    public float surfaceTemperature;
    public float luminosity;
    public float age;
    public string lifecycleStage;
    public string color;
    public string description;
}

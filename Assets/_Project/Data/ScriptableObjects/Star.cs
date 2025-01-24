using UnityEngine;

[CreateAssetMenu(fileName = "NewStar", menuName = "Celestial Object/Star", order = 1)]
public class Star : ScriptableObject, ICelestialObject
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

    public string GetName()
    {
        return objectName;
    }

    public string GetDisplayInfo()
    {
        return $"Name: {objectName}\n" +
               $"Type: {starType}\n" +
               $"Mass: {mass} solar masses\n" +
               $"Radius: {radius} solar radii\n" +
               $"Temperature: {surfaceTemperature}K\n" +
               $"Luminosity: {luminosity} times the Sun's luminosity\n" +
               $"Age: {age} billion years\n" +
               $"Lifecycle Stage: {lifecycleStage}\n" +
               $"Color: {color}\n" +
               $"Description: {description}";
    }
}

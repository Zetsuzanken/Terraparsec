using UnityEngine;

public class StarClickHandler : MonoBehaviour
{
    public Star starData;

    void OnMouseDown()
    {
        if (starData != null)
        {
            Debug.Log($"Star Name: {starData.objectName}");
            Debug.Log($"Type: {starData.starType}");
            Debug.Log($"Mass: {starData.mass} solar masses");
            Debug.Log($"Radius: {starData.radius} solar radii");
            Debug.Log($"Temperature: {starData.surfaceTemperature}K");
            Debug.Log($"Luminosity: {starData.luminosity} times the Sun's luminosity");
            Debug.Log($"Age: {starData.age} billion years");
            Debug.Log($"Lifecycle Stage: {starData.lifecycleStage}");
            Debug.Log($"Color: {starData.color}");
            Debug.Log($"Description: {starData.description}");
        }
        else
        {
            Debug.Log("No Star data assigned to this object!");
        }
    }
}

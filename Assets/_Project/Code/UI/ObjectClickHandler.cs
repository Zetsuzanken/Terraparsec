using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    [Tooltip("Reference to the data implementing ICelestialObject.")]
    public ScriptableObject objectData;

    void OnMouseDown()
    {
        if (objectData is ICelestialObject celestialObject)
        {
            UIManager.Instance.OpenScanPanel(gameObject, celestialObject);
        }
    }
}

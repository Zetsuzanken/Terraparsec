using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    public ScriptableObject objectData;

    private void OnMouseDown()
    {
        if (objectData is ICelestialObject celestialObject)
        {
            UIManager.Instance.OpenScanPanel(gameObject, celestialObject);
        }
    }
}

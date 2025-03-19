using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public SpriteClassifier spriteClassifier;

    private void Start()
    {
        foreach (ObjectClickHandler h in FindObjectsOfType<ObjectClickHandler>())
        {
            if (h.objectData is Star starData)
            {
                starData.starName = "";
                starData.AssignRandomName();

                if (h.TryGetComponent(out Warpable warpable))
                {
                    warpable.UpdateWarpableNameFromObjectData();
                }

                spriteClassifier.AssignSprite(h.gameObject);
            }
        }
    }
}

using UnityEngine;

public class Warpable : MonoBehaviour
{
    [HideInInspector]
    public string warpName;

    [HideInInspector]
    public Sprite warpIcon;

    [HideInInspector]
    public bool isStar;

    public string starSystemID;

    [HideInInspector]
    public Vector3 warpPosition;

    [HideInInspector]
    public bool visited;

    void Start()
    {
        if (string.IsNullOrEmpty(warpName))
        {
            warpName = TryGetComponent(out ObjectClickHandler handler)
                       && handler.objectData is ICelestialObject co
                       ? co.Name
                       : gameObject.name;
        }

        if (warpIcon == null && TryGetComponent(out SpriteRenderer sr))
        {
            warpIcon = sr.sprite;
        }

        isStar = TryGetComponent(out ObjectClickHandler oh) && oh.objectData is Star;

        if (warpPosition == Vector3.zero)
        {
            warpPosition = transform.position;
        }
    }

    public void UpdateWarpableNameFromObjectData()
    {
        if (TryGetComponent(out ObjectClickHandler handler)
            && handler.objectData is ICelestialObject co)
        {
            warpName = co.Name;
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewWarpDestination", menuName = "Celestial Object/Warp Destination")]
public class WarpDestination : ScriptableObject
{
    public string destinationName;
    public Sprite destinationSprite;
    public Vector2 teleportPosition;
}

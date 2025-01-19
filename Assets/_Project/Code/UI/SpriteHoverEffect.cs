using UnityEngine;

public class SpriteHoverEffect : MonoBehaviour
{
    private SpriteOutline outline;

    void Start()
    {
        outline = GetComponent<SpriteOutline>();
    }

    void OnMouseEnter()
    {
        outline?.EnableOutline();
    }

    void OnMouseExit()
    {
        outline?.DisableOutline();
    }
}

using UnityEngine;

public class SpriteHoverEffect : MonoBehaviour
{
    SpriteOutline outline;
    bool isSelected;
    private static bool disableHover = false;

    void Start()
    {
        outline = GetComponent<SpriteOutline>();
    }

    void OnMouseEnter()
    {
        if (!isSelected && !disableHover)
        {
            if (outline != null)
            {
                outline.EnableOutline();
            }
        }
    }

    void OnMouseExit()
    {
        if (!isSelected)
        {
            if (outline != null)
            {
                outline.DisableOutline();
            }
        }
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        if (isSelected)
        {
            if (outline != null)
            {
                outline.EnableOutline();
            }
        }
        else
        {
            if (outline != null)
            {
                outline.DisableOutline();
            }
        }
    }

    public static void SetHoverEnabled(bool enabled)
    {
        disableHover = !enabled;
    }
}

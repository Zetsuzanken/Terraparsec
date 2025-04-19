using UnityEngine;

public class SpriteHoverEffect : MonoBehaviour
{
    private SpriteOutline outline;
    private bool isSelected;
    private static bool disableHover = false;

    private void Start()
    {
        outline = GetComponent<SpriteOutline>();
    }

    private void OnMouseEnter()
    {
        if (!isSelected && !disableHover)
        {
            if (outline != null)
            {
                outline.EnableOutline();
            }
        }
    }

    private void OnMouseExit()
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

using UnityEngine;

// Sprite Outline Effect
// Based on an outline shader originally written by Nielson
// (formerly at http://nielson.io/2016/04/2d-sprite-outlines-in-unity/, now unavailable)
// Archived by Mandarinx: https://gist.github.com/mandarinx/f28931faa3c6a5378978a82c84d3dbcd
// Slightly modified by me for my thesis project (Terraparsec), primarily for compatibility with the latest Unity version.

public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;
    [Range(0, 16)] public int outlineSize = 1;

    SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline(false);
    }

    public void EnableOutline()
    {
        UpdateOutline(true);
    }

    public void DisableOutline()
    {
        UpdateOutline(false);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}

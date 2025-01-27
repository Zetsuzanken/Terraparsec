using UnityEngine;

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

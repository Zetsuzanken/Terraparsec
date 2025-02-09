using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarpManager : MonoBehaviour
{
    public static WarpManager Instance;

    [Header("Spaceship Reference")]
    public Transform spaceship;

    [Header("Warp Destinations")]
    public List<WarpDestination> warpDestinations = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenWarpPanel()
    {
        if (UIManager.Instance.isInteracting) return;
        UIManager.Instance.OpenWarpPanel(warpDestinations);
    }

    public void WarpTo(WarpDestination dest)
    {
        if (!PlayerResources.Instance.CanWarp()) return;
        StartCoroutine(WarpSequence(dest));
    }

    IEnumerator WarpSequence(WarpDestination dest)
    {
        UIManager.Instance.CloseAllPanels();
        PlayerResources.Instance.HandleWarp();
        Time.timeScale = 0;
        SpriteHoverEffect.SetHoverEnabled(false);
        UIManager.Instance.FadeOverlayIn();

        yield return new WaitForSecondsRealtime(UIManager.Instance.FadeOverlayDuration);

        if (spaceship != null)
        {
            spaceship.position = new Vector3(dest.teleportPosition.x, dest.teleportPosition.y, spaceship.position.z);
            if (Camera.main.TryGetComponent<CameraFollow>(out var camFollow)) camFollow.SnapToTarget();

            if (spaceship.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }

            Transform newClosestObject = PlayerDistanceMonitor.Instance.FindClosestObject();
            PlayerDistanceMonitor.Instance.UpdateClosestObject(newClosestObject);
        }

        UIManager.Instance.FadeOverlayOut();
        SpriteHoverEffect.SetHoverEnabled(true);
        Time.timeScale = 1;
    }
}

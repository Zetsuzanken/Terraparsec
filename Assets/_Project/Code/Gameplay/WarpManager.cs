using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarpManager : MonoBehaviour
{
    public static WarpManager Instance;

    public Transform spaceship;

    private readonly List<Warpable> allWarpables = new();

    private Warpable earthWarpable;
    private Warpable currentDepartingObject = null;
    private bool hasLeftEarth = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        allWarpables.AddRange(FindObjectsOfType<Warpable>());
        GameObject earthObj = GameObject.FindGameObjectWithTag("Finish");

        if (earthObj != null && earthObj.TryGetComponent(out Warpable earthW))
        {
            earthWarpable = earthW;
        }
    }

    public void OpenWarpPanel(Warpable currentObject)
    {
        if (UIManager.Instance.isInteracting)
        {
            return;
        }

        currentDepartingObject = currentObject;
        List<Warpable> validDestinations = GetValidWarpDestinations(currentObject);
        UIManager.Instance.OpenWarpPanel(validDestinations);
    }

    private List<Warpable> GetValidWarpDestinations(Warpable current)
    {
        List<Warpable> results = new();

        if (IsEarth(current))
        {
            foreach (Warpable w in allWarpables)
            {
                if (w == current)
                {
                    continue;
                }

                if (w.isStar)
                {
                    results.Add(w);
                }
            }
            return results;
        }

        if (current.isStar)
        {
            foreach (Warpable w in allWarpables)
            {
                if (w == current)
                {
                    continue;
                }

                if (w.starSystemID == current.starSystemID || w.isStar || IsEarth(w))
                {
                    results.Add(w);
                }
            }
        }
        else
        {
            foreach (Warpable w in allWarpables)
            {
                if (w != current && w.starSystemID == current.starSystemID)
                {
                    results.Add(w);
                }
            }
        }

        return results;
    }

    private bool IsEarth(Warpable w)
    {
        return w == earthWarpable;
    }

    public void WarpTo(Warpable dest)
    {
        StartCoroutine(WarpSequence(dest));
    }

    private IEnumerator WarpSequence(Warpable dest)
    {
        UIManager.Instance.CloseAllPanels();
        bool departingEarthForTheFirstTime = (!hasLeftEarth && currentDepartingObject != null && IsEarth(currentDepartingObject));

        if (departingEarthForTheFirstTime)
        {
            PlayerResources.Instance.UseEnergy(PlayerResources.Instance.warpEnergyCost);
            hasLeftEarth = true;
            PlayerResources.Instance.StartTimer();
        }
        else
        {
            PlayerResources.Instance.HandleWarp();
        }

        if (UIManager.Instance.gameHasEnded)
        {
            yield break;
        }

        Time.timeScale = 0;
        SpriteHoverEffect.SetHoverEnabled(false);
        UIManager.Instance.FadeOverlayIn();

        yield return new WaitForSecondsRealtime(UIManager.Instance.FadeOverlayDuration);

        if (spaceship != null)
        {
            spaceship.position = new Vector3(dest.warpPosition.x, dest.warpPosition.y - 2, spaceship.position.z);

            if (Camera.main.TryGetComponent(out CameraFollow camFollow))
            {
                camFollow.SnapToTarget();
            }

            if (spaceship.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }

            if (!dest.isStar)
            {
                dest.visited = true;
            }

            Transform newClosestObject = PlayerDistanceMonitor.Instance.FindClosestObject();
            PlayerDistanceMonitor.Instance.UpdateClosestObject(newClosestObject);
        }

        UIManager.Instance.FadeOverlayOut();
        SpriteHoverEffect.SetHoverEnabled(true);
        Time.timeScale = 1;
        Input.ResetInputAxes();
        currentDepartingObject = null;
    }
}

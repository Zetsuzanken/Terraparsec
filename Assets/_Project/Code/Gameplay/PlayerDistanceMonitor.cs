using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceMonitor : MonoBehaviour
{
    public static PlayerDistanceMonitor Instance;

    public List<Transform> celestialObjects = new();
    public float maxAllowedDistance = 15f;

    private Transform closestObject;
    private Transform spaceship;
    private bool warningActive = false;

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
        spaceship = GameObject.FindGameObjectWithTag("Player").transform;

        UpdateClosestObject(FindClosestObject());
    }

    private void Update()
    {
        if (closestObject == null)
        {
            return;
        }

        float distance = Vector2.Distance(spaceship.position, closestObject.position);

        if (distance > maxAllowedDistance && !warningActive)
        {
            warningActive = true;
            UIManager.Instance.ShowWarningPanel(closestObject);
        }
        else if (distance <= maxAllowedDistance && warningActive)
        {
            warningActive = false;
        }
    }

    public Transform FindClosestObject()
    {
        float minDistance = Mathf.Infinity;
        Transform nearest = null;
        foreach (Transform obj in celestialObjects)
        {
            float dist = Vector2.Distance(spaceship.position, obj.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = obj;
            }
        }
        return nearest;
    }

    public Warpable GetClosestWarpable()
    {
        if (closestObject == null)
        {
            return null;
        }

        _ = closestObject.TryGetComponent<Warpable>(out Warpable w);
        return w;
    }

    public void UpdateClosestObject(Transform newClosest)
    {
        closestObject = newClosest;
        warningActive = false;
    }
}

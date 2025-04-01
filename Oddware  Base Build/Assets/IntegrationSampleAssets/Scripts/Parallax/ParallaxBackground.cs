using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxBackground : MonoBehaviour
{
    public event Action OnStopParallax;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Transform center;
    [SerializeField] private float centerBounds;
    private ParallaxManager parallaxManager;
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    private BoxCollider2D colliderBounds;
    private Vector3 camBoundRightPoint;
    private Vector3 camBoundLeftPoint;
    private Coroutine lastLeftReset;
    private Coroutine lastRightReset;
    private CinemachineConfiner2D vCamConfiner;
    private Vector3 topRightCorner;
    private Vector3 bottomLeftCorner;
    private Vector3 initialPos;
    private Vector3 boundsPosRight;
    private Vector3 boundsPosLeft;
    private float theX;
    [SerializeField] private float diffThreshold = 0.01f;
    [SerializeField] private float snapSpeed = 2;

    void Awake()
    {
        colliderBounds = GetComponentInChildren<BoxCollider2D>();
        if (vCam != null)
        {
            vCamConfiner = vCam.GetComponent<CinemachineConfiner2D>();
            vCamConfiner.m_BoundingShape2D = colliderBounds;
        }

        if (parallaxManager == null)
            parallaxManager = FindObjectOfType<ParallaxManager>();
        Init();
        SetLayers();
    }

    private void OnEnable()
    {
        if (parallaxManager != null)
        {
            parallaxManager.OnCameraTranslate += Move;

            //Handle camera bounds
            parallaxManager.OnCameraTranslate += ApplyBounds;
            OnStopParallax += parallaxManager.StopParallax;
        }
    }

    private void OnDisable()
    {
        if (parallaxManager != null)
        {
            parallaxManager.OnCameraTranslate -= Move;

            parallaxManager.OnCameraTranslate -= ApplyBounds;
            OnStopParallax -= parallaxManager.StopParallax;
        }
    }

    private void Init()
    {
        float zDistance =
            Mathf.Abs(colliderBounds.transform.position.z - parallaxManager.ReferenceCam.transform.position.z);
        camBoundRightPoint = GetUpperRightCornerWorldPosition(parallaxManager.ReferenceCam, zDistance);
        camBoundLeftPoint = GetLowerLeftCornerWorldPosition(parallaxManager.ReferenceCam, zDistance);
    }

    private Vector3 GetLowerLeftCornerWorldPosition(Camera parallaxManagerReferenceCam, float zDistance)
    {
        // The lower left corner in viewport space is (0, 0)
        Vector3 viewportPoint = new Vector3(0, 0, zDistance);

        // Convert the viewport point to world space
        Vector3 worldPosition = parallaxManagerReferenceCam.ViewportToWorldPoint(viewportPoint);

        return worldPosition;
    }

    private void SetLayers()
    {
        parallaxLayers.Clear();
        parallaxLayers = GetComponentsInChildren<ParallaxLayer>().ToList();
        for (int i = 0; i < parallaxLayers.Count; i++)
        {
            parallaxLayers[i].Manager = parallaxManager;
        }
    }

    private void Move(float delta)
    {
        Vector3 newPos = transform.position;
        newPos.x += delta * parallaxManager.XParallaxScale;

        transform.position = newPos;

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }

    private void MoveLiteral(float distance)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(distance);
        }
    }
    
    private void ApplyBounds(float delta)
    {
        if (lastRightReset != null) StopCoroutine(lastRightReset);
        if (lastLeftReset != null) StopCoroutine(lastLeftReset);

        Bounds bounds = colliderBounds.bounds;
        topRightCorner = new Vector3(bounds.max.x, bounds.max.y, colliderBounds.transform.position.z);
        bottomLeftCorner = new Vector3(bounds.min.x, bounds.min.y, colliderBounds.transform.position.z);
        Debug.Log($"Cam bound right point {camBoundRightPoint}");
        Debug.Log($"URight from bounds point {topRightCorner}");

        Debug.Log($"Cam bound left point {camBoundLeftPoint}");
        Debug.Log($"DLeft from bounds point {bottomLeftCorner}");
        Debug.Log($"Delta greater than zero {delta > 0} {delta} left");
        Debug.Log($"Delta greater than zero {delta > 0} {delta} right");
        if (topRightCorner.x <= camBoundRightPoint.x && delta < 0)
        {
            Debug.Log($"Too far left");
            if (lastLeftReset != null) StopCoroutine(lastLeftReset);
            lastLeftReset = StartCoroutine(ResetRight());
        }

        if (bottomLeftCorner.x >= camBoundLeftPoint.x && delta > 0)
        {
            Debug.Log($"Too far right");
            if (lastRightReset != null) StopCoroutine(lastRightReset);
            lastRightReset =StartCoroutine(ResetLeft());
        }
    }

    private IEnumerator ResetLeft()
    {
        OnStopParallax?.Invoke();
        Bounds bounds = colliderBounds.bounds;
        bottomLeftCorner = new Vector3(bounds.min.x, bounds.min.y, colliderBounds.transform.position.z);
        float diff = camBoundLeftPoint.x - bottomLeftCorner.x;
        Move(diff);

        while (Mathf.Abs(diff) > diffThreshold)
        {
            bounds = colliderBounds.bounds;
            bottomLeftCorner = new Vector3(bounds.min.x, bounds.min.y, colliderBounds.transform.position.z);
            diff = camBoundLeftPoint.x - bottomLeftCorner.x;
            Move(diff * snapSpeed);
            yield return null;
        }
 yield break;
    }

    private IEnumerator ResetRight()
    {
        OnStopParallax?.Invoke();
        Bounds bounds = colliderBounds.bounds;
        topRightCorner = new Vector3(bounds.max.x, bounds.max.y, colliderBounds.transform.position.z);
        float diff = camBoundRightPoint.x - topRightCorner.x;
        Move(diff);

        while (Mathf.Abs(diff) > diffThreshold)
        {
            bounds = colliderBounds.bounds;
            topRightCorner = new Vector3(bounds.max.x, bounds.max.y, colliderBounds.transform.position.z);
            bottomLeftCorner = new Vector3(bounds.min.x, bounds.min.y, colliderBounds.transform.position.z);
            diff = camBoundRightPoint.x - topRightCorner.x;
            Move(diff * snapSpeed);

            yield return null;
        }
        yield break;

    }

    public Vector3 GetUpperRightCornerWorldPosition(Camera camera, float zDistance)
    {
        // The upper right corner in viewport space is (1, 1)
        Vector3 viewportPoint = new Vector3(1, 1, zDistance);

        // Convert the viewport point to world space
        Vector3 worldPosition = camera.ViewportToWorldPoint(viewportPoint);

        return worldPosition;
    }

    public bool AreFloatsClose(float a, float b)
    {
        // Check if the difference between the two floats is smaller than Mathf.Epsilon
        return Mathf.Abs(a - b) < Mathf.Epsilon + 0.3f;
    }
}
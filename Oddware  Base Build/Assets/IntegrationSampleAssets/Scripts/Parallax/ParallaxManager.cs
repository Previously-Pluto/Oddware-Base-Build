using System;
using System.Collections;
using Cinemachine;
using Highlighters_BuiltIn;
using Naninovel;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParallaxManager : MonoBehaviour
{
    public event Action OnInitialized;

    public delegate void ParallaxCameraDelegate(float deltaMovement);

    public ParallaxCameraDelegate OnCameraTranslate;

    [SerializeField] private InputActionReference mouseClick;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float decayRate = 0.3f;
    [SerializeField] private float _xParallaxScale = 0.1f;
    [SerializeField] private float _xLayerParallaxScale = 0.1f;

    private bool clickHeld;
    public Camera ReferenceCam { get; set; }
    private Coroutine lastCoroutine;
    private Coroutine lastUpCoroutine;
    private Coroutine lastDecayCancelCoroutine;

    private float lastDelta;
    public float XParallaxScale => _xParallaxScale;
    public float XLayerParallaxScale => _xLayerParallaxScale;
    public CinemachineVirtualCamera VCam => vCam;

    private void Awake()
    {
        Engine.OnInitializationFinished += () =>
        {
            ReferenceCam = Engine.GetService<ICameraManager>().Camera;
            OnInitialized?.Invoke();
        };
    }

    private void OnEnable()
    {
        mouseClick.action.started += HandleMouseDown;
        mouseClick.action.canceled += HandleMouseUp;
        mouseClick.action.Enable();
        OnCameraTranslate += HandleDecayCancel;
        OnCameraTranslate += MoveVCam;
    }

    private void OnDisable()
    {
        mouseClick.action.started -= HandleMouseDown;
        mouseClick.action.canceled -= HandleMouseUp;
        mouseClick.action.Disable();
        OnCameraTranslate -= HandleDecayCancel;
        OnCameraTranslate -= MoveVCam;
    }

    private void MoveVCam(float deltamovement)
    {
        Vector3 newPos = vCam.transform.position;
        newPos.x += deltamovement * XParallaxScale;
        vCam.transform.position = newPos;
    }

    private void HandleMouseDown(InputAction.CallbackContext obj)
    {
        clickHeld = true;
        lastDelta = 0;
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        if (lastUpCoroutine != null) StopCoroutine(lastUpCoroutine);
        if (lastDecayCancelCoroutine != null) StopCoroutine(lastDecayCancelCoroutine);

        lastCoroutine = StartCoroutine(ParallaxCalcSequence());
    }

    private void HandleMouseUp(InputAction.CallbackContext obj)
    {
        clickHeld = false;
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        if (lastUpCoroutine != null) StopCoroutine(lastUpCoroutine);
        if (lastDecayCancelCoroutine != null) StopCoroutine(lastDecayCancelCoroutine);
        lastUpCoroutine = StartCoroutine(ParallaxDecaySequence());
    }

    public void StopParallax()
    {
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        if (lastUpCoroutine != null) StopCoroutine(lastUpCoroutine);
    }

    private IEnumerator ParallaxCalcSequence()
    {
        float oldPosition = Input.mousePosition.x;
        while (true)
        {
            if (Input.mousePosition.x != oldPosition)
            {
                float delta = Input.mousePosition.x - oldPosition;

                // if (delta < 0) //Debug.Log($"Parallaxing left");
                // if (delta > 0) //Debug.Log($"Parallaxing right");

                OnCameraTranslate?.Invoke(delta);
                lastDelta = delta;
                oldPosition = Input.mousePosition.x;
            }

            yield return null;
        }
    }

    private void HandleDecayCancel(float deltaMovement)
    {
        if (lastDecayCancelCoroutine != null) StopCoroutine(lastDecayCancelCoroutine);
        lastDecayCancelCoroutine = StartCoroutine(ParallaxDecayCancelSequence());
    }

    private IEnumerator ParallaxDecayCancelSequence()
    {
        yield return new WaitForSeconds(0.1f);
        lastDelta = 0;
    }

    /// <summary>
    /// When the mouse is lifted up,
    /// continue to parallax for a duration.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ParallaxDecaySequence()
    {
        float deltaValue = Mathf.Abs(lastDelta);
        while (deltaValue > 0)
        {
            if (lastDelta < 0)
            {
                //Debug.Log($"Parallaxing decay left");
                lastDelta += Time.deltaTime * decayRate;
            }

            if (lastDelta > 0)
            {
                //Debug.Log($"Parallaxing decay right");
                lastDelta -= Time.deltaTime * decayRate;
            }

            OnCameraTranslate?.Invoke(lastDelta);

            deltaValue -= Time.deltaTime * decayRate;
            yield return null;
        }
    }
}
using System.Collections;
using Naninovel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TipsNotificationUI : MonoBehaviour
{
    public enum PositionFlags
    {
        BottomLeft,
        TopLeft
    }

    [SerializeField] private PositionFlags tipPosition;
    [SerializeField] private CanvasGroup tipNotificationUI;
    [SerializeField] private TMP_Text tipTitle;
    
    [SerializeField] private float tipMaxHeightScale = 0.1f;
    [SerializeField] private float tipMinHeightScale = 0.1f;
    [SerializeField] private float tipMaxHeight;
    [SerializeField] private float tipMinHeight;
    
    [SerializeField] private float animSpeed = 1;
    [SerializeField] private float tipHangDuration = 2f;
    [SerializeField] private string unlockedTipPrefix = "New Tip:";
    private float currentAnimTime;
    private IUnlockableManager unlockableManager;
    private ITextManager textManager;
    private Coroutine lastRoutine;
    private Coroutine lastDownRoutine;

    private void Awake()
    {
        if (tipPosition == PositionFlags.BottomLeft)
        {
            tipMinHeight = -(Screen.height * tipMinHeightScale);   
            tipMaxHeight = Screen.height * tipMaxHeightScale;
        }

        if (tipPosition == PositionFlags.TopLeft)
        {
            tipMinHeight = (Screen.height * tipMinHeightScale);   
            tipMaxHeight = Screen.height - (Screen.height * tipMaxHeightScale);
        }
        
        unlockableManager = Engine.GetService<IUnlockableManager>();
        textManager = Engine.GetService<ITextManager>();

        ResetPosition();
    }

    [ContextMenu("Reset Noti Position")]
    public void ResetPosition()
    {
        RectTransform rect = tipNotificationUI.GetComponent<RectTransform>();
        Vector2 newPos = rect.anchoredPosition;
        newPos.y = tipMinHeight;
        rect.anchoredPosition = newPos;
    }

    private void OnEnable()
    {
        unlockableManager.OnItemUpdated += HandleUnlockableUpdate;
    }
    
    private void OnDisable()
    {
        unlockableManager.OnItemUpdated -= HandleUnlockableUpdate;
    }

    [ContextMenu("Test Up Anim")]
    public void TestUpAnim()
    {
        if(lastRoutine != null) StopCoroutine(lastRoutine);
        lastRoutine = StartCoroutine(TipFlashSequence());
    }
    
    private void HandleUnlockableUpdate(UnlockableItemUpdatedArgs obj)
    {
        if (obj.Unlocked)
        {
            SetNotificationData(obj);
            if(lastRoutine != null) StopCoroutine(lastRoutine);
            lastRoutine = StartCoroutine(TipFlashSequence());
        }
    }

    private void SetNotificationData(UnlockableItemUpdatedArgs obj)
    {
        var recordValue = textManager.GetRecordValueWithFallback(obj.Id.GetAfterFirst($"Tips/"), ManagedTextConfiguration.TipCategory);
        tipTitle.text = unlockedTipPrefix + " " + (recordValue.GetBefore("|")?.Trim() ?? recordValue);
    }
    
    private IEnumerator TipFlashSequence()
    {
        RectTransform rect = tipNotificationUI.GetComponent<RectTransform>();
        currentAnimTime = 0;
        float currentHeight = 0;
        Vector2 newPos = rect.anchoredPosition;
        if (tipPosition == PositionFlags.BottomLeft)
        {
            tipMinHeight = -(Screen.height * tipMinHeightScale);   
            tipMaxHeight = Screen.height * tipMaxHeightScale;
        }

        if (tipPosition == PositionFlags.TopLeft)
        {
            tipMinHeight = (Screen.height * tipMinHeightScale);   
            tipMaxHeight = Screen.height - (Screen.height * tipMaxHeightScale);
        }
        
        while (currentAnimTime < 1)
        {
            currentHeight = Mathf.Lerp(tipMinHeight, tipMaxHeight, currentAnimTime);
            newPos.y = currentHeight;
            rect.anchoredPosition = newPos;
            currentAnimTime += Time.deltaTime * animSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(tipHangDuration);
        if(lastDownRoutine != null) StopCoroutine(lastDownRoutine);
         lastDownRoutine = StartCoroutine(TipDownAnimSequence());
    }
    
    private IEnumerator TipDownAnimSequence()
    {
        RectTransform rect = tipNotificationUI.GetComponent<RectTransform>();
        currentAnimTime = 0;
        float currentHeight = 0;
        Vector2 newPos = rect.anchoredPosition;
        
        while (currentAnimTime < 1)
        {
            currentHeight = Mathf.Lerp(tipMaxHeight, tipMinHeight, currentAnimTime);
            newPos.y = currentHeight;
            rect.anchoredPosition = newPos;
            currentAnimTime += Time.deltaTime * animSpeed;
            yield return null;
        }
    }
}
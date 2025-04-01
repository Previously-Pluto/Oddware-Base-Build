using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Naninovel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ContentPlayer : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private RectTransform contentRectTransform;
    [SerializeField] private float maxHeightScale = 0.6f;
    [SerializeField] private float minHeightScale = 0;
    [SerializeField] private float expandDuration = 0.6f;
    [SerializeField] private float collapseDuration = 0.6f;

    [SerializeField] private UnityEvent onContentOpen;
    private float maxHeight;
    private float minHeight;
    private Coroutine lastRoutine;

    private void Awake()
    {
        maxHeight = Screen.height * maxHeightScale;
        minHeight = Screen.height * minHeightScale;
    }

    private void OnEnable()
    {
        exitButton.onClick.AddListener(() => { Close(); });
    }

    private void OnDisable()
    {
        exitButton.onClick.RemoveAllListeners();
    }

    [ContextMenu("Open")]
    public void Open()
    {
        //ToggleCanvasGroup(mainUI, true);
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);
        lastRoutine = StartCoroutine(PlayOpenAnim());
        onContentOpen?.Invoke();
    }

    [ContextMenu("Close")]
    public void Close()
    {
        //ToggleCanvasGroup(mainUI, false);
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);
        lastRoutine = StartCoroutine(PlayCloseAnim());
    }

    public IEnumerator PlayOpenAnim()
    {
        maxHeight = Screen.height * maxHeightScale;
        float initialHeight = contentRectTransform.rect.height; // Current height of the RectTransform
        Vector2 initialOffsetMin = contentRectTransform.offsetMin; // Initial offsetMin value

        // Calculate the final bottom offset needed to reach the target height
        float targetBottomOffset = initialOffsetMin.y - (maxHeight - initialHeight);

        float elapsedTime = 0f;

        while (elapsedTime < expandDuration)
        {
            // Lerp the bottom offset value over time
            float newBottomOffset = Mathf.Lerp(initialOffsetMin.y, targetBottomOffset, elapsedTime / expandDuration);
            contentRectTransform.offsetMin = new Vector2(initialOffsetMin.x, newBottomOffset);

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        // Ensure the final value is set
        contentRectTransform.offsetMin = new Vector2(initialOffsetMin.x, targetBottomOffset);
    }

    public IEnumerator PlayCloseAnim()
    {
        minHeight = Screen.height * minHeightScale;
        float initialHeight = contentRectTransform.rect.height; // Current height of the RectTransform
        Vector2 initialOffsetMin = contentRectTransform.offsetMin; // Initial offsetMin value

        // Calculate the final bottom offset needed to collapse to the target height
        float targetBottomOffset = initialOffsetMin.y + (initialHeight - minHeight);

        float elapsedTime = 0f;

        while (elapsedTime < collapseDuration)
        {
            // Lerp the bottom offset value over time to collapse
            float newBottomOffset = Mathf.Lerp(initialOffsetMin.y, targetBottomOffset, elapsedTime / collapseDuration);
            contentRectTransform.offsetMin = new Vector2(initialOffsetMin.x, newBottomOffset);

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        // Ensure the final value is set
        contentRectTransform.offsetMin = new Vector2(initialOffsetMin.x, targetBottomOffset);
    }
}
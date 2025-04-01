using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PositionController : MonoBehaviour
{
    [SerializeField]
    private float leftBoundsOffset = 0.0f;

    [SerializeField]
    private float rightBoundsOffset = 0.0f;

    [SerializeField]
    private float bottomBoundsOffset = 0.0f;

    [SerializeField]
    private float topBoundsOffset = 0.0f;

    [SerializeField] private RectTransform boundaryRect;

    [Tooltip(
        "The 'Width' of the Tooltip game object. This is how Console Cursors determines the size boundaries at runtime.")]
    public int toolTipWidth = 770;

    [Tooltip(
        "The 'Height' of the Tooltip game object. This is how Console Cursors determines the size boundaries at runtime.")]
    public int toolTipHeight = 245;

    [FormerlySerializedAs("toolTipOffsetX")]
    [Tooltip(
        "The Pos X of the game object 'container' that is used to re-position the tooltip so it always stays visible on the screen. Adjusting this value will adjust how far left or right the tooltip positions itself when re-aligning.")]
    public int rectOffsetX = 468;

    [FormerlySerializedAs("toolTipOffsetY")]
    [Tooltip(
        "The Pos Y of the game object 'container' that is used to re-position the tooltip so it always stays visible on the screen. Adjusting this value will adjust how far left or right the tooltip positions itself when re-aligning.")]
    public int rectOffsetY = -206;

    private float XMin { get; set; }
    private float XMax { get; set; }
    private float YMin { get; set; }
    private float YMax { get; set; }
    private bool tooFarRight = false;
    private bool tooFarLeft = false;

    private void Awake()
    {
        XMin = (rightBoundsOffset);
        XMax = boundaryRect.rect.width - rightBoundsOffset;
        YMin = topBoundsOffset;
        YMax = boundaryRect.rect.height - topBoundsOffset;
    }

    public Vector2 GetPosition(RectTransform refTransform, Vector2 pointerPosition)
    {
        Vector2 currentXAxisPlacement = Vector2.zero;
        Vector2 currentYAxisPlacement = Vector2.zero;
        Debug.Log($"Mouse pos {pointerPosition}");
        float xThreshold = Screen.width - refTransform.rect.width;
        float yThreshold = Screen.height - refTransform.rect.height;

        if (pointerPosition.x >= XMax - refTransform.rect.width)
        {
            float offset = pointerPosition.x - XMax;
            currentXAxisPlacement = (refTransform.transform.right * (leftBoundsOffset - offset));
        }
        else if(pointerPosition.x <= XMin)
        {
            float offset = XMin - pointerPosition.x;
            currentXAxisPlacement = (refTransform.transform.right * (rightBoundsOffset + offset));
        }
        else
        {
            currentXAxisPlacement = (refTransform.transform.right * rightBoundsOffset);
        }

        if (pointerPosition.y >= YMax)
        {
            float offset = pointerPosition.y - YMax ;
            currentYAxisPlacement = (refTransform.transform.up * (bottomBoundsOffset - offset));
        }
        else if(pointerPosition.y <= YMin)
        {
            float offset = YMin - pointerPosition.y;
            currentYAxisPlacement = (refTransform.transform.up * (topBoundsOffset + offset));
        }
        else
        {
            currentYAxisPlacement = (refTransform.transform.up * topBoundsOffset);
        }

        Vector2 result = pointerPosition + currentXAxisPlacement + currentYAxisPlacement;
        Debug.Log($"Result pos {result}");

        return result;
    }

    public Vector2 GetPosition2(RectTransform refTransform, Vector3 pointerPosition)
    {
        Vector2 rectPosition = new Vector2();
        if (refTransform.anchoredPosition.x <= XMin + (refTransform.rect.width + 70))
        {
            // Too Far left
            tooFarLeft = true;
            if (refTransform.anchoredPosition.y <= YMin + (refTransform.rect.height + 55))
                rectPosition = pointerPosition + new Vector3(rectOffsetX, -rectOffsetY, 100);
            else
                rectPosition = pointerPosition + new Vector3(rectOffsetX, rectOffsetY, 100);
        }
        else
            tooFarLeft = false;

        if (refTransform.anchoredPosition.x >= XMax - (refTransform.rect.width + 70))
        {
            // Too Far Right
            tooFarRight = true;
            if (refTransform.anchoredPosition.y <= YMin + (refTransform.rect.height + 55))
                rectPosition = pointerPosition + new Vector3(-rectOffsetX, -rectOffsetY, 100);
            else
                rectPosition = pointerPosition + new Vector3(-rectOffsetX, rectOffsetY, 100);
        }
        else
            tooFarRight = false;

        if (refTransform.anchoredPosition.y <= YMin + (refTransform.rect.height + 55))
            if (!tooFarRight) rectPosition = pointerPosition + new Vector3(rectOffsetX, -rectOffsetY, 100);
            else if (!tooFarRight) rectPosition = pointerPosition + new Vector3(rectOffsetX, rectOffsetY, 100);

        return rectPosition;
    }
}
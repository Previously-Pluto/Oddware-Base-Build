using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenuInputBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ContextMenuManager ContextMenuManager { get; set; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ContextMenuManager.ToggleInput(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ContextMenuManager.ToggleInput(true);
    }
}
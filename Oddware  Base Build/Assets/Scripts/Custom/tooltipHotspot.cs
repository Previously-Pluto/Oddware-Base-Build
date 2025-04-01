using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class tooltipHotspot : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToolTipObj;
    public TextMeshProUGUI theTMPgameobject;
    // the image you want to fade, assign in inspector
    public Image img;

    void Start()
    {
        img.color = new Color(1, 1, 1, 0);
        theTMPgameobject.color = new Color(1, 1, 1, 0);
    }
 
    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                if(theTMPgameobject){
                    theTMPgameobject.color = new Color(1, 1, 1, i);
                }
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime * 3)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                if(theTMPgameobject){
                    theTMPgameobject.color = new Color(1, 1, 1, i);
                }
                yield return null;
            }
        }
    }

     public void OnSelect(BaseEventData eventData)
     {
         //Debug.Log("Selected");
         //ToolTipObj.SetActive(true);
         StartCoroutine(FadeImage(false));
     }
     public void OnDeselect(BaseEventData eventData)
     {
         //Debug.Log("De-Selected");
         //ToolTipObj.SetActive(false);
         StartCoroutine(FadeImage(true));
     }
     public void OnPointerEnter(PointerEventData eventData)
     {
         //Debug.Log("Pointer Enter");
         //ToolTipObj.SetActive(true);
         StartCoroutine(FadeImage(false));
     }
     public void OnPointerExit(PointerEventData eventData)
     {
         //Debug.Log("Pointer Exit");
         //ToolTipObj.SetActive(false);
         StartCoroutine(FadeImage(true));
     }


}

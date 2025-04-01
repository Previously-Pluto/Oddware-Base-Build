using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class buttonRolloverSound : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

  public AudioClip pressedSound;
  public AudioClip rolloverSound;
  public Texture2D cursorHover;

  private AudioSource audioSource;

  private void Awake()
  {
      audioSource = GetComponent<AudioSource>();
  }


  public void OnPointerEnter(PointerEventData eventData)
   {
     audioSource.clip = rolloverSound;
     Cursor.SetCursor(cursorHover, Vector2.zero, CursorMode.Auto);
     audioSource.Play();
   }
   public void OnPointerExit(PointerEventData eventData)
   {
       Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
       //Debug.Log("The cursor entered the selectable UI element.");
   }

    public void OnSelect(BaseEventData eventData)
    {
     audioSource.clip = rolloverSound;
     Cursor.SetCursor(cursorHover, Vector2.zero, CursorMode.Auto);
     audioSource.Play();
    }
    public void OnDeselect(BaseEventData eventData)
    {
       Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
       //Debug.Log("The cursor entered the selectable UI element.");
    }

   //Detect if a click occurs
   public void OnPointerClick(PointerEventData pointerEventData)
   {
       audioSource.clip = pressedSound;
       audioSource.Play();
   }
    public void OnClick(BaseEventData eventData)
    {
       audioSource.clip = pressedSound;
       audioSource.Play();
    }
    
}

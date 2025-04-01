using AdventurePuzzleKit;
using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExamineSystem;
using NoteSystem;

public class ItemsEventHandler : MonoBehaviour
{

    public GameObject PuzzleCreatorKit;
    public Transform m_itemsContainer;
    public void ShowItem(StringEventListener _eventListener)
    {
        PuzzleCreatorKit.SetActive(false);
        foreach(Transform t in m_itemsContainer)
        {
            t.gameObject.SetActive(false);
            foreach(Transform c in t)
            {
                if(c.gameObject.name.ToLower().Equals(_eventListener.Event.m_previousValue.ToLower()))
                {
                    t.gameObject.SetActive(true);
                    c.gameObject.SetActive(true);
                    GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<ExamineItemController>().Init();
                    GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<AKItemController>().Init();
                    GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<AKItemController>().InteractionType();
                }
                foreach(Transform cc in c)
                {
                    if (cc.gameObject.name.ToLower().Equals(_eventListener.Event.m_previousValue.ToLower()))
                    {
                        t.gameObject.SetActive(true);
                        c.gameObject.SetActive(true);
                        cc.gameObject.SetActive(true);
                        if(GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<SingleObjectHighlight>() != null)
                            GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<SingleObjectHighlight>().Init();
                        if(GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<NormalCustomNoteController>() != null)
                            GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<NormalCustomNoteController>().Init();
                        GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<AKItemController>().Init();
                        GameObject.Find(_eventListener.Event.m_previousValue).GetComponent<AKItemController>().InteractionType();
                    }

                }
            }
        }
    }

    public void ActivateNaninovel()
    {
        GetComponent<PlayScript>().Play();
    }
}

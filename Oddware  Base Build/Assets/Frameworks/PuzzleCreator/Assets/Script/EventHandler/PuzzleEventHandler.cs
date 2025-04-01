using AdventurePuzzleKit;
using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEventHandler : MonoBehaviour
{
    public GameObject PuzzleCreatorKit;
    public GameObject m_mainCamera;
    public Transform m_itemsContainer;
    public void PlayPuzzle(StringEventListener _eventListener)
    {
        PuzzleCreatorKit.SetActive(true);
        GameObject.Find("PuzzlesContainer").transform.Find(_eventListener.Event.m_previousValue).gameObject.SetActive(true);
        AP_GlobalPuzzleManager_Pc _apGlobalPuzzleManager = GameObject.Find("GlobalPuzzleManager").GetComponent<AP_GlobalPuzzleManager_Pc>();
        _apGlobalPuzzleManager.m_currentPuzzle = GameObject.Find("PuzzlesContainer").transform.Find(_eventListener.Event.m_previousValue);
        AP_PuzzleRaycast_Pc objPuzzleRaycast = null;
        Transform puzzleTransform = GameObject.Find("PuzzlesContainer").transform.Find(_eventListener.Event.m_previousValue);
        if (puzzleTransform.Find("PuzzleDetector") != null)
        {
            _apGlobalPuzzleManager.currentPuzzle = GameObject.Find("PuzzlesContainer").transform.Find(_eventListener.Event.m_previousValue).Find("PuzzleDetector").GetComponent<AP_PuzzleDetector_Pc>();
            objPuzzleRaycast = GameObject.Find("puzzleRaycast").GetComponent<AP_PuzzleRaycast_Pc>();
        }
        else
        {
            foreach (Transform t in puzzleTransform)
            {
                if (t.Find("PuzzleDetector") != null)
                {
                    _apGlobalPuzzleManager.currentPuzzle = t.Find("PuzzleDetector").GetComponent<AP_PuzzleDetector_Pc>();
                    objPuzzleRaycast = GameObject.Find("puzzleRaycast").GetComponent<AP_PuzzleRaycast_Pc>();
                }
            }
        }
        objPuzzleRaycast.SetCurrentPuzzle(_apGlobalPuzzleManager.currentPuzzle);
        objPuzzleRaycast.AP_ActivatePuzzle();
    }

    public void PlayAKPuzzle(StringEventListener _eventListener)
    {
        foreach (Transform t in m_itemsContainer)
        {
            foreach (Transform c in t)
            {
                foreach(Transform cc in c)
                {
                    if (cc.gameObject.name.ToLower().Equals(_eventListener.Event.m_previousValue.ToLower()))
                    {
                        t.gameObject.SetActive(true);
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

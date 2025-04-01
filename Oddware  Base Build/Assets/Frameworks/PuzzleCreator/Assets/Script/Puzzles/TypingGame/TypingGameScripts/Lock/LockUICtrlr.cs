using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockUICtrlr : MonoBehaviour
{
    public TextMeshProUGUI _timerText;
    public TextMeshProUGUI _lockIndexText;
    

    public void SetLockInformation(float _time, int _lockIndex)
    {

        SetTimer(_time);
        _lockIndexText.text = _lockIndex.ToString();    //set the lock index
    }

    public void SetTimer(float _time)
    {
        string minutes = Mathf.Abs(Mathf.Floor(_time / 60)).ToString("00");
        string seconds = (_time % 60).ToString("00");
        string fraction = ((_time * 100) % 100).ToString("00");
        _timerText.text = minutes + ":" + seconds;  //set the timer text
    }

    /// <summary>
    /// Desactivated the current lock
    /// </summary>
    public void DesactivateCurrentLock()
    {
        _lockIndexText.transform.parent.gameObject.SetActive(false); 
    }
}

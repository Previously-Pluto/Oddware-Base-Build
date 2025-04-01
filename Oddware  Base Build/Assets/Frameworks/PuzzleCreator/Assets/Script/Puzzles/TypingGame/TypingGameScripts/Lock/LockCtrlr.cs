using System.Collections;
using System.Collections.Generic;
using TypingGameKit.Demo;
using UnityEngine;

public class LockCtrlr : MonoBehaviour
{
    public List<LockStruct> _locks;
    public Transform _locksContainer;
    private int _currentLockIndex;
    public bool _timerIsSet;
    float _timer;
    public GameObject _lockUI;
    public GameObject _endScr;
    void Start()
    {
        _locks = GetComponent<DemoManager>()._locks;
        SetLockUI();
        SetLocksInformation(_locks);
    }

    private void Update()
    {
        if(_timerIsSet)
        {
            if(_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }

            if(_timer <= 0f)
            {
                _endScr.SetActive(true);
            }
            SetTimer();
        }
    }

    public void ResetCurrentLockIndex()
    {
        _currentLockIndex = 0;
    }

    public void ActivateLockUI()
    {
        _lockUI.SetActive(true);
    }
    public void DesactivateLockUI()
    {
        _lockUI.SetActive(false);
    }

    public void SetLockUI()
    {
        for(int i = 0; i < _locks.Count - 1; i++)
        {
            Instantiate(_locksContainer.GetChild(0), _locksContainer);
        }
    }
    public void SetTimer()
    {
        if(_currentLockIndex != _locks.Count)
        {
            _locksContainer.transform.GetChild(_currentLockIndex).GetComponent<LockUICtrlr>().SetTimer(_timer);
        }
    }

    public void SetCurrentTimerValue(float timer)
    {
        _timer = timer;
    }

    public void SetLocksInformation(List<LockStruct> locks)
    {
        int index = 1;
        foreach(Transform c in _locksContainer)
        {
            c.GetComponent<LockUICtrlr>().SetLockInformation(locks[index-1].timer, index-1);
            index++;
        }
    }

    public int GetCurrentLockIndex()
    {
        return _currentLockIndex;
    }

    public LockStruct GetCurrentLock()
    {
        return _locks[_currentLockIndex];
    }

    public void IncreaseLockIndex()
    {
        _currentLockIndex++;
    }

    public void DesactivateCurrentLock()
    {
        if(_currentLockIndex != _locks.Count)
        {
            _locksContainer.GetChild(_currentLockIndex).GetComponent<LockUICtrlr>().DesactivateCurrentLock();
        }
    }

}


[System.Serializable]
public class LockStruct
{
    public int numberOfWords;
    public float  timer;
}

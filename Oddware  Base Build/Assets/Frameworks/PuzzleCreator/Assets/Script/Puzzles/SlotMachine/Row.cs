using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    private int _randomValue;
    private float _timeInterval;
    public bool _rowStopped;
    public string _stoppedSlot;
    public GameObject _rowCollider;
    public GameObject _slotCollider;
    // Start is called before the first frame update
    void Start()
    {
        _rowStopped = true;
        SlotMachineCtrlr.HandlePulled += StartRotating;
    }

    private void StartRotating()
    {
        _stoppedSlot = "";
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        _rowStopped = false;
        _timeInterval = 0.005f;//0.025f;

        for(int i = 0; i < 10; i++)
        {
            if(transform.localPosition.y <= -0.11)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 1.08f, transform.localPosition.z);
                _slotCollider.transform.localPosition = new Vector3(_slotCollider.transform.localPosition.x, 1.08f, _slotCollider.transform.localPosition.z);
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.17f, transform.localPosition.z);
            _slotCollider.transform.localPosition = new Vector3(_slotCollider.transform.localPosition.x, _slotCollider.transform.localPosition.y - 0.17f, _slotCollider.transform.localPosition.z);
            yield return new WaitForSeconds(_timeInterval);
        }

        _randomValue = Random.Range(10, 20);
        switch(_randomValue % 3)
        {
            case 1:
                _randomValue += 2;
                break;
            case 2:
                _randomValue += 1;
                break;
        }


        for (int i = 0; i < _randomValue; i++)
        {
            if (transform.localPosition.y <= -0.11)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 1.08f, transform.localPosition.z);
                _slotCollider.transform.localPosition = new Vector3(_slotCollider.transform.localPosition.x, 1.08f, _slotCollider.transform.localPosition.z);
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.17f, transform.localPosition.z);
            _slotCollider.transform.localPosition = new Vector3(_slotCollider.transform.localPosition.x, _slotCollider.transform.localPosition.y - 0.17f, _slotCollider.transform.localPosition.z);
            if(i > Mathf.RoundToInt(_randomValue * 0.173f))
            {
                _timeInterval = 0.05f;
            }
            if(i > Mathf.RoundToInt(_randomValue * 0.346f))
            {
                _timeInterval = 0.1f;
            }
            if(i > Mathf.RoundToInt(_randomValue * 0.519f))
            {
                _timeInterval = 0.15f;
            }
            if(i > Mathf.RoundToInt(_randomValue * 0.692f))
            {
                _timeInterval = 0.2f;
            }
            yield return new WaitForSeconds(_timeInterval);
        }

        _stoppedSlot = _rowCollider.GetComponent<RowCollider>()._stoppedSlot;
        _rowStopped = true;
    }


    private void OnDestroy()
    {
        SlotMachineCtrlr.HandlePulled -= StartRotating;
    }
}

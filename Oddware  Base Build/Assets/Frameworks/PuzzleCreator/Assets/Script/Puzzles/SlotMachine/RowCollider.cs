using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowCollider : MonoBehaviour
{
    public string _stoppedSlot;

    private void OnTriggerEnter(Collider other)
    {
        _stoppedSlot = other.gameObject.name;
    }
}

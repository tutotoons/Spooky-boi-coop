using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseTriggerArea : MonoBehaviour
{
    [SerializeField] private LayerMask triggerMask;
    [SerializeField] private Collider trigger;
    [SerializeField] private UnityEvent OnTriggerEntered, OnTriggerExited;

    private bool isActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == triggerMask && !isActive)
        {
            isActive = true;
            OnTriggerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == triggerMask && isActive)
        {
            isActive = false;
            OnTriggerExited?.Invoke();
        }
    }
}

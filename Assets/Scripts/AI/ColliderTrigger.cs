using System;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public event Action TriggerEnterEvent;
    public event Action TriggerExitEvent;
    public event Action TriggerStayEvent;

    [SerializeField] private string compareTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            TriggerEnterEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            TriggerExitEvent?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            TriggerStayEvent?.Invoke();
        }
    }

    public void Trigger()
    {
        TriggerEnterEvent?.Invoke();
    }
}

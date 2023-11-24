using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseInteractable : NetworkBehaviour
{
    [SerializeField] protected float interactionCooldown;
    [SerializeField] private Outline outline;
    protected float timer;
    protected bool CanInteract() => timer <= 0f;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public virtual void Interact()
    {

    }

    public void StopHighlight()
    {
        outline.enabled = false;
    }

    public void StartHighlight()
    {
        outline.enabled = true;
    }
}

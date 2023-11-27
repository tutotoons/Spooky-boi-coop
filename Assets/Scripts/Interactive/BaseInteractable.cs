using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class BaseInteractable : NetworkBehaviour
{
    public event Action<BaseInteractable> InteractEvent;

    [SerializeField] protected Animator animator;
    [SerializeField] protected float interactionCooldown;
    [SerializeField] private Outline outline;
    [SerializeField] protected UnityEvent InteractUnityEvent;

    protected float timer;

    protected bool CanInteract() => timer <= 0f;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public virtual void AnimateInteraction()
    {
        animator.SetTrigger("Interact");
        timer = interactionCooldown;
    }

    public virtual void Interact()
    {
        if (CanInteract())
        {
            InteractUnityEvent?.Invoke();
            InvokeBaseInteractEventServerRpc();
            AnimateInteractionServerRpc();
            return;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    protected void InvokeBaseInteractEventServerRpc()
    {
        InvokeBaseInteractEventClientRpc();
    }

    [ClientRpc]
    protected void InvokeBaseInteractEventClientRpc()
    {
        Debug.Log($"interact called for {transform.name}");
        InteractEvent?.Invoke(this);
    }


    [ServerRpc(RequireOwnership = false)]
    protected void AnimateInteractionServerRpc()
    {
        AnimateInteractionClientRpc();
    }

    [ClientRpc]
    private void AnimateInteractionClientRpc()
    {
        AnimateInteraction();
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

using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseInteractable : NetworkBehaviour
{
    public event Action<BaseInteractable> InteractEvent;

    [SerializeField] private Animator animator;
    [SerializeField] protected float interactionCooldown;
    [SerializeField] private Outline outline;
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
        InteractEvent?.Invoke(this);
        AnimateInteractionServerRpc();
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

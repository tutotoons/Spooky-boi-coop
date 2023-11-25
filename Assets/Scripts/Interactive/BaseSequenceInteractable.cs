using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSequenceInteractable : BaseInteractable
{
    [SerializeField] int sequenceNumber;
    [SerializeField] private Animator animator;
    [SerializeField] private BaseSequenceInteractionListener[] interactives;

    public override void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        base.Interact();

        timer = interactionCooldown;
        AnimateInteractionServerRpc();

        foreach (BaseSequenceInteractionListener _interactive in interactives)
        {
            _interactive.Activate(sequenceNumber);
        }
    }
}

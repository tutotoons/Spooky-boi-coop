using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTextInteractable : BaseInteractable
{
    [SerializeField] private string message;
    [SerializeField] private float duration;

    public override void Interact()
    {
        if (timer > 0f)
        {
            return;
        }
        timer = interactionCooldown;
        AnimateInteraction();

        Phone.Instance?.DisplayTextServerRpc(message, duration);
    }
}

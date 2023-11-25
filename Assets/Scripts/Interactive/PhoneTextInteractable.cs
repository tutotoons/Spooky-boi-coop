using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTextInteractable : BaseToggleInteractable
{
    [SerializeField] private string message;
    [SerializeField] private float duration;
    [SerializeField] private float heat;

    public override void Interact()
    {
        if (timer > 0f)
        {
            return;
        }

        base.Interact();

        timer = interactionCooldown;
        AnimateInteractionServerRpc();

        Phone.Instance?.DisplayTextServerRpc(message, duration);
        MonsterManager.Instance?.monster.AddHeat(heat);
    }
}

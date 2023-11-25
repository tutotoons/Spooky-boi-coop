using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneColorInteractable : BaseToggleInteractable
{
    [SerializeField] private Color color;
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

        Phone.Instance?.DisplayColorServerRpc(color, duration);
        MonsterManager.Instance?.monster.AddHeat(heat);
    }
}

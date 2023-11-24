using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneSoundInteractable : BaseInteractable
{
    [SerializeField] private SoundType sound;

    public override void Interact()
    {
        if (timer > 0f)
        {
            return;
        }
        timer = interactionCooldown;
        AnimateInteraction();

        Phone.Instance?.PlaySoundServerRpc(sound);
    }
}
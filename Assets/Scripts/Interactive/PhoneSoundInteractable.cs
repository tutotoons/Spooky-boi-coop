using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneSoundInteractable : BaseToggleInteractable
{
    [SerializeField] private SoundType sound;
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

        Phone.Instance?.PlaySoundServerRpc(sound);
        MonsterManager.Instance?.monster.AddHeat(heat);
    }
}

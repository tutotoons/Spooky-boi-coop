using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoorInteractionListener : DoorInteractionListener
{
    public override void Activate(bool _state)
    {
        base.Activate(!currentState);
    }
}

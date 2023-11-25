using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private Light lightObj;

    public override void Animate(bool _state)
    {
        lightObj.enabled = _state;
    }
}

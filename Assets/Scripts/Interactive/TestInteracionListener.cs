using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteracionListener : BaseInteractionListener
{
    [SerializeField] private MeshRenderer rend;
    public override void Activate(bool _state)
    {
        rend.enabled = !rend.enabled;
    }
}

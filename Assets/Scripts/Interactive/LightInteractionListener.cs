using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private Light lightObj;
    [SerializeField] private Transform lightParent;

    public override void Animate(bool _state)
    {
        lightObj.enabled = _state;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        LightManager.Instance?.AddLight(lightParent);
    }

    public float GetDistance(Vector3 pos) => (transform.position - pos).sqrMagnitude;
}

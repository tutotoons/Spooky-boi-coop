using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractionListener : BaseToggleInteractionListener, IDynamicLight
{
    [SerializeField] private Light lightObj;
    [SerializeField] private GameObject lightParent;

    public override void Animate(bool _state)
    {
        lightObj.enabled = _state;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();
        Initialize();
    }

    public void Initialize()
    {
        LightManager.Instance.AddLight(this);
    }

    public void Disable()
    {
        lightParent.SetActive(false);
    }

    public void Enable()
    {
        lightParent.SetActive(true);
    }

    public float GetDistance(Vector3 pos) => (transform.position - pos).sqrMagnitude;
}

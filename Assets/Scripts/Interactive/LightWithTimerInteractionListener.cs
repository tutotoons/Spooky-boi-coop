using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWithTimerInteractionListener : BaseToggleInteractionListener, IDynamicLight
{
    [SerializeField] private Light lightObj;
    [SerializeField] private float timeToTurnOff;
    [SerializeField] private GameObject lightParent;

    private float timer;

    public override void Animate(bool _state)
    {
        currentState = _state;
        lightObj.gameObject.SetActive(_state);
        timer = timeToTurnOff;
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

    private void Update()
    {
        if (currentState == startingState)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Activate(!currentState);
        }
    }
}

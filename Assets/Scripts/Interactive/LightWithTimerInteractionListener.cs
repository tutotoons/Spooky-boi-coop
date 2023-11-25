using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWithTimerInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private Light lightObj;
    [SerializeField] private float timeToTurnOff;
    [SerializeField] private Transform lightParent;

    private float timer;

    public override void Animate(bool _state)
    {
        currentState = _state;
        lightObj.gameObject.SetActive(_state);
        timer = timeToTurnOff;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        LightManager.Instance?.AddLight(lightParent);
    }

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

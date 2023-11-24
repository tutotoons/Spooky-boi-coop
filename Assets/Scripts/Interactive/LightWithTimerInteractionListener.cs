using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWithTimerInteractionListener : BaseInteractionListener
{
    [SerializeField] private Light lightObj;
    [SerializeField] private float timeToTurnOff;

    private bool isActive;
    private float timer;

    public override void Animate(bool _state)
    {
        isActive = _state;
        lightObj.gameObject.SetActive(_state);
        timer = timeToTurnOff;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Activate(false);
        }
    }
}

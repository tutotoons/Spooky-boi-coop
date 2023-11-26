using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flicker : MonoBehaviour
{
    [SerializeField] private float minOnTime, maxOnTime;
    [SerializeField] private float minOffTime, maxOffTime;
    [SerializeField] private GameObject objToFlicker;

    private bool state;
    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        SwitchState();
    }

    private void SwitchState()
    {
        if (state)
        {
            timer = Random.Range(minOffTime, maxOffTime);
            objToFlicker.SetActive(false);
            state = false;
        }
        else
        {
            timer = Random.Range(minOnTime, maxOnTime);
            objToFlicker.SetActive(true);
            state = true;
        }
    }
}

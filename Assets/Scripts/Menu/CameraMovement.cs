using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float yRange, zRange;
    [SerializeField] float minChangeTime, maxChangeTime;
    private Vector3 initialPos;
    private Vector3 startPos, endPos;

    private float timer;
    private float duration;

    void Start()
    {
        initialPos = transform.position;
        SetNewTargetPos();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, endPos, 1f - (timer / duration));

        if (timer > 0f)
        {
            return;
        }
        SetNewTargetPos();
    }

    private void SetNewTargetPos()
    {
        duration = Random.Range(minChangeTime, maxChangeTime);
        startPos = transform.position;
        endPos = GetTargetPos();
        timer = duration;
    }

    private Vector3 GetTargetPos()
    {
        return initialPos + transform.right * Random.Range(-zRange, zRange) + transform.up * Random.Range(-yRange, yRange);
    }
}

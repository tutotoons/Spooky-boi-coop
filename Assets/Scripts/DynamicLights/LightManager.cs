using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    [SerializeField] private int allowedAmount = 5;
    [SerializeField] private float refreshRatePerSecond;

    private float timer;
    private Transform playerTransform;
    private List<IDynamicLight> lights;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            lights = new List<IDynamicLight>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActivePlayer(Transform _playerTransform)
    {
        playerTransform = _playerTransform;
    }

    public void AddLight(IDynamicLight _light)
    {
        lights.Add(_light);
        _light.Disable();
    }

    private void LateUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }

        UpdateLights();
    }

    private void UpdateLights()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            return;
        }
        timer = 1f / refreshRatePerSecond;
        Vector3 _playerPos = playerTransform.position;
        lights.OrderBy(v => v.GetDistance(_playerPos));
        int addedCount = 0;
        for (int i = lights.Count - 1; i >= 0; i--)
        {
            if (addedCount < allowedAmount)
            {
                lights[i].Enable();
                addedCount++;
            }
            else
            {
                lights[i].Disable();
            }
        }

        Debug.Log($"light update");

    }
}

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
    private List<Transform> lights;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            lights = new List<Transform>();
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

    public void AddLight(Transform _light)
    {
        lights.Add(_light);
        _light.gameObject.SetActive(false);
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
        lights = lights.OrderBy(v => (v.transform.position - _playerPos).sqrMagnitude).ToList();
        for (int i = 0; i < lights.Count; i++)
        {
            if (i < allowedAmount)
            {
                lights[i].gameObject.SetActive(true);
            }
            else
            {
                lights[i].gameObject.SetActive(false);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolRoute
{
    Vector3[] points;
}

public class PatrolRouteManager : MonoBehaviour
{
    public static PatrolRouteManager Instance 
    {
        get;
        private set;
    }

    [SerializeField] private PatrolRoute[] routes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Init();
        }
        
    }

    private void Init()
    {

    }
}

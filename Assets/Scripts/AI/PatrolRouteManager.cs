using System;
using Unity.Netcode;
using UnityEngine;

public class PatrolRouteManager : NetworkBehaviour
{
    public event Action<PatrolRoute> RouteChangedEvent;

    public PatrolRoute ActiveRoute
    {
        get => _activeRoute;
        private set
        {
            if(_activeRoute != value)
            {
                _activeRoute = value;
                RouteChangedEvent?.Invoke(value);
            }
        }
    }

    [SerializeField] private PatrolRoute[] routes;

    private PatrolRoute _activeRoute;

    public override void OnNetworkSpawn()
    {
        Init();
    }

    private void Init()
    {
        foreach(PatrolRoute route in routes)
        {
            route.trigger.TriggerEnterEvent += () => OnTriggeredRoute(route);
        }

        ActiveRoute = routes[0];
    }


    public void SetClosestRouteActive(Transform t)
    {
        PatrolRoute closestRoute = null;
        float closestRouteDistance = 999999;

        foreach(PatrolRoute route in routes)
        {
            float minDistance = 999999;

            foreach(Transform point in route.points)
            {
                float distance = Vector3.Distance(t.position, point.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            if(minDistance < closestRouteDistance)
            {
                closestRouteDistance = minDistance;
                closestRoute = route;
            }
        }

        ActiveRoute = closestRoute;
    }

    private void OnTriggeredRoute(PatrolRoute route)
    {
        ActiveRoute = route;
    }
}

[Serializable]
public class PatrolRoute
{
    public Transform[] points;
    public ColliderTrigger trigger;
}

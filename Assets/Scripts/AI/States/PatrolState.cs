using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private readonly NavMeshAgent agent;
    private readonly PatrolRouteManager routeManager;

    private int index;
    private float waitAfterTimer;

    public PatrolState(NavMeshAgent _agent, PatrolRouteManager _routeManager)
    {
        agent = _agent;
        routeManager = _routeManager;
    }

    public void OnEnter()
    {
        waitAfterTimer = Random.Range(10f, 20f);

        routeManager.RouteChangedEvent += OnRouteChangedCallback;

        routeManager.SetClosestRouteActive(agent.transform);

        agent.SetDestination(routeManager.ActiveRoute.points[index].position);
    }

    public void OnExit()
    {
        routeManager.RouteChangedEvent -= OnRouteChangedCallback;
    }

    public void Tick(float _delta)
    {
        waitAfterTimer -= _delta;

        if (agent.remainingDistance < AIUtils.DISTANCE_THRESHOLD)
        {
            ChangeDestination();
        }

        if(agent.destination != routeManager.ActiveRoute.points[index].position)
        {
            agent.SetDestination(routeManager.ActiveRoute.points[index].position);
        }
    }

    private int GetClosestPosition(PatrolRoute route)
    {
        float _closest = float.MaxValue;
        Transform[] patrolPoints = route.points;

        int _index = 0;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float _dist = (agent.transform.position - patrolPoints[i].position).sqrMagnitude;
            if (_dist < _closest)
            {
                _closest = _dist;
                _index = i;
            }
        }

        return _index;
    }

    public bool PatrolTimerRunOut()
    {
        return waitAfterTimer < 0;
    }

    private void ChangeDestination()
    {
        Transform[] patrolPoints = routeManager.ActiveRoute.points;

        index = (index + 1) % patrolPoints.Length;
    }

    private void OnRouteChangedCallback(PatrolRoute route)
    {
        index = GetClosestPosition(route);
    }
}

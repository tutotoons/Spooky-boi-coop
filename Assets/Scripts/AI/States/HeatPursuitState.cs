using UnityEngine;
using UnityEngine.AI;

public class HeatPursuitState : IState
{
    private readonly NavMeshAgent agent;
    private readonly Transform target;

    public HeatPursuitState(NavMeshAgent _agent, Transform _target)
    {
        agent = _agent;
        target = _target;
    }

    public void OnEnter()
    {
        agent.isStopped = false;

        agent.SetDestination(target.position);
    }

    public void OnExit()
    {
        
    }

    public void Tick(float _delta)
    {

    }

    public bool ReachedTarget()
    {
        return agent.remainingDistance < AIUtils.DISTANCE_THRESHOLD;
    }
}

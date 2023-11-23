using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private NavMeshAgent agent;
    private float timer;

    public IdleState(NavMeshAgent _agent)
    {
        timer = 3f;
        agent = _agent;
    }

    public void OnEnter()
    {
        agent.isStopped = true;
    }

    public void OnExit()
    {
    }

    public void SafeDestroy()
    {
    }

    public void Tick(float _delta)
    {
        timer -= _delta;
    }

    public bool IdleTimerOver() => timer <= 0;
}

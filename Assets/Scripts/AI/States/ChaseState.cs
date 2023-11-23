using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private NavMeshAgent agent;
    private Transform target;

    public ChaseState(NavMeshAgent _navMeshAgent, Transform _target)
    {
        agent = _navMeshAgent;
        target = _target;
    }

    public void OnEnter()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void OnExit()
    {
        agent.isStopped = true;
    }

    public void Tick(float _delta)
    {
        agent.SetDestination(target.position);
    }

    internal bool CloseToPlayer() => (agent.transform.position - target.position).magnitude < 5f;
}

using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private readonly NavMeshAgent agent;
    private readonly Transform target;
    private readonly ColliderTrigger detectionCone;

    private SearchState _searchState;

    public ChaseState(NavMeshAgent _navMeshAgent, Transform _target, ColliderTrigger _detectionCone)
    {
        agent = _navMeshAgent;
        target = _target;
        detectionCone = _detectionCone;

        detectionCone.TriggerEnterEvent += OnDetectionEnter;
        detectionCone.TriggerExitEvent += OnDetectionExit;
        detectionCone.TriggerStayEvent += OnDetectionStay;
    }

    public void OnEnter()
    {
        Debug.Log("Start Chase");

        _searchState = SearchState.Chasing;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void OnExit()
    {
        Debug.Log("End Chase");

        _searchState = SearchState.None;

        agent.isStopped = true;
    }

    public void Tick(float _delta)
    {
        agent.SetDestination(target.position);
    }

    private void DetectPlayer()
    {
        Vector3 direction = (target.position - agent.transform.position).normalized;

        if (Physics.Raycast(agent.transform.position, direction, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<NetworkPlayer>(out var player))
            {
                _searchState = SearchState.FoundPlayer;
            }
        }
    }

    private void LoosePlayer()
    {
        _searchState = SearchState.LostPlayer;
    }
    
    private void OnDetectionEnter()
    {
        if(_searchState == SearchState.None)
        {
            DetectPlayer();
        }
    }

    private void OnDetectionStay()
    {
        if(_searchState == SearchState.None)
        {
            DetectPlayer();
        }
    }

    private void OnDetectionExit()
    {
        if(_searchState == SearchState.FoundPlayer || _searchState == SearchState.Chasing)
        {
            LoosePlayer();
        }
    }

    internal bool LostPlayer()
    {
        return _searchState == SearchState.LostPlayer;
    }

    internal bool TriggeredPlayerDetection()
    {
        return _searchState == SearchState.FoundPlayer;
    }

    enum SearchState
    {
        None,
        FoundPlayer,
        Chasing,
        LostPlayer,
    }
}

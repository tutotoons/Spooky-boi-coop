using UnityEngine;
using UnityEngine.AI;

public class SearchState : IState
{
    private readonly NavMeshAgent agent;
    private readonly Transform target;
    private readonly ColliderTrigger detectionCone;
    private readonly LayerMask ignoreRaycastLayerMask;

    private State _state;
    private float _lostTimer;

    public SearchState(NavMeshAgent _navMeshAgent, Transform _target, ColliderTrigger _detectionCone, LayerMask _ignoreRaycastLayerMask)
    {
        agent = _navMeshAgent;
        target = _target;
        detectionCone = _detectionCone;
        ignoreRaycastLayerMask = _ignoreRaycastLayerMask;

        detectionCone.TriggerEnterEvent += OnDetectionEnter;
        detectionCone.TriggerStayEvent += OnDetectionStay;
    }

    public void OnEnter()
    {
        Debug.Log("Chase Lost Start");

        agent.isStopped = false;
        _lostTimer = 2f;
        _state = State.Searching;
    }

    public void OnExit()
    {
        Debug.Log("Chase Lost End");

        _state = State.None;
    }

    public void Tick(float _delta)
    {
        if(_state != State.Searching)
        {
            return;
        }

        if(_lostTimer > 0)
        {
            _lostTimer -= _delta;
            agent.SetDestination(target.position);
            DetectPlayer();
            return;
        }

        _state = State.Lost;
    }

    public bool FoundPlayer()
    {
        return _state == State.Found;
    }

    public bool LostPlayer()
    {
        return _state == State.Lost;
    }

    private void DetectPlayer()
    {
        Vector3 direction = (target.position - agent.transform.position).normalized;

        if (Physics.Raycast(agent.transform.position, direction, out RaycastHit hit, 20, ~ignoreRaycastLayerMask))
        {
            if (hit.collider.TryGetComponent<NetworkPlayer>(out var player))
            {
                _state = State.Found;
            }
        }
    }

    private void OnDetectionStay()
    {
        if(_state == State.Searching)
        {
            DetectPlayer();
        }
    }

    private void OnDetectionEnter()
    {
        if (_state == State.Searching)
        {
            DetectPlayer();
        }
    }

    enum State
    {
        None,
        Searching,
        Found,
        Lost,
    }
}

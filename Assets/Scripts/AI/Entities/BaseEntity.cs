using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEntity : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float stopDist;

    protected StateMachine stateMachine;

    protected ChaseState chase;
    protected IdleState idle;

    private Transform target;

    private float DistanceToPlayer => (transform.position - target.position).magnitude;

    private void Awake()
    {
        Initialize(FindObjectOfType<ExplorerBehaviour>().transform);
    }

    public virtual void Initialize(Transform _target)
    {
        target = _target;
        stateMachine = new StateMachine();

        chase = new ChaseState(navMeshAgent, target);
        idle = new IdleState(navMeshAgent);

        stateMachine.AddTransition(idle, chase, () => { return idle.IdleTimerOver(); });
        stateMachine.AddTransition(chase, idle, () => { return chase.CloseToPlayer(); });

        stateMachine.SetState(idle);
    }


    private void OnDestroy()
    {
    }

    private void Update()
    {
        Debug.Log($"{DistanceToPlayer}");
        stateMachine.Tick(Time.deltaTime);
    }
}

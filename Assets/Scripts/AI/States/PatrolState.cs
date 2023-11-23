using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    int index;
    NavMeshAgent agent;
    Vector3[] patrolPoints;

    public PatrolState(NavMeshAgent _agent, Vector3[] _patrolPoints)
    {
        agent = _agent;
        patrolPoints = _patrolPoints;
    }

    private int GetClosestPosition()
    {
        float _closest = float.MaxValue;
        int _index = 0;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float _dist = (agent.transform.position - patrolPoints[i]).sqrMagnitude;
            if(_dist < _closest)
            {
                _closest = _dist;
                _index = i;
            }
        }

        return _index;
    }

    public void OnEnter()
    {
        index = GetClosestPosition();
    }

    public void OnExit()
    {
    }

    public void Tick(float _delta)
    {
        if (agent.remainingDistance < AIUtils.DISTANCE_THRESHOLD)
        {
            ChangeDestination();
        }
    }

    private void ChangeDestination()
    {
        index = index == patrolPoints.Length-1 ? 0 : index++;
    }
}

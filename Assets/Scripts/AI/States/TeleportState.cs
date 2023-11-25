using UnityEngine;
using UnityEngine.AI;

public class TeleportState : IState
{
    private readonly NavMeshAgent agent;
    private readonly Transform teleportTarget;

    private bool teleportedAway;

    public TeleportState(NavMeshAgent _agent, Transform _teleportTarget)
    {
        agent = _agent;
        teleportTarget = _teleportTarget;
    }

    public void OnEnter()
    {
        if(teleportTarget != null)
        {
            agent.Move(agent.transform.position - teleportTarget.position);
        }

        teleportedAway = true;
    }

    public void OnExit()
    {
        teleportedAway = false;
    }

    public void Tick(float _delta)
    {
        
    }

    public bool TeleportedAway() => teleportedAway;
}

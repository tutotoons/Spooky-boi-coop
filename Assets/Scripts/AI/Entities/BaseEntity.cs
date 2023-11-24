using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEntity : NetworkBehaviour
{
    private static WaitForSeconds waitForSecondsToInitialize = new WaitForSeconds(0.1f);

    [SerializeField] protected PatrolRouteManager patrolRouteManager;
    [SerializeField] protected ColliderTrigger detectionConeTrigger;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float stopDist;

    [Header("Tweaking")]
    [SerializeField] private float heatDecaySpeed;
    [SerializeField] private float maxHeat = 1;

    protected HeatPursuitState heatPursuitState;
    protected StateMachine stateMachine;
    protected SearchState search;
    protected PatrolState patrol;
    protected ChaseState chase;
    protected IdleState idle;
    protected float totalHeat;

    private Transform target;
    private bool initialized;

    private float DistanceToPlayer => (transform.position - target.position).magnitude;

    public override void OnNetworkSpawn()
    {
        StartCoroutine(FindPlayerAndInitializeCoroutine());
    }

    public virtual void Initialize(Transform _target)
    {
        target = _target;
        stateMachine = new StateMachine();

        heatPursuitState = new HeatPursuitState(navMeshAgent, target);
        search = new SearchState(navMeshAgent, target, detectionConeTrigger);
        chase = new ChaseState(navMeshAgent, target, detectionConeTrigger);
        patrol = new PatrolState(navMeshAgent, patrolRouteManager);
        idle = new IdleState(navMeshAgent);


        stateMachine.AddTransition(idle, patrol, idle.IdleTimerOver);
        stateMachine.AddTransition(patrol, idle, patrol.PatrolTimerRunOut);
        stateMachine.AddTransition(chase, search, chase.LostPlayer);
        stateMachine.AddTransition(search, chase, search.FoundPlayer);
        stateMachine.AddTransition(search, idle, search.LostPlayer);
        stateMachine.AddTransition(heatPursuitState, idle, heatPursuitState.ReachedTarget);
        stateMachine.AddAnyTransition(chase, chase.TriggeredPlayerDetection);
        stateMachine.AddAnyTransition(heatPursuitState, HeatThresholdReached);

        stateMachine.SetState(patrol);

        initialized = true;
    }

    private IEnumerator FindPlayerAndInitializeCoroutine()
    {
        NetworkPlayer player = null;

        while(player == null)
        {
            player = FindObjectOfType<NetworkPlayer>();
            yield return waitForSecondsToInitialize;
        }

        Initialize(player.transform);
    }

    public void AddHeat(float value)
    {
        totalHeat = Mathf.Clamp(totalHeat + value, 0, float.MaxValue);
    }

    private bool HeatThresholdReached()
    {
        return totalHeat > maxHeat;
    }

    private void Update()
    {
        if(!initialized)
        {
            return;
        }

        stateMachine.Tick(Time.deltaTime);
        AddHeat(-Time.deltaTime * heatDecaySpeed);
    }
}

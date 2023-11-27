using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEntity : NetworkBehaviour
{
    [SerializeField] protected PatrolRouteManager patrolRouteManager;
    [SerializeField] protected ColliderTrigger detectionConeTrigger;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float stopDist;
    [SerializeField] protected Transform teleportAwayTarget;
    [SerializeField] protected LayerMask ignoreRaycastMask;
    [SerializeField] protected Animator animator;

    [Header("Tweaking")]
    [SerializeField] private float heatDecaySpeed;
    [SerializeField] private float maxHeat = 1;

    protected HeatPursuitState heatPursuitState;
    protected StateMachine stateMachine;
    protected TeleportState teleport;
    protected SearchState search;
    protected PatrolState patrol;
    protected ChaseState chase;
    protected IdleState idle;

    protected NetworkVariable<float> totalHeat = new NetworkVariable<float>();

    private Transform target;
    private bool initialized;

    private float DistanceToPlayer => (transform.position - target.position).magnitude;

    public override void OnNetworkSpawn()
    {
        if (!IsServer && !IsHost)
        {
            return;
        }

        StartCoroutine(PlayerUtils.FindPlayerDoAction(PlayerType.Explorer, OnFoundPlayerExplorer));
    }

    public virtual void Initialize(Transform _target)
    {
        target = _target;
        stateMachine = new StateMachine();

        heatPursuitState = new HeatPursuitState(navMeshAgent, target);
        search = new SearchState(navMeshAgent, target, detectionConeTrigger, ignoreRaycastMask);
        teleport = new TeleportState(navMeshAgent, teleportAwayTarget);
        chase = new ChaseState(navMeshAgent, target, detectionConeTrigger, ignoreRaycastMask);
        patrol = new PatrolState(navMeshAgent, patrolRouteManager);
        idle = new IdleState(navMeshAgent);

        stateMachine.AddTransition(idle, patrol, idle.IdleTimerOver);
        stateMachine.AddTransition(patrol, idle, patrol.NoRoute);
        stateMachine.AddTransition(patrol, idle, patrol.PatrolTimerRunOut);
        stateMachine.AddTransition(chase, search, chase.LostPlayer);
        stateMachine.AddTransition(chase, teleport, chase.CaughtPlayer);
        stateMachine.AddTransition(teleport, idle, teleport.TeleportedAway);
        stateMachine.AddTransition(search, chase, search.FoundPlayer);
        stateMachine.AddTransition(search, idle, search.LostPlayer);
        stateMachine.AddTransition(heatPursuitState, idle, heatPursuitState.ReachedTarget);
        stateMachine.AddAnyTransition(chase, chase.TriggeredPlayerDetection);
        stateMachine.AddAnyTransition(heatPursuitState, HeatThresholdReached);

        stateMachine.SetState(patrol);

        initialized = true;
    }

    private void OnFoundPlayerExplorer(NetworkPlayer player)
    {
        Initialize(player.transform);
    }

    public void AddHeat(float value)
    {
        AddHeatServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddHeatServerRpc(float value)
    {
        totalHeat.Value = Mathf.Clamp(totalHeat.Value + value, 0, float.MaxValue);
    }

    private bool HeatThresholdReached()
    {
        return totalHeat.Value > maxHeat;
    }

    private Vector3 previousPos;

    private void Update()
    {
        if (!initialized)
        {
            return;
        }
        ANimate();
        stateMachine.Tick(Time.deltaTime);
        AddHeat(-Time.deltaTime * heatDecaySpeed);
    }

    private void ANimate()
    {
        float deltaPos = (transform.position - previousPos).magnitude;
        animator.SetFloat("Speed", Mathf.Clamp01((deltaPos / Time.deltaTime) / 4f));
        previousPos = transform.position;
    }
}

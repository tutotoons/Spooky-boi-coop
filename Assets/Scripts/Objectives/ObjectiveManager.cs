using System;
using Unity.Netcode;
using UnityEngine;

public class ObjectiveManager : NetworkBehaviour
{
    public event Action CompletedAllObjectives;

    public static ObjectiveManager Instance
    {
        get;
        private set;
    }

    [SerializeField] private Objective[] objectives;

    private NetworkVariable<int> _index = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            _index.Value = 0;
        }

        _index.OnValueChanged += OnIndexValueChanged;
        OnIndexValueChanged(0, 0);
    }

    public override void OnNetworkDespawn()
    {
        _index.OnValueChanged -= OnIndexValueChanged;
    }

    private void OnIndexValueChanged(int previousValue, int newValue)
    {
        objectives[previousValue].CompletedEvent -= OnCompletedObjective;
        objectives[previousValue].UpdatedGoalEvent -= OnUpdatedObjectiveGoal;
        objectives[previousValue].ResetEvent -= OnUpdatedObjectiveGoal;
        objectives[previousValue].IsActive = false;

        objectives[newValue].CompletedEvent += OnCompletedObjective;
        objectives[newValue].UpdatedGoalEvent += OnUpdatedObjectiveGoal;
        objectives[newValue].ResetEvent += OnUpdatedObjectiveGoal;
        objectives[newValue].IsActive = true;

        SetRadarToCurrentObjective();
    }

    private void OnUpdatedObjectiveGoal(Objective objective)
    {
        SetRadarToCurrentObjective();
    }

    private void OnCompletedObjective(Objective objective)
    {
        if(_index.Value < objectives.Length - 1)
        {
            UpdateObjectiveServerRpc(_index.Value + 1);
        }
        else
        {
            RadarManager.Instance.SetObjective(null);
            CompletedAllObjectives?.Invoke();
        }
    }

    private void SetRadarToCurrentObjective()
    {
        SetRadarToCurrentObjectiveServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetRadarToCurrentObjectiveServerRpc()
    {
        if (objectives.Length > 0)
        {
            RadarManager.Instance.SetObjective(objectives[_index.Value].GetTarget());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateObjectiveServerRpc(int index)
    {
        _index.Value = index;
    }
}

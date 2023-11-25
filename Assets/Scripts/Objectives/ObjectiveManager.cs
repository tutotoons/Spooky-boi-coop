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

    private NetworkVariable<int> _index = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if(IsServer || IsHost)
        {
            _index.Value = 0;
        }
        else
        {
            _index.OnValueChanged += OnIndexValueChanged;
        }
    }

    private void OnIndexValueChanged(int previousValue, int newValue)
    {
        objectives[previousValue].CompletedEvent -= OnCompletedObjective;
        objectives[newValue].CompletedEvent += OnCompletedObjective;
    }

    private void OnCompletedObjective(Objective objective)
    {
        if(_index.Value < objectives.Length)
        {
            UpdateObjectiveServerRpc(_index.Value + 1);
        }
        else
        {
            CompletedAllObjectives?.Invoke();
        }
    }

    [ServerRpc]
    private void UpdateObjectiveServerRpc(int index)
    {
        _index.Value = index;
    }
}

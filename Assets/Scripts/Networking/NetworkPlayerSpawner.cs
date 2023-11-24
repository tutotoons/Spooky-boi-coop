using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject explorerPrefab;
    [SerializeField] private GameObject navigatorPrefab;

    [SerializeField] private Transform explorerSpawnPoint;
    [SerializeField] private Transform navigatorSpawnPoint;

    public override void OnNetworkSpawn()
    {
        Debug.Log($"player connected trying to spawn isServer{IsServer}");
        if (IsServer)
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        }
        else
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong _clientId, int _prefabId)
    {
        Vector3 _spawnPos = _prefabId == 0 ? navigatorSpawnPoint.position : explorerSpawnPoint.position;
        GameObject _playerObj = _prefabId == 0 ? Instantiate(navigatorPrefab, _spawnPos, Quaternion.identity) : Instantiate(explorerPrefab, _spawnPos, Quaternion.identity);

        _playerObj.SetActive(true);
        _playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(_clientId, true);
    }
}

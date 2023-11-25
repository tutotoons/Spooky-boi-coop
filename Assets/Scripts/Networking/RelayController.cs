using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayController : MonoBehaviour
{
    public async void Initialize()
    {
        Debug.Log($"init start");
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log($"init end");

    }

    public async void CreateRelay(Action<string> onConnected)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            NetworkManager.Singleton.StartHost();
            onConnected?.Invoke(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void JoinRelay(string _joinCode, Action onConnected)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
                );
            NetworkManager.Singleton.StartClient();
            onConnected?.Invoke();
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
        }
    }
}

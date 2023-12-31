using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayController : MonoBehaviour
{
    public async void Initialize(Action onComplete)
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        onComplete?.Invoke();
    }

    public async void CreateRelay(Action<string, bool> onConnected)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData serverData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);

            NetworkManager.Singleton.StartHost();
            onConnected?.Invoke(joinCode, true);
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
            onConnected?.Invoke("", false);
        }
    }

    public async void JoinRelay(string _joinCode, Action<bool> onConnected)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);

            RelayServerData serverData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            NetworkManager.Singleton.StartClient();
            onConnected?.Invoke(true);
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
            onConnected?.Invoke(false);
        }
    }
}

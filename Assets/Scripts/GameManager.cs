using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinField;
    [SerializeField] GameObject gameStartUI;
    [SerializeField] GameObject playerUI;
    [SerializeField] RelayController relay;

    private string joinCode;

    private void Start()
    {
        relay.Initialize();
        joinField.text = joinCode;
    }


    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (IsConnected())
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    private void StatusLabels()
    {
        string _status = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label($"{joinCode} {_status}");
    }

    public void StartHost()
    {
        relay.CreateRelay((_joinCode) => 
        {
            joinCode = _joinCode;
            gameStartUI.SetActive(false);
            playerUI.SetActive(true);
        });
    }

    public void StartClient()
    {
        joinCode = joinField.text;

        relay.JoinRelay(joinCode ,() =>
        {
            gameStartUI.SetActive(false);
            playerUI.SetActive(true);
        });
    }

    private bool IsConnected()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }
}

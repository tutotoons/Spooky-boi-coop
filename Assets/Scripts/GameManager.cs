using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinField;
    [SerializeField] GameObject gameStartUI;
    [SerializeField] GameObject playerUI;
    [SerializeField] RelayController relay;
    [SerializeField] GameObject menuArea;

    private string joinCode;
    private bool initialized;
    private bool canClickButtons;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        relay.Initialize(() => 
        {
            initialized = true;
            canClickButtons = true;
        });
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
        if (!initialized || !canClickButtons)
        {
            return;
        }
        canClickButtons = false;
        relay.CreateRelay((_joinCode, _success) => 
        {
            if (_success)
            {
                joinCode = _joinCode;
                GUIUtility.systemCopyBuffer = joinCode;
                gameStartUI.SetActive(false);
                playerUI.SetActive(true);
            }
            else
            {
                canClickButtons = true;
            }
        });
    }

    public void StartClient()
    {
        if (!initialized || !canClickButtons)
        {
            return;
        }

        joinCode = joinField.text.ToUpper();
        canClickButtons = false;
        relay.JoinRelay(joinCode ,(_success) =>
        {
            if (_success)
            {
                gameStartUI.SetActive(false);
                playerUI.SetActive(true);
            }
            else
            {
                canClickButtons = true;
            }
        });
    }

    private bool IsConnected()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }
}

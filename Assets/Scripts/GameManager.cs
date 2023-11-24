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
    [SerializeField] private TMP_InputField ipField;
    [SerializeField] GameObject connectScreen;
    private string ip;

    
    private void Start()
    {
        ip = "0.0.0.0";
        SetIpAddress();
    }

    private void SetIpAddress()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ip;
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
        GUILayout.Label($"{ip} {_status}");
    }

    public void StartHost()
    {
        connectScreen.SetActive(false);
        GetLocalIp();
        NetworkManager.Singleton.StartHost();
    }

    private void GetLocalIp()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var _ip in host.AddressList)
        {
            if (_ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ip = _ip.ToString();
                return;
            }
        }

        throw new Exception("network problem pls fix");
    }

    public void StartClient()
    {
        connectScreen.SetActive(false);
        ip = ipField.text;
        SetIpAddress();
        NetworkManager.Singleton.StartClient();
    }

    private bool IsConnected()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }
}

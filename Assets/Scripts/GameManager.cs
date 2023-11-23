using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        
        var networkManager = NetworkManager.Singleton;

        if (!IsConnected())
        {
            if (GUILayout.Button("Host"))
            {
                networkManager.StartHost();
            }

            if (GUILayout.Button("Client"))
            {
                networkManager.StartClient();
            }

            if (GUILayout.Button("Server"))
            {
                networkManager.StartServer();
            }
        }
        else
        {
            GUILayout.Label($"Mode: {(networkManager.IsHost ? "Host" : networkManager.IsServer ? "Server" : "Client")}");

            if (networkManager.IsClient)
            {
                GUILayout.Label("Connected as client");
            }
        }

        GUILayout.EndArea();
    }

    private bool IsConnected()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }
}

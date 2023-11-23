using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    [SerializeField] private NetworkManager netManager;
    void Start()
    {
        if (Application.isEditor)
        {
            return;
        }

        Dictionary<string, string> _args = GetCommandLineArgs();
        if (_args.TryGetValue("-mode", out string _mode))
        {
            if (_mode == "server")
            {
                netManager.StartServer();
            }
            else if (_mode == "client")
            {
                netManager.StartClient();
            }
            else if (_mode == "host")
            {
                netManager.StartHost();
            }
        }
    }

    private Dictionary<string, string> GetCommandLineArgs()
    {
        Dictionary<string, string> _argDictionary = new Dictionary<string, string>();
        string[] _args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < _args.Length; i++)
        {
            string _arg = _args[i].ToLower();
            if (_arg.StartsWith("-"))
            {
                var value = i < _args.Length - 1 ? _args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                _argDictionary.Add(_arg, value);
            }
        }

        return _argDictionary;
    }
}

using System;
using Unity.Netcode;
using UnityEngine;

public class TeleportManager : NetworkBehaviour
{
    [SerializeField] private Transform mainTeleportPoint;
    [SerializeField] private TeleportData[] teleportData;

    public override void OnNetworkSpawn()
    {
        Init();
    }

    private void Init()
    {
        foreach(TeleportData data in teleportData)
        {
            data.trigger.TriggerEnterEvent += () => OnTriggeredTeleportLocationChange(data);
        }
    }

    private void OnTriggeredTeleportLocationChange(TeleportData data)
    {
        mainTeleportPoint.position = data.teleportPoint.position;
    }

    [Serializable]
    public class TeleportData
    {
        public Transform teleportPoint;
        public ColliderTrigger trigger;
    }

}

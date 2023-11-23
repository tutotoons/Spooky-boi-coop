using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!IsClient)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpServerRpc();
        }
    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        NetworkManager manager = NetworkManager.Singleton;

        if(!manager.ConnectedClients.ContainsKey(OwnerClientId))
        {
            return;
        }

        var client = manager.ConnectedClients[OwnerClientId];
        var clientPlayer = client.PlayerObject.GetComponent<PlayerController>();

        clientPlayer.Jump();
    }
}

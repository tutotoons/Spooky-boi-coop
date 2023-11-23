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
        if(!IsClient || !IsOwner)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpServerRpc();
        }
    }

    [ServerRpc]
    public void JumpServerRpc()
    {
        _rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }
}

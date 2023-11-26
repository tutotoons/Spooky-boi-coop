using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneShowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out NetworkPlayer _player))
        {
            _player.ShowPhoneOnce();
            gameObject.SetActive(false);
        }
    }
}

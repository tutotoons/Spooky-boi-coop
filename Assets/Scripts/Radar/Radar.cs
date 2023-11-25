using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private string prefix;
    [SerializeField] private Transform pivot;
    [SerializeField] private TextMeshProUGUI text;

    private Transform player;
    private Transform target;

    public void SetPlayerTransform(Transform _player)
    {
        player = _player;
    }

    public void SetTargetTransform(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null || player == null)
        {
            text.text = "NaN";
            return;
        }

        UpdateRotation();
        UpdateDistance();
    }

    private void UpdateDistance()
    {
        float _dist = (target.position - player.position).magnitude;
        text.text = prefix + ((int)_dist).ToString();
    }

    private void UpdateRotation()
    {
        Quaternion _rotation = Quaternion.LookRotation(target.position - player.position, Vector3.up);
        pivot.localRotation = Quaternion.Euler(0f, _rotation.eulerAngles.y - player.rotation.eulerAngles.y, 0f);
    }
}

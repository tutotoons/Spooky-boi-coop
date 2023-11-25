using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public static RadarManager Instance
    {
        get;
        private set;
    }

    [SerializeField] private Radar monsterRadar;
    [SerializeField] private Radar objectiveRadar;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            monsterRadar.SetTargetTransform(MonsterManager.Instance.monster.transform);
        }
    }

    public void SetPlayer(Transform _playerTransform)
    {
        monsterRadar.SetPlayerTransform(_playerTransform);
        objectiveRadar.SetPlayerTransform(_playerTransform);
    }

    public void SetObjective(Transform _objectiveTransform)
    {
        objectiveRadar.SetTargetTransform(_objectiveTransform);
    }
}

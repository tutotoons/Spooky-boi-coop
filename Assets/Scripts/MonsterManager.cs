using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public BaseEntity monster;

    private void Awake()
    {
        Instance = this;
    }
}

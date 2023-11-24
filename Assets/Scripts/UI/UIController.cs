using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private NetworkPlayer player;

    private void Awake()
    {
        player.DiedEvent += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        gameOverCanvas.gameObject.SetActive(true);
    }
}

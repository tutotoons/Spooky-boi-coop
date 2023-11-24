using System;
using System.Collections;
using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    private static readonly WaitForSeconds waitForSecondsToFind = new WaitForSeconds(0.1f);

    public static IEnumerator FindPlayerDoAction(PlayerType type, Action<NetworkPlayer> foundCallback)
    {
        NetworkPlayer foundPlayer = null;

        while (foundPlayer == null)
        {
            NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();

            foreach (NetworkPlayer player in players)
            {
                if (player.PlayerType == type)
                {
                    foundPlayer = player;
                    break;
                }
            }

            yield return waitForSecondsToFind;
        }

        foundCallback.Invoke(foundPlayer);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            GameManager.Instance.playersInside.Add(player);

            GameManager.Instance.FreezeAndLockPlayer(player);

            GameManager.Instance.CheckWinCondition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (GameManager.Instance.frozenPlayers.Contains(player)) return;

            GameManager.Instance.playersInside.Remove(player);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public readonly HashSet<PlayerController> playersInside = new HashSet<PlayerController>();

    public readonly HashSet<PlayerController> frozenPlayers = new HashSet<PlayerController>();

    public List<PlayerController> allPlayers;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip moveClip;
    public GameObject levelFailedPanel;
    public GameObject levelSucceededPanel;
    public TextMeshProUGUI moveCountText;
    public int moveCount = 10;
    public bool pauseGame = false;

    private Vector3 moveDirectionThisFrame = Vector3.zero;
    private Quaternion rotateDirectionThisFrame = Quaternion.identity;

    private PlayerInputHandler playerInputHandler;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;

        playerInputHandler = GetComponent<PlayerInputHandler>();
        audioSource = GetComponent<AudioSource>();

        if (allPlayers == null) allPlayers = new List<PlayerController>();

        moveCountText.text = "Hamle Sayısı: " + moveCount;
    }

    private void Update()
    {
        if (IsAnyPlayerMoving()) return;

        if (pauseGame)
        {
            return;
        }

        if (moveCount <= 0)
        {
            LevelFailed();
            pauseGame = true;
            return;
        }

        moveDirectionThisFrame = Vector3.zero;
        rotateDirectionThisFrame = Quaternion.identity;

        if (playerInputHandler.Forward)
        {
            moveDirectionThisFrame = Vector3.forward;
            playerInputHandler.Forward = false;

            rotateDirectionThisFrame = Quaternion.Euler(0f, 0f, 0f);

            moveCount--;
            moveCountText.text = "Hamle Sayısı: " + moveCount;
        }
        else if (playerInputHandler.Backward)
        {
            moveDirectionThisFrame = Vector3.back;
            playerInputHandler.Backward = false;

            rotateDirectionThisFrame = Quaternion.Euler(0f, 180f, 0f);

            moveCount--;
            moveCountText.text = "Hamle Sayısı: " + moveCount;
        }
        else if (playerInputHandler.Left)
        {
            moveDirectionThisFrame = Vector3.left;
            playerInputHandler.Left = false;

            rotateDirectionThisFrame = Quaternion.Euler(0f, 270f, 0f);

            moveCount--;
            moveCountText.text = "Hamle Sayısı: " + moveCount;
        }
        else if (playerInputHandler.Right)
        {
            moveDirectionThisFrame = Vector3.right;
            playerInputHandler.Right = false;

            rotateDirectionThisFrame = Quaternion.Euler(0f, 90f, 0f);

            moveCount--;
            moveCountText.text = "Hamle Sayısı: " + moveCount;
        }

        if (moveDirectionThisFrame != Vector3.zero)
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {
                var player = allPlayers[i];
                if (player == null) continue;
                if (frozenPlayers.Contains(player)) continue;

                player.ReceiveMovementCommand(moveDirectionThisFrame);
                player.transform.rotation = rotateDirectionThisFrame;

                audioSource.clip = moveClip;
                audioSource.Play();
            }
        }
        else
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {
                var player = allPlayers[i];
                if (player == null) continue;

                player.animator.SetBool("isRun", false);
                player.animator.SetBool("Obstacle", false);
            }
        }
    }

    public void FreezeAndLockPlayer(PlayerController player)
    {
        if (player == null) return;
        if (frozenPlayers.Contains(player)) return;

        frozenPlayers.Add(player);
    }

    public void FreezeSelectedPlayer(int index)
    {
        if (index >= 0 && index < allPlayers.Count && allPlayers[index] != null)
            allPlayers[index].FreezePlayer();
    }

    public void ReverseSelectedPlayer(int index)
    {
        if (index >= 0 && index < allPlayers.Count && allPlayers[index] != null)
            allPlayers[index].ReversePlayer();
    }

    private bool IsAnyPlayerMoving()
    {
        foreach (var player in allPlayers)
        {
            if (player != null && player.GetIsMoving()) return true;
        }
        return false;
    }

    public void LevelFailed()
    {
        pauseGame = true;

        audioSource.clip = loseClip;
        audioSource.Play();
        levelFailedPanel.SetActive(true);
    }

    public void LevelSucceeded()
    {
        pauseGame = true;

        audioSource.clip = winClip;
        audioSource.Play();
        levelSucceededPanel.SetActive(true);
    }

    public void CheckWinCondition()
    {
        var allPlayers = this.allPlayers;

        if (allPlayers.Count == 0) return;

        foreach (var p in allPlayers)
        {
            if (!playersInside.Contains(p))
                return;
        }

        LevelSucceeded();
    }
}

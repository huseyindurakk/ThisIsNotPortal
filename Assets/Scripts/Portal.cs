using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public Portal destinationPortal;
    public Transform exitPoint;

    public AudioClip teleportClip;

    public float teleportDuration = 0.5f;
    private bool isTeleporting = false;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;
        if (!other.CompareTag("Player")) return;

        if (destinationPortal == null || exitPoint == null)
        {
            Debug.LogWarning($"[Portal] {name}: destinationPortal or exitPoint is not set. Teleport aborted.");
            return;
        }

        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null) return;

        Debug.Log(other.name + " entered a portal. Starting teleport animation...");

        audioSource.clip = teleportClip;
        audioSource.Play();

        isTeleporting = true;
        if (destinationPortal != null)
            destinationPortal.isTeleporting = true;

        StartCoroutine(player.TeleportThroughPortal(
            transform.position,
            destinationPortal.exitPoint.position,
            teleportDuration,
            this,
            destinationPortal
        ));
    }

    public void OnTeleportFinished()
    {
        isTeleporting = false;
    }
}

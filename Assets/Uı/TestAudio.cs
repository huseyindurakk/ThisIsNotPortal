using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioClip testClip;  // Ses dosyasýný atayýn
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
            return;
        }

        if (testClip != null)
        {
            audioSource.PlayOneShot(testClip);
            Debug.Log("Test sound is playing.");
        }
        else
        {
            Debug.LogError("Test clip is missing.");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour
{
    public AudioClip buttonClickClip;  // Ses dosyasýný atamak için
    private AudioSource audioSource;  // AudioSource referansý

    public Button yourButton;  // Buton referansý

    void Start()
    {
        // AudioSource bileþenini al
        audioSource = GetComponent<AudioSource>();

        // Butona týklama eventini ekle
        yourButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Ses kaynaðý ve ses dosyasý varsa, ses çal ve objeyi devre dýþý býrak
        if (audioSource && buttonClickClip)
        {
            audioSource.Play(); // Ses çal
            Debug.Log("Butona týklandý, ses çalýyor.");
            gameObject.GetComponent<Image>().enabled = false; // Objeyi devre dýþý býrak
        }
    }


    // UI elemanýný yok et

}


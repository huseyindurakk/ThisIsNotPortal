using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour
{
    public AudioClip buttonClickClip;  // Ses dosyas�n� atamak i�in
    private AudioSource audioSource;  // AudioSource referans�

    public Button yourButton;  // Buton referans�

    void Start()
    {
        // AudioSource bile�enini al
        audioSource = GetComponent<AudioSource>();

        // Butona t�klama eventini ekle
        yourButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Ses kayna�� ve ses dosyas� varsa, ses �al ve objeyi devre d��� b�rak
        if (audioSource && buttonClickClip)
        {
            audioSource.Play(); // Ses �al
            Debug.Log("Butona t�kland�, ses �al�yor.");
            gameObject.GetComponent<Image>().enabled = false; // Objeyi devre d��� b�rak
        }
    }


    // UI eleman�n� yok et

}


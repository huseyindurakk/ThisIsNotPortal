using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicToggleButton : MonoBehaviour
{
    public AudioSource musicSource; // M�zik i�in AudioSource
    public Button toggleButton;     // Buton referans�
    public TMP_Text musicText;      // TMP yaz�s� (TextMeshPro)
    private bool isMusicPlaying = true; // M�zik durumu

    void Start()
    {
        // Ba�lang��ta yaz� rengini g�ncelle
        UpdateTextOpacity();

        // Butona t�klama eventini ekle
        toggleButton.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            musicSource.Pause(); // M�zi�i durdur
            isMusicPlaying = false;
        }
        else
        {
            musicSource.Play(); // M�zi�i ba�lat
            isMusicPlaying = true;
        }

        // Yaz� rengini g�ncelle
        UpdateTextOpacity();
    }

    void UpdateTextOpacity()
    {
        if (musicText != null)
        {
            // Opakl�k de�erini belirle (1f: tam g�r�n�r, 0.5f: yar� saydam)
            float opacity = isMusicPlaying ? 1f : 0.5f;

            // TMP_Text'in rengini de�i�tir
            Color color = musicText.color;
            color.a = opacity;
            musicText.color = color;
        }
    }
}

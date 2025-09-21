using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicToggleButton : MonoBehaviour
{
    public AudioSource musicSource; // Müzik için AudioSource
    public Button toggleButton;     // Buton referansý
    public TMP_Text musicText;      // TMP yazýsý (TextMeshPro)
    private bool isMusicPlaying = true; // Müzik durumu

    void Start()
    {
        // Baþlangýçta yazý rengini güncelle
        UpdateTextOpacity();

        // Butona týklama eventini ekle
        toggleButton.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            musicSource.Pause(); // Müziði durdur
            isMusicPlaying = false;
        }
        else
        {
            musicSource.Play(); // Müziði baþlat
            isMusicPlaying = true;
        }

        // Yazý rengini güncelle
        UpdateTextOpacity();
    }

    void UpdateTextOpacity()
    {
        if (musicText != null)
        {
            // Opaklýk deðerini belirle (1f: tam görünür, 0.5f: yarý saydam)
            float opacity = isMusicPlaying ? 1f : 0.5f;

            // TMP_Text'in rengini deðiþtir
            Color color = musicText.color;
            color.a = opacity;
            musicText.color = color;
        }
    }
}

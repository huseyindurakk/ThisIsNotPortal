using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggleButton : MonoBehaviour
{
    public AudioSource[] soundSources; // Birden fazla ses kaynaðý için dizi
    public Button toggleButton;        // Buton referansý
    public TMP_Text soundText;         // TMP yazýsý (TextMeshPro)
    private bool areSoundsPlaying = true; // Seslerin oynama durumu

    void Start()
    {
        // Baþlangýçta yazý rengini güncelle
        UpdateTextOpacity();

        // Butona týklama eventini ekle
        toggleButton.onClick.AddListener(ToggleSounds);
    }

    void ToggleSounds()
    {
        if (areSoundsPlaying)
        {
            // Tüm ses kaynaklarýný durdur
            foreach (var source in soundSources)
            {
                source.enabled = false; // Müziði durdur
            }
            areSoundsPlaying = false;
        }
        else
        {
            // Tüm ses kaynaklarýný baþlat
            foreach (var source in soundSources)
            {
                source.enabled = false; // Müziði baþlat
            }
            areSoundsPlaying = true;
        }

        // Yazý rengini güncelle
        UpdateTextOpacity();
    }

    void UpdateTextOpacity()
    {
        if (soundText != null)
        {
            // Opaklýk deðerini belirle (1f: tam görünür, 0.5f: yarý saydam)
            float opacity = areSoundsPlaying ? 1f : 0.5f;

            // TMP_Text'in rengini deðiþtir
            Color color = soundText.color;
            color.a = opacity;
            soundText.color = color;
        }
    }
}

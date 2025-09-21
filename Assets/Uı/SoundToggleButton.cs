using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggleButton : MonoBehaviour
{
    public AudioSource[] soundSources; // Birden fazla ses kayna�� i�in dizi
    public Button toggleButton;        // Buton referans�
    public TMP_Text soundText;         // TMP yaz�s� (TextMeshPro)
    private bool areSoundsPlaying = true; // Seslerin oynama durumu

    void Start()
    {
        // Ba�lang��ta yaz� rengini g�ncelle
        UpdateTextOpacity();

        // Butona t�klama eventini ekle
        toggleButton.onClick.AddListener(ToggleSounds);
    }

    void ToggleSounds()
    {
        if (areSoundsPlaying)
        {
            // T�m ses kaynaklar�n� durdur
            foreach (var source in soundSources)
            {
                source.enabled = false; // M�zi�i durdur
            }
            areSoundsPlaying = false;
        }
        else
        {
            // T�m ses kaynaklar�n� ba�lat
            foreach (var source in soundSources)
            {
                source.enabled = false; // M�zi�i ba�lat
            }
            areSoundsPlaying = true;
        }

        // Yaz� rengini g�ncelle
        UpdateTextOpacity();
    }

    void UpdateTextOpacity()
    {
        if (soundText != null)
        {
            // Opakl�k de�erini belirle (1f: tam g�r�n�r, 0.5f: yar� saydam)
            float opacity = areSoundsPlaying ? 1f : 0.5f;

            // TMP_Text'in rengini de�i�tir
            Color color = soundText.color;
            color.a = opacity;
            soundText.color = color;
        }
    }
}

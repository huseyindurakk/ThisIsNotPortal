using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    public Button playButton; // Play butonu
    public GameObject joinButton; // Join butonu
    public GameObject createButton; // Create butonu

    private bool isExpanded = false; // Butonlar�n a��k/kapal� durumunu kontrol eden de�i�ken

    void Start()
    {
        // Ba�lang��ta Join ve Create butonlar�n� gizle
        joinButton.SetActive(false);
        createButton.SetActive(false);

        // Play butonuna t�klama olay�n� dinle
        playButton.onClick.AddListener(ToggleButtons);
    }

    void ToggleButtons()
    {
        // Butonlar�n a��k/kapal� durumunu tersine �evir
        isExpanded = !isExpanded;

        // Join ve Create butonlar�n�n durumunu ayarla
        joinButton.SetActive(isExpanded);
        createButton.SetActive(isExpanded);

        if (isExpanded)
        {
            // Pozisyonlar� ayarla (opsiyonel)
            RectTransform playRect = playButton.GetComponent<RectTransform>();
            RectTransform joinRect = joinButton.GetComponent<RectTransform>();
            RectTransform createRect = createButton.GetComponent<RectTransform>();

            joinRect.anchoredPosition = playRect.anchoredPosition + new Vector2(-20, -75);
            createRect.anchoredPosition = playRect.anchoredPosition + new Vector2(20, -150);
        }
    }
}

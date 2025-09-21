using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    public Button playButton; // Play butonu
    public GameObject joinButton; // Join butonu
    public GameObject createButton; // Create butonu

    private bool isExpanded = false; // Butonlarýn açýk/kapalý durumunu kontrol eden deðiþken

    void Start()
    {
        // Baþlangýçta Join ve Create butonlarýný gizle
        joinButton.SetActive(false);
        createButton.SetActive(false);

        // Play butonuna týklama olayýný dinle
        playButton.onClick.AddListener(ToggleButtons);
    }

    void ToggleButtons()
    {
        // Butonlarýn açýk/kapalý durumunu tersine çevir
        isExpanded = !isExpanded;

        // Join ve Create butonlarýnýn durumunu ayarla
        joinButton.SetActive(isExpanded);
        createButton.SetActive(isExpanded);

        if (isExpanded)
        {
            // Pozisyonlarý ayarla (opsiyonel)
            RectTransform playRect = playButton.GetComponent<RectTransform>();
            RectTransform joinRect = joinButton.GetComponent<RectTransform>();
            RectTransform createRect = createButton.GetComponent<RectTransform>();

            joinRect.anchoredPosition = playRect.anchoredPosition + new Vector2(-20, -75);
            createRect.anchoredPosition = playRect.anchoredPosition + new Vector2(20, -150);
        }
    }
}

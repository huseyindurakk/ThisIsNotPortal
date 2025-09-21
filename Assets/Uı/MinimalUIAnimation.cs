using UnityEngine;

public class MinimalUIAnimation : MonoBehaviour
{
    public RectTransform targetImage; // Animasyon uygulanacak UI element
    public float movementRange = 10f; // Hareketin maksimum mesafesi (pixel olarak)
    public float speed = 2f; // Hareket h�z�

    private Vector2 originalPosition;

    void Start()
    {
        // Objenin ba�lang�� pozisyonunu kaydet
        originalPosition = targetImage.anchoredPosition;
    }

    void Update()
    {
        // Sin�s fonksiyonu kullanarak objeyi minimal hareket ettir
        float offset = Mathf.Sin(Time.time * speed) * movementRange;
        targetImage.anchoredPosition = originalPosition + new Vector2(0, offset); // Yukar�-a�a�� hareket
    }
}

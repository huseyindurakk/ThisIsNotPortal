using UnityEngine;

public class MinimalUIAnimation : MonoBehaviour
{
    public RectTransform targetImage; // Animasyon uygulanacak UI element
    public float movementRange = 10f; // Hareketin maksimum mesafesi (pixel olarak)
    public float speed = 2f; // Hareket hýzý

    private Vector2 originalPosition;

    void Start()
    {
        // Objenin baþlangýç pozisyonunu kaydet
        originalPosition = targetImage.anchoredPosition;
    }

    void Update()
    {
        // Sinüs fonksiyonu kullanarak objeyi minimal hareket ettir
        float offset = Mathf.Sin(Time.time * speed) * movementRange;
        targetImage.anchoredPosition = originalPosition + new Vector2(0, offset); // Yukarý-aþaðý hareket
    }
}

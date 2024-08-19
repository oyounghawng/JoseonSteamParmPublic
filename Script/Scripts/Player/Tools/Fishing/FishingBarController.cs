using UnityEngine;

public class FishingBarController : MonoBehaviour
{
    public float moveSpeed;
    private RectTransform rectTransform;
    public RectTransform parentTransform;

    private float miny;
    private float maxy;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        miny = parentTransform.localPosition.y - (parentTransform.rect.height / 2) + (rectTransform.rect.height / 2);
        maxy = parentTransform.localPosition.y + (parentTransform.rect.height / 2) - (rectTransform.rect.height / 2);

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, miny);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            rectTransform.anchoredPosition += Vector2.down * moveSpeed * Time.deltaTime;
        }
        float clampedY = Mathf.Clamp(rectTransform.anchoredPosition.y, miny, maxy);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, clampedY);
    }
}
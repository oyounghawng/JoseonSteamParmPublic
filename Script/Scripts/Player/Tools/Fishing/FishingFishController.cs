using UnityEngine;

public class FishingFishController : MonoBehaviour
{
    public float moveSpeed = 200f; // 움직임 속도 조정
    private RectTransform rectTransform;
    public RectTransform parentTransform;

    private float minY;
    private float maxY;
    private Vector2 targetPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        minY = parentTransform.rect.yMin + rectTransform.rect.height / 2;
        maxY = parentTransform.rect.yMax - rectTransform.rect.height / 2;

        SetInitialPosition(); // 초기 위치 설정
    }

    void Update()
    {
        MoveFish();
    }

    public void SetInitialPosition()
    {
        float initialY = minY; // 맨 아래에서 시작
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialY);
        SetRandomTargetPosition();
    }

    private void MoveFish()
    {
        // 물고기를 targetPosition으로 이동
        rectTransform.localPosition = Vector2.MoveTowards(rectTransform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        // targetPosition에 도달하거나 이동 도중에도 새로운 랜덤 위치를 설정
        if (Vector2.Distance(rectTransform.localPosition, targetPosition) < 0.1f || Random.value < 0.01f)
        {
            SetRandomTargetPosition();
        }
    }

    private void SetRandomTargetPosition()
    {
        float randomY = Random.Range(minY, maxY);
        targetPosition = new Vector2(rectTransform.anchoredPosition.x, randomY);
    }
}

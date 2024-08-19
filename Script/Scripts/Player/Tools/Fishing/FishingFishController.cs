using UnityEngine;

public class FishingFishController : MonoBehaviour
{
    public float moveSpeed = 200f; // ������ �ӵ� ����
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

        SetInitialPosition(); // �ʱ� ��ġ ����
    }

    void Update()
    {
        MoveFish();
    }

    public void SetInitialPosition()
    {
        float initialY = minY; // �� �Ʒ����� ����
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialY);
        SetRandomTargetPosition();
    }

    private void MoveFish()
    {
        // ����⸦ targetPosition���� �̵�
        rectTransform.localPosition = Vector2.MoveTowards(rectTransform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        // targetPosition�� �����ϰų� �̵� ���߿��� ���ο� ���� ��ġ�� ����
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

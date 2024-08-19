using UnityEngine;
using UnityEngine.UI;

public class UI_FishingMiniGame : UI_Popup
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public FishData fishData;

    public FishingBarController fishingBar;
    public FishingFishController fishingFish;
    public Slider fishingGauge;
    public float gaugeDecreaseRate;
    public float gaugeIncreaseRate = 0.0002f;

    private bool isEndGame = false;
    public bool IsEndGame => isEndGame;
    public float easyGaugeDecreaseRate = 0.6f;
    public float normalGaugeDecreaseRate = 0.7f;
    public float hardGaugeDecreaseRate = 0.8f;
    public Image fishShadow;

    private Difficulty currentDifficulty;
    public GameObject fishprefab;

    private void Start()
    {
        fishingGauge.value = 0.3f;

        StartMiniGame();
    }

    private void Update()
    {
        if (isEndGame)
            return;

        // �ٿ� ������� �浹 �˻�
        if (IsBarOverlappingFish())
        {
            // ������ ����
            fishingGauge.value += gaugeIncreaseRate * Time.deltaTime;
        }
        else
        {
            // ������ ����
            fishingGauge.value -= gaugeDecreaseRate * Time.deltaTime;
        }

        // ������ üũ
        if (fishingGauge.value >= 1f)
        {
            EndMiniGame(true);
        }
        else if (fishingGauge.value <= 0f)
        {
            EndMiniGame(false);
        }
    }

    public void StartMiniGame()
    {
        // �̴ϰ��� ���� �� ���� �ʱ�ȭ
        ResetMiniGameState();
        fishingGauge.value = 0.23f; // �������� �߰� ������ ����
        fishingBar.enabled = true;
        fishingFish.enabled = true;
        fishingFish.SetInitialPosition();
        fishShadow.sprite = fishprefab.GetComponent<ItemObject>().itemObjectData.silluetImage;
        SetDifficulty(GetRandomDifficulty());  // �̴ϰ��� ���� �� ���̵��� �������� ����
    }

    private async void EndMiniGame(bool success)
    {
        if (isEndGame)
            return;

        isEndGame = true;
        fishingBar.enabled = false;
        fishingFish.enabled = false;

        if (success)
        {
            // ����� ������ �ν��Ͻ�ȭ TODO ""�� �������� ����ϱ�
            Vector3 pos = (SceneManagerEx.Instance.CurrentScene as GameScene).player.transform.position;
            GameObject go = Instantiate(await ResourceManager.Instance.LoadAsset<GameObject>(fishData.RewardItem.ToLower(), eAddressableType.prefab));
            go.transform.position = pos + new Vector3(0, 1, 0);
        }
    }

    private void ResetMiniGameState()
    {
        isEndGame = false;
        fishingGauge.value = 0.3f;
        fishingBar.enabled = false;
        fishingFish.enabled = false;
    }

    private bool IsBarOverlappingFish()
    {
        // �ٿ� ������� RectTransform ��������
        RectTransform barRect = fishingBar.GetComponent<RectTransform>();
        RectTransform fishRect = fishingFish.GetComponent<RectTransform>();

        // �ٿ� ������� ��ġ �� ũ�� ���� ��������
        Rect barWorldRect = new Rect(barRect.position.x - barRect.rect.width / 2, barRect.position.y - barRect.rect.height / 2, barRect.rect.width, barRect.rect.height);
        Rect fishWorldRect = new Rect(fishRect.position.x - fishRect.rect.width / 2, fishRect.position.y - fishRect.rect.height / 2, fishRect.rect.width, fishRect.rect.height);

        // �浹 ���� ��ȯ
        return barWorldRect.Overlaps(fishWorldRect);
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gaugeDecreaseRate = easyGaugeDecreaseRate;
                fishingFish.moveSpeed = 200f;  // ����� �̵� �ӵ��� ���̵��� ���� ����
                break;
            case Difficulty.Normal:
                gaugeDecreaseRate = normalGaugeDecreaseRate;
                fishingFish.moveSpeed = 300f;
                break;
            case Difficulty.Hard:
                gaugeDecreaseRate = hardGaugeDecreaseRate;
                fishingFish.moveSpeed = 450f;
                break;
        }
    }

    private Difficulty GetRandomDifficulty()
    {
        int value = fishData.Difficulty;
        return (Difficulty)value;
    }
}

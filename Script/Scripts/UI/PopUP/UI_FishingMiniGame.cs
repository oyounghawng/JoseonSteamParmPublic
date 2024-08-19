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

        // 바와 물고기의 충돌 검사
        if (IsBarOverlappingFish())
        {
            // 게이지 증가
            fishingGauge.value += gaugeIncreaseRate * Time.deltaTime;
        }
        else
        {
            // 게이지 감소
            fishingGauge.value -= gaugeDecreaseRate * Time.deltaTime;
        }

        // 게이지 체크
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
        // 미니게임 시작 시 상태 초기화
        ResetMiniGameState();
        fishingGauge.value = 0.23f; // 게이지를 중간 정도로 설정
        fishingBar.enabled = true;
        fishingFish.enabled = true;
        fishingFish.SetInitialPosition();
        fishShadow.sprite = fishprefab.GetComponent<ItemObject>().itemObjectData.silluetImage;
        SetDifficulty(GetRandomDifficulty());  // 미니게임 시작 시 난이도를 랜덤으로 설정
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
            // 물고기 프리팹 인스턴스화 TODO ""에 뭐넣을지 고민하기
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
        // 바와 물고기의 RectTransform 가져오기
        RectTransform barRect = fishingBar.GetComponent<RectTransform>();
        RectTransform fishRect = fishingFish.GetComponent<RectTransform>();

        // 바와 물고기의 위치 및 크기 정보 가져오기
        Rect barWorldRect = new Rect(barRect.position.x - barRect.rect.width / 2, barRect.position.y - barRect.rect.height / 2, barRect.rect.width, barRect.rect.height);
        Rect fishWorldRect = new Rect(fishRect.position.x - fishRect.rect.width / 2, fishRect.position.y - fishRect.rect.height / 2, fishRect.rect.width, fishRect.rect.height);

        // 충돌 여부 반환
        return barWorldRect.Overlaps(fishWorldRect);
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gaugeDecreaseRate = easyGaugeDecreaseRate;
                fishingFish.moveSpeed = 200f;  // 물고기 이동 속도도 난이도에 따라 조절
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

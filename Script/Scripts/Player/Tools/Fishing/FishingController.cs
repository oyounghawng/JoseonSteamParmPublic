using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingController : MonoBehaviour
{
    private UI_FishingMiniGame UI_FishingMiniGame; // 미니게임 추가
    private bool isFishing = false; // 낚시 중인지 여부
    public event Action<bool> onStartFishing;
    public event Action onEndFishing;

    private Coroutine coFish = null;

    List<FishData> fishdatas;

    private void OnEnable()
    {
        TimeManager.Instance.onEndDay -= StopAllCoroutines;

        TimeManager.Instance.onEndDay += StopAllCoroutines;
    }

    public void FishingCorotuine(Define.WaterSpot spot)
    {
        if (!isFishing)
        {
            isFishing = true;
            if (coFish != null)
                StopCoroutine(coFish);
            coFish = StartCoroutine(Fish(spot));
        }
    }
    public void StopFishing()
    {
        if (coFish != null)
        {
            isFishing = false;
            onStartFishing?.Invoke(isFishing);
            StopCoroutine(coFish);
        }
    }
    private IEnumerator Fish(Define.WaterSpot spot)
    {
        // TODO: 낚싯대 던지기 애니메이션 재생
        Managers.Sound.Play(Define.Sound.Effect, "Effect/Fish_throw", Managers.Sound.EffectVolume);
        yield return new WaitForSeconds(2.0f); // 애니메이션이 끝날 때까지 대기
        // 랜덤한 시간을 기다림
        float randomWaitTime = Random.Range(3.0f, 8.0f);
        yield return new WaitForSeconds(randomWaitTime);
        onStartFishing?.Invoke(true);
        fishdatas = new List<FishData>();
        int season = TimeManager.Instance.Season; ;
        fishdatas = DataManager.Instance.FishDatas.Values.Where(data => data.Spot.Equals(spot.ToString())
        && data.Season.Contains(season)).ToList();
        FishData data = null;
        int count = fishdatas.Count;
        int idx = Random.Range(0, count);
        data = fishdatas[idx];
        StartFishingMiniGame(data); // 낚시 미니게임 시작
        yield return new WaitUntil(() => UI_FishingMiniGame);
        yield return new WaitUntil(() => UI_FishingMiniGame.IsEndGame);
        yield return CoroutineHelper.WaitForSeconds(0.25f);
        EndFishingMiniGame();
    }
    private async void StartFishingMiniGame(FishData fishData)
    {
        GameObject rewardItemPrefab = await ResourceManager.Instance.LoadAsset<GameObject>(fishData.RewardItem.ToLower(), eAddressableType.prefab);
        UI_FishingMiniGame = await UIManager.Instance.ShowTaskPopupUI<UI_FishingMiniGame>();
        UI_FishingMiniGame.fishData = fishData;
        UI_FishingMiniGame.fishprefab = rewardItemPrefab;
    }

    public void EndFishingMiniGame()
    {
        if (UI_FishingMiniGame != null)
        {
            UIManager.Instance.ClosePopupUI();
            UI_FishingMiniGame = null;
        }
        isFishing = false; // 낚시 중 상태 해제
        // TODO: 물고기 잡기 후 처리 로직 추가

        Managers.Sound.Play(Define.Sound.Effect, "Effect/Fish_catch",  Managers.Sound.EffectVolume);
        onEndFishing?.Invoke();
        onEndFishing = null;
    }
}

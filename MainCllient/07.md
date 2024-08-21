# Fishing
-   
    <aside>
    💡 **FishingController에서 모든 낚시의 과정을 관리**
    
    ---
    
    *📝 **낚시대에 있는 Rod스크립트로 모든 과정을 관리***
    
    - 코루틴을 통해 낚시 과정을 순차적으로 진행
    - 코드
        
        ```css
            private IEnumerator Fish(Define.WaterSpot spot)
            {
                Managers.Sound.Play(Define.Sound.Effect, "Effect/Fish_throw");
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
        ```
        
    
    📝 **UI_FishingMiniGame에서 실제 게임을 진행**
    
    ---
    
    - UI_FishingMiniGae에서 미니 게임의 진행을 관리
    - Update문에서 차오르는 게이지의 상태에 따라 게임 실패, 성공을 판단
    - 코드
        
        ```css
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
                    Debug.Log("Success! You caught the fish!");
                    EndMiniGame(true);
                }
                else if (fishingGauge.value <= 0f)
                {
                    Debug.Log("Failed! The fish got away!");
                    EndMiniGame(false);
                }
            }
        ```
        
    </aside>

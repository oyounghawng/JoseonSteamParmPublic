# JoseonSteamParmPublic
조선스팀팜 코드공개용 레퍼지터리

- Fishing
    
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
    
    
    <aside>
    💡 **Singleton을 활용한 매니저 관리**
    
    ---
    
    📝 **게임이 시작되면 Splash Image가 나오는 동안 Managers에서 각 매니저에서 필요한 데이터들을 저장 및 로드 하여 초기화 합니다.**
    
    - **코드의 가시성을 살리고 접근성을 높이기 위해 Managers라는 하나의 싱글톤에서 중요한 매니저들을 관리했습니다.**
    - **Scene의 이동 시, Data, UI 등을 초기화 또는 정리해야 하는 경우, Managers에서 각 매니저의 초기화 또는 정리 기능을 모아둠으로써 편리성을 얻을 수 있습니다.**
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/d3da1a0b-e923-4017-97c3-f0b70df70677/image.png)
    
    📝 MonoBehaviour 상속 필요 여부에 따라 Singleton과 MonoSingleton을 상속하는 Manager를 나누어 관리합니다.
    
    ---
    
    - 오브젝트를 생성할 필요가 있는 클래스와 생성할 필요가 없는 오브젝트를 구분함으로써 불필요한 생성을 줄입니다.
    - 싱글톤 중복 생성을 최대한 방지할 수 있습니다.
    - 씬에 따라 필요할 매니저가 존재하기에 이를 나누어서 관리할 수 있습니다.
    
    - 코드
        
        ```csharp
        public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
        {
            private static T instance;
            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            instance = obj.AddComponent<T>();
                        }
                    }
                    return instance;
                }
                set
                {
                    instance = value;
                }
            }
            protected virtual void Awake()
            {
                if (instance == null)
                {
                    instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        
            public virtual void Init()
            {
        
            }
        }
        ```
        
        ```csharp
        public class Singleton<T>
        {
            private static T instance;
            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = (T)Activator.CreateInstance(typeof(T));
                    }
                    return instance;
                }
            }
            public virtual void Init()
            {
        
            }
        }
        
        ```
        
    
    </aside>
    
# Managers    

-
    <aside>
    💡 **Singleton을 활용한 매니저 관리**
    
    ---
    
    📝 **게임이 시작되면 Splash Image가 나오는 동안 Managers에서 각 매니저에서 필요한 데이터들을 저장 및 로드 하여 초기화 합니다.**
    
    - **코드의 가시성을 살리고 접근성을 높이기 위해 Managers라는 하나의 싱글톤에서 중요한 매니저들을 관리했습니다.**
    - **Scene의 이동 시, Data, UI 등을 초기화 또는 정리해야 하는 경우, Managers에서 각 매니저의 초기화 또는 정리 기능을 모아둠으로써 편리성을 얻을 수 있습니다.**

    ![image](https://github.com/user-attachments/assets/d18becd8-9b89-44d1-8863-de91d06c16ac)

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

using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    //튜토리얼을 시작할때
    public abstract void Enter();

    //과정을 진행중일때 프레임마다 호출
    public abstract void Execute(TutorialController controller);

    //튜토리얼을 종료할때
    public abstract void Exit();
}

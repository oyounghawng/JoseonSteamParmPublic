using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    //Ʃ�丮���� �����Ҷ�
    public abstract void Enter();

    //������ �������϶� �����Ӹ��� ȣ��
    public abstract void Execute(TutorialController controller);

    //Ʃ�丮���� �����Ҷ�
    public abstract void Exit();
}

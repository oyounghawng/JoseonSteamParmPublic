using UnityEngine;

public class VillageSystem : MonoBehaviour
{
    private VillageData data;
    public VillageData Data
    {
        get
        {
            if (data == null)
            {
                // data = Managers.Game.SaveData.villageData;
            }
            return data;
        }
        set
        {
            data = value;

        }
    }

    // TODO : ���� Ȯ��
    public void ExtendVillage()
    {

    }
}

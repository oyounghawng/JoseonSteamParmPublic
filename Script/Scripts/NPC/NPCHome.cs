using UnityEngine;

public class NPCHome : MonoBehaviour
{
    private NPC npcOwner;
    public NPC NPCOwner
    {
        get => npcOwner;
        set
        {
            npcOwner = value;
            buildingInfo.ownerName = npcOwner.NPCData.rcode;
            buildingInfo.buildingName = gameObject.name;
            SaveBuiildingData();
        }
    }

    public BuildingInfo buildingInfo = null;
    public SpriteRenderer spRenderer;
    public Define.NPCRank rank;

    private void SaveBuiildingData()
    {
        var list = Managers.Game.SaveData.villageData.buildingInfo;
        int index = list.IndexOf(buildingInfo);
        list.Remove(buildingInfo);
        list.Insert(index, buildingInfo);
    }

    public void SetOwner(NPC npcOwner)
    {
        NPCOwner = npcOwner;
    }
}

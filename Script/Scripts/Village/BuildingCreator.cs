using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingCreator
{
    // ��밡���� ����
    public List<NPCHome> home = new List<NPCHome>();

    private bool isInit = false;
    public void Init()
    {
        if (!isInit)
        {
            home = Managers.Resource.LoadAll<NPCHome>("Prefabs/Village").ToList();
            isInit = true;
        }
    }

    public NPCHome CreateNPCHome(NPCData data)
    {
        var list = Managers.Game.SaveData.villageData.buildingInfo;

        switch (data.type)
        {
            case Define.NPCType.Normal:
                // ����� ������ ��������

                // �ش� NPC�� ������ �����ϰ� �ִ��� �Ǵ�
                var foundBuilding = list.Find(building => building.ownerName.Equals(data.rcode));

                // NPC �� ������ ��������
                NPCHome[] homes = home.Where(building => building.rank == data.rank).ToArray();

                // ���� ������ �ϰ� ���� �ʴٸ�
                if (foundBuilding == null)
                {
                    // �������� �����͸� �ֽ��ϴ�.
                    int count = Random.Range(0, homes.Length);


                    foreach (var building in list)
                    {

                        if (building.ownerName.Equals(string.Empty))
                        {
                            homes[count].buildingInfo = building;
                            return homes[count];
                        }
                    }
                }
                else
                {
                    foreach (var building in homes)
                    {
                        if (building.name.Equals(foundBuilding.buildingName))
                        {
                            building.buildingInfo = foundBuilding;
                            return building;
                        }
                    }
                }

                break;


            case Define.NPCType.Main:
                var mainNPCBuildingInfo = list.Find(building => building.ownerName.Equals(string.Empty));

                var mainBuilding = home.Find(x => x.name.Equals(data.homeCode));
                mainBuilding.buildingInfo = mainNPCBuildingInfo;

                return mainBuilding;
        }
        return null;
    }
}

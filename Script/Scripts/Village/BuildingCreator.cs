using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingCreator
{
    // 사용가능한 에셋
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
                // 저장된 데이터 가져오기

                // 해당 NPC가 빌딩을 소유하고 있는지 판단
                var foundBuilding = list.Find(building => building.ownerName.Equals(data.rcode));

                // NPC 집 데이터 가져오기
                NPCHome[] homes = home.Where(building => building.rank == data.rank).ToArray();

                // 빌딩 소유를 하고 있지 않다면
                if (foundBuilding == null)
                {
                    // 랜덤으로 데이터를 넣습니다.
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

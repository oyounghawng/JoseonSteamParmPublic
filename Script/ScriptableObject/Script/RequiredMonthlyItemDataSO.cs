using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequiredMontlyItemData", menuName = "ItemSlot/RequiredMontlyItemData", order = 1)]
public class RequiredMonthlyItemDataSO : ScriptableObject
{
    public int season;
    public List<RequiredItemDataSO> requiredItemDatas;
}

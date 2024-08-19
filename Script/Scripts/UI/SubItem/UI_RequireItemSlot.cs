using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RequireItemSlot : UI_Base
{
    ItemObjectData[] playerInventory;
    public int id = 0;

    public int cuurentItemCount = 0;

    [SerializeField]
    private RequiredItemDataSO requireData = null;

    public RequiredItemDataSO RequireData
    {
        get
        {
            return requireData;
        }
        set
        {
            requireData = value;

            playerInventory = (Managers.Scene.CurrentScene as GameScene).player.playerInventory;

            cuurentItemCount = requireData.type != Define.ItemType.GoldItem
                ? playerInventory.Where(require => require != null && require.itemKey.Equals(requireData.requiredItem.itemObjectData.itemKey)).Sum(item => item.count)
                : 0;

            itemProfile.sprite = requireData.type != Define.ItemType.GoldItem ? requireData.requiredItem.itemObjectData.image : requireData.image;
                
            requiredCounter.text = requireData.type != Define.ItemType.GoldItem ? 
                $"{cuurentItemCount} / {requireData.reuqiredItemStack}" : $"{Managers.Game.SaveData.Gold} / {requireData.reuqiredItemStack}";
        }
    }

    public Image itemProfile = null;
    public TMP_Text requiredCounter = null;

    private void OnEnable()
    {
        if(RequireData != null)
        {
            playerInventory = (Managers.Scene.CurrentScene as GameScene).player.playerInventory;

            cuurentItemCount = requireData.type != Define.ItemType.GoldItem
                ? playerInventory.Where(require => require != null && require.Equals(requireData?.requiredItem?.itemObjectData)).Sum(item => item.count)
                : 0;

            itemProfile.sprite = requireData.type != Define.ItemType.GoldItem ? requireData.requiredItem.itemObjectData.image : requireData.image;

            requiredCounter.text = requireData.type != Define.ItemType.GoldItem ?
                $"{cuurentItemCount} / {requireData.reuqiredItemStack}" : $"{Managers.Game.SaveData.Gold} / {requireData.reuqiredItemStack}";
        }  
    }

    public override void Init() { }

}

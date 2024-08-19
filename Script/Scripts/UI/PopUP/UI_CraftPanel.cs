using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftPanel : UI_Base
{
    enum Buttons
    {
        BoxMakeBtn,
        UpgradeBag
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.BoxMakeBtn).gameObject.BindEvent(MakeItem);
        GetButton((int)Buttons.UpgradeBag).gameObject.BindEvent(UpgradePlayerBag);
    }

    async void MakeItem(PointerEventData evt)
    {
        Box go = await ResourceManager.Instance.LoadAsset<Box>("ITE00113".ToLower(), eAddressableType.prefab);
        (Managers.Scene.CurrentScene as GameScene).player.AddItem(go.GetComponent<ItemObject>().itemObjectData);
    }

    private void UpgradePlayerBag(PointerEventData evt)
    {
        (Managers.Scene.CurrentScene as GameScene).player.PlayerInventoryLevel = 1;
        Managers.UI.CloseAllPopupUI();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopSlot : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    protected ItemData data;
    protected ItemObject itemOjbect;
    protected Image ItemImage;
    protected TextMeshProUGUI priceText;
    private Sprite mainimage;

    enum Images
    {
        ItemImage,
    }
    enum Texts
    {
        Price
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        ItemImage = GetImage((int)Images.ItemImage);
        priceText = GetText((int)Texts.Price);
        GetImage();
        ItemImage.gameObject.BindEvent(OnClickEvent);

    }


    async void GetImage()
    {
        itemOjbect = await ResourceManager.Instance.LoadAsset<ItemObject>(data.rCode.ToLower() , eAddressableType.prefab);
        mainimage = itemOjbect.itemObjectData.image;
        ItemImage.sprite = mainimage;
    }
    public void SetData(ItemData _data)
    {
        data = _data;
    }

    protected virtual void OnClickEvent(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemOjbect.itemObjectData == null)
            return;

        UI_ItemToolTip.instance.ShowToolTip(itemOjbect.itemObjectData);
        //TODO 위치잡아주는것
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemOjbect.itemObjectData == null)
            return;

        UI_ItemToolTip.instance.HideToolTip();
    }
}

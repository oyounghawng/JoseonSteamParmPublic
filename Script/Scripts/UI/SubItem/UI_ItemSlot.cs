using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_ItemSlot : UI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected int index;
    protected UI_DragSlot instance;

    public Image myImage;
    public GameObject block;
    public TextMeshProUGUI countText;
    public ItemObjectData[] slotsData;
    public ItemObjectData slotItemData;
    private bool isAvailable;
    enum Images
    {
        testImage,
        Block,
    }
    enum Texts
    {
        countText,
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        instance = UI_DragSlot.instance;
        myImage = GetImage((int)Images.testImage);
        block = GetImage((int)Images.Block).gameObject;
        countText = GetText((int)Texts.countText);
        countText.text = string.Empty;
        Set();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (block.activeSelf)
            return;

        if (instance == null)
            return;


        if (myImage.sprite != null)
        {
            instance.itemSlot = this;
            instance.SetDragSlot(myImage, 1);
            instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (block.activeSelf)
            return;

        if (instance == null)
            return;

        if (myImage.sprite != null)
        {
            instance.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (block.activeSelf)
            return;

        if (instance.itemSlot != null)
        {
            ChangeSlot();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (block.activeSelf)
            return;

        if (instance == null)
            return;

        instance.ResetDragSlot();
        instance.itemSlot = null;
    }
    protected virtual void ChangeSlot()
    {
        // 현재 슬롯에 아이템이 있는지 체크
        if (this.slotItemData != null)
        {
            if (instance.itemSlot.slotItemData.itemKey.Equals(this.slotItemData.itemKey) && instance.itemSlot.slotItemData.itemData.canStack)
            {
                // 아이템이 같은 경우 합치기
                slotItemData.count += instance.itemSlot.slotItemData.count;
                instance.itemSlot.slotsData[instance.itemSlot.index] = null;
            }
            else
            {
                instance.itemSlot.slotsData.ArraySwap(slotsData, instance.itemSlot.index, index);
            }
        }
        else
        {
            instance.itemSlot.slotsData.ArraySwap(slotsData, instance.itemSlot.index, index);
        }
        instance.itemSlot.Set();
        Set();
        (SceneManagerEx.Instance.CurrentScene as GameScene).player.CallInventoryReset();
    }
    public void Set()
    {
        if (isAvailable)
        {
            if (block.activeSelf)
                block.SetActive(false);

            if (slotsData[index] != null)
            {
                slotItemData = slotsData[index];
                myImage.sprite = slotItemData.image;
                myImage.color = Util.SetColorAlpha(myImage, 1);
                countText.text = slotItemData.count > 1 ? slotItemData.count.ToString() : string.Empty;
            }
            else
            {
                slotItemData = null;
                myImage.sprite = null;
                myImage.color = Util.SetColorAlpha(myImage, 0);
                countText.text = string.Empty;
            }
        }
        else
        {
            slotItemData = null;
        }
    }

    public void SetIndex(int _index, bool _isAvailable = true, ItemObjectData[] _slotsData = null)
    {
        index = _index;
        slotsData = _slotsData;
        isAvailable = _isAvailable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotItemData == null)
            return;

        UI_ItemToolTip.instance.ShowToolTip(slotItemData);
        //TODO 위치잡아주는것
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotItemData == null)
            return;

        UI_ItemToolTip.instance.HideToolTip();
    }
}

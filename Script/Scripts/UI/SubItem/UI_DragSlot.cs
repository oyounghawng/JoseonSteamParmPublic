using UnityEngine.UI;

public class UI_DragSlot : UI_Base
{
    public static UI_DragSlot instance;
    public UI_ItemSlot itemSlot;
    public Image dragImage;
    enum Images
    {
        testImage,
    }
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        instance = this;
        dragImage = GetImage((int)Images.testImage);
        ResetDragSlot();
    }
    public void ResetDragSlot()
    {
        dragImage.color = Util.SetColorAlpha(dragImage, 0);
        dragImage.sprite = null;
    }

    public void SetDragSlot(Image _image, float alpha)
    {
        dragImage.sprite = _image.sprite;
        dragImage.color = Util.SetColorAlpha(dragImage, alpha);
    }
}

using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    public ItemObjectData[] itemList = new ItemObjectData[30];

    private void Start()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            itemList[i] = null;
        }
    }
    public void Interact()
    {
        LoadPrefab();
    }
    private async void LoadPrefab()
    {
        UI_Box ui_box = await UIManager.Instance.ShowTaskPopupUI<UI_Box>();
        ui_box.SetBoxItemList(itemList);
    }
}

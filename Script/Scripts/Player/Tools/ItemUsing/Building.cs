using System;
using UnityEngine;

public class Building : MonoBehaviour, ITool
{
    public GameObject preBuildprefab;
    private GameObject go;
    private ItemObjectData itemObjectData;
    public void SetItemObjectData(ItemObjectData _itemObjectData)
    {
        itemObjectData = _itemObjectData;
    }


    private void OnEnable()
    {
        if (go == null)
        {
            go = ResourceManager.Instance.Instantiate(preBuildprefab);
        }
        else
        {
            go.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (go != null)
            go.SetActive(false);
    }

    public bool Use()
    {
        Build();
        return true;
    }

    public void UseAnimation(PlayerAnimationController animationController)
    {

    }

    public void Subscribe(Action<bool> callback)
    {

    }

    public async void Build()
    {
        preBuild preBuild = go.GetComponent<preBuild>();
        if (!preBuild.CanBuild())
            return;

        GameObject buildPrefab = Instantiate(await ResourceManager.Instance.LoadAsset<GameObject>(itemObjectData.itemData.rCode.ToLower(), eAddressableType.prefab));
        buildPrefab.transform.position = preBuild.GetBuildPosition();
        (Managers.Scene.CurrentScene as GameScene).player.RemoveItem(itemObjectData);
    }
}

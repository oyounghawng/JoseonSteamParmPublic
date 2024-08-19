using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Tree,
    Stone
}
public class ResourcesItem : MonoBehaviour
{
    [SerializeField] List<GameObject> dropItems = new List<GameObject>();
    public ResourceType type;
    public void ItemDrop()
    {
        int randomIndex = Random.Range(0, dropItems.Count);
        GameObject go = Managers.Resource.Instantiate(dropItems[randomIndex]);
        go.transform.position = this.transform.position;
        Managers.Resource.Destroy(this.gameObject);
    }
}

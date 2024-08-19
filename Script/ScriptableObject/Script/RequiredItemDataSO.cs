using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RequiredItem", menuName = "ItemSlot/RequiredItem", order = 0)]
public class RequiredItemDataSO : ScriptableObject
{
    public Define.ItemType type;
    public Sprite image;
    public ItemObject requiredItem;
    public int reuqiredItemStack;
}
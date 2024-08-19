using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : MonoBehaviour
{
    public ItemObjectData[] playerInventory = new ItemObjectData[30];
    private int playerInventoryLevel = 0;
    public int PlayerInventoryLevel
    {
        get
        {
           return playerInventoryLevel;
        }
        set
        {
            playerInventoryLevel = Mathf.Clamp(playerInventoryLevel + value, 0, 2);
        }
    }

    public GameObject curEquip;
    public PlayerInputController inputController;
    public PlayerAnimationController animationController;

    private Dictionary<string, GameObject> equipList = new Dictionary<string, GameObject>();
    private GameObject playerLight;
    private GameObject equipItem;
    private SpriteRenderer headSpriteRenderer;

    public Action resetSlotsUI;
    public Action resetPlayerEquipItem;

    public const string Seeding = "Seeding";
    public const string Building = "Building";

    private void Awake()
    {
        equipItem = Util.FindChild(this.gameObject, "EquipItem", true);
        headSpriteRenderer = equipItem.GetComponent<SpriteRenderer>();
        playerLight = Util.FindChild(this.gameObject, "Light", true);
        headSpriteRenderer.sprite = null;

        for (int i = 0; i < playerInventory.Length; i++)
        {
            playerInventory[i] = null;
        }

        GameObject go = Util.FindChild(this.gameObject, "Tool", true);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject tool = go.transform.GetChild(i).gameObject;
            if (tool.TryGetComponent(out ItemObject data))
            {
                equipList.Add(data.itemObjectData.itemKey, tool);
                playerInventory[i] = data.itemObjectData;
            }
            else
            {
                equipList.Add(tool.name, tool);
            }
            tool.SetActive(false);
        }
    }

    private void OnEnable()
    {
        TimeManager.Instance.onChangedTime -= TurnOnLight;
        TimeManager.Instance.onChangedTime += TurnOnLight;
        TimeManager.Instance.onStartDay -= Respawn;
        TimeManager.Instance.onStartDay += Respawn;

        resetPlayerEquipItem += UnEquipItem;
    }

    public bool HasPlayerItem(ItemObjectData itemObjectData)
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i] == null)
                continue;

            if (playerInventory[i].itemData.displayName.Equals(itemObjectData.itemData.displayName))
            {
                return true;
            }
        }
        return false;
    }
    public bool HasPlyerItem(ItemType type)
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i] == null)
                continue;

            if (playerInventory[i].itemData.type == (int)type)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(ItemObjectData itemObjectData)
    {
        if (itemObjectData.itemData.canStack)
        {
            //중복체크
            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i] == null)
                    continue;

                if (playerInventory[i].itemData.displayName.Equals(itemObjectData.itemData.displayName))
                {
                    playerInventory[i].count++;
                    resetSlotsUI?.Invoke();
                    return;
                }
            }
        }
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i] == null)
            {
                ItemObjectData temp = new ItemObjectData(itemObjectData);
                playerInventory[i] = temp;
                resetSlotsUI?.Invoke();
                return;
            }
        }
    }

    public void RemoveItem(ItemObjectData itemObjectData)
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i] == null)
                continue;

            if (playerInventory[i].itemData.displayName.Equals(itemObjectData.itemData.displayName))
            {
                playerInventory[i].count--;
                if (playerInventory[i].count <= 0)
                {
                    playerInventory[i] = null;
                    resetPlayerEquipItem?.Invoke();
                }
                resetSlotsUI?.Invoke();
                return;
            }
        }
        return;
    }

    private void UnEquipItem()
    {
        if (curEquip == null)
            return;

        headSpriteRenderer.sprite = null;
        curEquip.SetActive(false);
        curEquip = null;
    }

    public bool SwitchEquip(int idx)
    {
        if (playerInventory[idx] == null)
            return false;

        // 이미 장착된 장비와 동일한지 확인
        if (curEquip != null && playerInventory[idx].itemKey == curEquip.name)
        {

            UnEquipItem();
            return true;  // 동일한 장비였으므로 장착 해제 후 종료
        }

        UnEquipItem();

        if (playerInventory[idx].itemData.type == (int)ItemType.ToolItem)
        {
            equipList.TryGetValue(playerInventory[idx].itemKey, out curEquip);
            curEquip.SetActive(true);
        }
        else
        {
            headSpriteRenderer.sprite = playerInventory[idx].image;

            if (playerInventory[idx].itemData.type == (int)ItemType.FarmItem && playerInventory[idx].itemData.displayName.Contains("씨앗"))
            {
                equipList.TryGetValue(Seeding, out curEquip);
                curEquip.SetActive(true);
                curEquip.GetComponent<Seeding>().SetItemObjectData(playerInventory[idx]);

            }
            else if (playerInventory[idx].itemData.type == (int)ItemType.BuildItem)
            {
                equipList.TryGetValue(Building, out curEquip);
                curEquip.SetActive(true);
                curEquip.GetComponent<Building>().SetItemObjectData(playerInventory[idx]);
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            AddItem(collision.gameObject.GetComponent<ItemObject>().itemObjectData);
            ResourceManager.Instance.Destroy(collision.gameObject);
        }
    }

    private void TurnOnLight(int time)
    {
        if (time < 1080)
            playerLight.SetActive(false);
        else
            playerLight.SetActive(true);
    }

    private void Respawn()
    {
        transform.position = (Managers.Scene.CurrentScene as GameScene).respawn.position;
    }

    public void CallInventoryReset()
    {
        resetPlayerEquipItem?.Invoke();
        resetSlotsUI?.Invoke();
    }
}

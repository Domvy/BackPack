using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, Inventory
{
    [SerializeField] int maxSlot; // 최대 슬롯
    public int MaxSlot { get => maxSlot; set => maxSlot = value; }
    [SerializeField] int slotCount; // 현재 채워진 슬롯
    public int SlotCount { get => slotCount; set => slotCount = value; }
    bool[,] slotBase; // 채워진 슬롯 위치
    public bool[,] SlotBase { get => slotBase; set => slotBase = value; }
    SerializableDictionary<string, List<Item>> itemTable = new SerializableDictionary<string, List<Item>>(); // 아이템 리스트
    [SerializeField] SerializableDictionary<string, int> itemTableCount = new SerializableDictionary<string, int>(); // 아이템 숫자
    public SerializableDictionary<string, List<Item>> ItemTable { get => itemTable; set => itemTable = value; }
    Dictionary<string, List<Vector3>> itemPosition = new Dictionary<string, List<Vector3>>(); // 아이템 위치

    [SerializeField] GameObject slotListObj;
    [SerializeField] List<GameObject> slot;

    private void Awake()
    {
        SlotSetting();
    }
    private void Start()
    {

    }
    private void Update()
    {

    }

    private void SlotSetting() // 슬롯 리스트 받아오기 & 슬롯 배열 세팅
    {
        for (int i = 0; i < slotListObj.transform.childCount; i++)
        { slot.Add(slotListObj.transform.GetChild(i).gameObject); }
        int countA = slotListObj.GetComponent<GridLayoutGroup>().constraintCount;
        int countB = slot.Count % countA == 0 ? slot.Count / countA : (slot.Count / countA) + 1;
        SlotBase = new bool[countB, countA];
    }
    public bool ItemDrop(Item item, GameObject itemObj) // 슬롯에 아이템 배치하기
    {
        Debug.Log("Item Dropped!");
        GameObject closestSlot = null;
        float distanceA;
        float distanceB;
        foreach (GameObject s in slot) // 가장 가까운 슬롯 구하기
        {
            closestSlot = closestSlot == null ? s : closestSlot;
            distanceA = Vector3.Distance(itemObj.transform.position, closestSlot.transform.position);
            distanceB = Vector3.Distance(itemObj.transform.position, s.transform.position);
            if (closestSlot == null || distanceB < distanceA) { closestSlot = s; }
        }
        bool result = CheckSlot(item, closestSlot);
        if (result)
        {
            SetItem(item, closestSlot);
            return true;
        }
        else
            return false;
    }
    private bool CheckSlot(Item item, GameObject closestSlot) // 배치할 슬롯 상태 확인 및 버튼 스프라이트 변경
    {
        Debug.Log("CheckSlot!");
        int row = item.Row;
        int column = item.Column;
        bool[,] size = item.Slot;
        int idx = closestSlot.transform.GetSiblingIndex();
        List<GameObject> checkedSlot = new List<GameObject>();

        if (1 < row || 1 < column)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (size[i, j])
                    {
                        int num = idx + (i * slotListObj.GetComponent<GridLayoutGroup>().constraintCount) + j;
                        if (num < slot.Count && (idx % 4) + (column - 1) < slotListObj.GetComponent<GridLayoutGroup>().constraintCount)
                        {
                            if (!item.TakeSlot && !slot[num].GetComponent<Button>().interactable)
                                return false;
                            else
                                checkedSlot.Add(slot[num]);
                        }
                        else
                        {
                            Debug.Log("CheckSlot False!");
                            Debug.Log(closestSlot.name);
                            return false;
                        }
                    }
                }
            }
            foreach (GameObject slot in checkedSlot)
            { slot.GetComponent<Button>().interactable = item.TakeSlot == false ? false : true; }
        }
        else
        {
            if (!item.TakeSlot && !closestSlot.GetComponent<Button>().interactable)
            {
                return false;
            }
            closestSlot.GetComponent<Button>().interactable = item.TakeSlot == false ? false : true;
        }
        return true;
    }
    private void SetItem(Item item, GameObject closestSlot) // 확인한 아이템 배치
    {
        Debug.Log("SetItem!");
        item.OwnPos = closestSlot.transform.position;
        item.Inventory = this;
    }
    private void OnTriggerStay2D(Collider2D collision) // 콜라이더 충돌 시 정보 가져오기
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && !item.Drag && !item.TakeSlot)
        {
            AddItem(item, collision.gameObject);
        }
    }
    public void AddItem(Item item, GameObject itemObj) // 아이템 추가
    {
        Debug.Log("AddItem!");
        if (ItemDrop(item, itemObj))
        {
            item.TakeSlot = true;

            if (!ItemTable.ContainsKey(item.Name))
            {
                ItemTable.Add(item.Name, new List<Item>());
                itemPosition.Add(item.Name, new List<Vector3>());
                itemTableCount.Add(item.Name, 0);                
            }
            ItemTable[item.Name].Add(item);
            itemPosition[item.Name].Add(item.OwnPos);
            itemTableCount[item.Name] += 1;

            itemObj.transform.parent = this.transform.Find("Item");
            SaveItem();
        }
    }

    public void BuyItem() // 아이템 구매
    {
        throw new System.NotImplementedException();
    }

    public void DeleteItem(Item item, GameObject itemObj) // 아이템 삭제
    {
        Debug.Log("Item Delete!");
        ItemDrop(item, itemObj);
        item.TakeSlot = false;
        ItemTable[item.Name].Remove(item);
        itemPosition[item.Name].Remove(item.OwnPos);
        itemTableCount[item.Name] -= 1;
        if (itemTableCount[item.Name] == 0)
        {
            itemTable.Remove(item.Name);
            itemPosition.Remove(item.Name);
            itemTableCount.Remove(item.Name);
        }
        itemObj.transform.parent = GameObject.Find("Canvas").transform;
        SaveItem();
    }

    public void ItemCombination()
    {
        throw new System.NotImplementedException();
    }
    public void SaveItem()
    {
        GameManager.GetInstance().Inventory.SaveData(ItemTable,itemTableCount,itemPosition);
    }
    public void LoadItem()
    {
        throw new System.NotImplementedException();
    }

    public void SearchItem()
    {
        throw new System.NotImplementedException();
    }

    public void SellItem()
    {
        throw new System.NotImplementedException();
    }
}
#region Dictionary 직렬화
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();


    [SerializeField]
    private List<TValue> values = new List<TValue>();


    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }


    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();


        if (keys.Count != values.Count)
            throw new Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");


        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
#endregion
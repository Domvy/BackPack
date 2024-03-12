using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Stone : MonoBehaviour, IPointerClickHandler, Item
{
    [Header("이름")]
    [SerializeField] string itemName;
    public string Name { get => itemName; set => itemName = value; }
    [Header("칸 갯수")]
    [Tooltip("총 크기")]
    [SerializeField] 
    int capacity;
    public int Capacity { get => capacity; set => capacity = value; }
    [Tooltip("행")]
    [SerializeField]
    int row; // 행
    public int Row { get => row; set => row = value; }
    [Tooltip("열")]
    [SerializeField]
    int column; // 열
    public int Column { get => column; set => column = value; }
    [SerializeField]
    bool[,] slot;
    [Tooltip("아이템 모양")]
    [SerializeField]
    SlotArray[] slotArray;
    public SlotArray[] SlotArray { get => slotArray; set => slotArray = value; }
    public bool[,] Slot { get => slot; set => slot = value; }
    [Header("기본 스텟")]
    [SerializeField] float damage;
    [SerializeField] float armor;
    [SerializeField] float speed;
    public float Damage { get => damage; set => damage = value; }
    public float Armor { get => armor; set => armor = value; }
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] bool drag;
    public bool Drag { get { return drag; } set { drag = value; } }
    [SerializeField]
    bool takeSlot;
    public bool TakeSlot { get { return takeSlot; } set { takeSlot = value; } }
    public PlayerInventory Inventory { get; set; }
    [SerializeField]
    Vector3 ownPos = new Vector3();
    public Vector3 OwnPos { get { return ownPos; } set { ownPos = value; } }


    private void Awake()
    {
        OwnPos = transform.position;        
        ItemSize();
    }
    private void Update()
    {
        if (drag)
        {
            Vector2 point = Input.mousePosition;
            this.gameObject.transform.position = point;
        }
        else if (!drag)
        {
            StartCoroutine(ItemPosition());
        }
    }

    public void ActiveItem()
    {
        throw new System.NotImplementedException();
    }

    public void DestoyItem()
    {
        Destroy(this);
    }

    public void ItemSize() // 아이템 모양 정하기
    {
        int capacityNum = 0;
        if (slotArray.Length != 0)
        {
            row = slotArray.Length;
            column = slotArray[0].m_slotArray.Length;            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (slotArray[i].m_slotArray[j])
                    {
                        capacityNum++;
                    }
                }
            }
            Capacity = capacityNum;
        }
        
        Slot = new bool[row, column];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Slot[i,j] = slotArray[i].m_slotArray[j];
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData) // 마우스 클릭
    {
        drag = drag == false ? true : false;
        if (Inventory != null && TakeSlot) { Inventory.DeleteItem(this, this.gameObject); }
    }
    IEnumerator ItemPosition()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = OwnPos;
    }
}
[Serializable]
public struct SlotArray
{
    [SerializeField]
    public bool[] m_slotArray;    
}
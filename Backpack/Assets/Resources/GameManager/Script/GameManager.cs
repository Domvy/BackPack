using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Loading;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    #region ���ӸŴ��� �̱���
    static GameManager instance;
    public static GameManager GetInstance() { return instance; } // ���� ��� GetInstance()

    [SerializeField]
    StageList stageList; // �� �̸��� ��� ������
    public InventoryData Inventory { get; set; } = new InventoryData();
    public StageList GetStageList() { return stageList; }
    [SerializeField] MapCreateManager mapCreater;
    Text systemText; // ��� �ؽ�Ʈ  

    [Header("���� �ӵ�")]
    [Range(0, 2)]
    [SerializeField] float gameSpeed = 1f;
    public float GameSpeed
    {
        get { return gameSpeed; }
        set
        {
            gameSpeed = value;
            OnGameSpeedChaged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler OnGameSpeedChaged;
    [Header("�÷��̾� ������Ʈ")]
    [SerializeField] GameObject player;
    public GameObject Player { get { return player; } set { player = value; } }

    #region ���� ������
    int stage = 1; // ��������
    public int Stage { get { return stage; } set { stage = value; } }
    int category = 0; // �� ����
    public int Category { get { return category; } set { category = value; } }
    int difficulty = 0; // ���̵�
    public int Difficulty { get { return difficulty; } set { difficulty = value; } }
    #endregion

    void Awake() // �̱��� �Ҵ�
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(instance);
        OnGameSpeedChaged += GameSpeedChange;
        OnGameSpeedChaged.Invoke(this, EventArgs.Empty);
    }
    //private void Start()
    //{
    //    itemTable = new Dictionary<string, Item>();
    //}
    private void LateUpdate()
    {

    }
    #endregion
    #region ���    
    public void SceneProgress(string sceneName) // ���� ���� ������
    {
        switch (sceneName)
        {
            case "MainMenu":
                LoadingSceneManager.LoadingScene(stageList.MainMenu);
                break;
            case "CharacterSelect":
                LoadingSceneManager.LoadingScene(stageList.CharacterSelect);
                break;
            case "Shop":
                LoadingSceneManager.LoadingScene(stageList.Shop);
                break;
            case "Event":
                LoadingSceneManager.LoadingScene(stageList.Event);
                break;
            case "Lobby":
                LoadingSceneManager.LoadingScene(stageList.Lobby);
                break;
            case "Battle":
                if (stage == 1)
                {
                    if (difficulty == 1)
                        LoadingSceneManager.LoadingScene(stageList.FirstNormalBattle);
                    if (difficulty == 2)
                        LoadingSceneManager.LoadingScene(stageList.FirstEliteBattle);
                    if (difficulty == 3)
                        LoadingSceneManager.LoadingScene(stageList.FirstBossBattle);
                }
                else if (stage == 2)
                {
                    if (difficulty == 1)
                        LoadingSceneManager.LoadingScene(stageList.SecondNormalBattle);
                    if (difficulty == 2)
                        LoadingSceneManager.LoadingScene(stageList.SecondEliteBattle);
                    if (difficulty == 3)
                        LoadingSceneManager.LoadingScene(stageList.SecondBossBattle);
                }
                else if (stage == 3)
                {
                    if (difficulty == 1)
                        LoadingSceneManager.LoadingScene(stageList.ThirdNormalBattle);
                    if (difficulty == 2)
                        LoadingSceneManager.LoadingScene(stageList.ThirdEliteBattle);
                    if (difficulty == 3)
                        LoadingSceneManager.LoadingScene(stageList.ThirdBossBattle);
                }
                break;
        }
    }
    public bool SetInformation(string Input) // ���� ������ �ֽ�ȭ
    {
        switch (Input)
        {
            case ("PlayerUpdate"): // �÷��̾� ���� �ֽ�ȭ
                break;
            case ("ItemUpdate"): // ������ ����Ʈ �ֽ�ȭ
                break;
            case ("StageClear"): // �������� Ŭ����
                mapCreater.MapCreated = false;
                stage++;
                break;
        }
        return true;
    }
    public void GameOver() { } // ���� ����
    public void ReStart() { } // �����
    public void TextPrint(Text systemText) // �ý��� �ؽ�Ʈ ���
    {
        Console.WriteLine(systemText);
    }
    #endregion
    #region �̺�Ʈ
    private void GameSpeedChange(object sender, EventArgs e)
    {
        Time.timeScale = GameSpeed;
        Debug.Log("GameSpeed Chaged!");
        Debug.Log(GameSpeed);
    }
    #endregion
}
public class InventoryData
{    
    public SerializableDictionary<string, List<Item>> ItemTable { get; set; }
    public SerializableDictionary<string, int> ItemTableCount { get; set; }
    public Dictionary<string, List<Vector3>> ItemPosition { get; set; }

    public void SaveData(SerializableDictionary<string, List<Item>> itemTable, SerializableDictionary<string, int> itemTableCount, Dictionary<string, List<Vector3>> itemPosition)
    {
        ItemTable = itemTable;
        ItemTableCount = itemTableCount;
        ItemPosition = itemPosition;
    }
}
#region �������̽�(Interface)
public interface Save // ����
{
    void UpdateData(); // ������ �ֽ�ȭ
    void SaveData(); // ������ ����
    void DeleteData(); // ������ ����
}
public interface Character // ĳ����
{
    string name { get; set; } // ĳ���� �̸�
    void Ability(); // ĳ���� Ư��
}
public interface Item // ���� ������
{
    string Name { get; set; } // ������ �̸�
    int Capacity { get; set; } // �����ϴ� ���� ����
    int Row { get; set; } // ��
    int Column { get; set; } // ��
    bool[,] Slot { get; set; } // ������ ���� ����
    SlotArray[] SlotArray { get; set; } // ������ ���� ����(�ν�����)
    float Damage { get; set; } // ��
    float Armor { get; set; } // ��
    float Speed { get; set; } // ��
    bool Drag { get; set; } // ������ ��ȣ�ۿ� ����
    bool TakeSlot { get; set; } // ������ ��ġ ����
    PlayerInventory Inventory { get; set; } // �κ��丮 �Լ� ȣ���
    Vector3 OwnPos { get; set; } // ������ ��ġ
    void ItemSize(); // �������� ���(����)
    void ActiveItem(); // ������ ȿ��
    void DestoyItem(); // ������ ����
    void OnPointerClick(PointerEventData eventData); // ������ Ŭ��
}
public interface Inventory // �κ��丮
{
    int MaxSlot { get; set; } // ���� �ִ�ũ��
    int SlotCount { get; set; } // ���� ���� ũ��
    bool[,] SlotBase { get; set; } // ���� ���� ����
    SerializableDictionary<string, List<Item>> ItemTable { get; set; } // ������ ������
    void ItemInformation(Dictionary<string, Item> itemTable, bool[,] slotBase) { } // ���� ������ ������ ��ġ
    void SaveItem(); // �κ��丮 ������ ���� ����
    void LoadItem(); // �κ��丮�� ������ �ҷ�����
    void DeleteItem(Item value, GameObject valueObj); // �������� �ʴ� ������ ����
    void SearchItem(); // ������ ������ �Ǻ�
    void AddItem(Item value, GameObject valueObj); // ������ �߰�
    void SellItem(); // ������ �ȱ�
    void BuyItem(); // ������ ���
    void ItemCombination(); // ������ ���� �ó���(+ ��ġ�� ���� ȿ��)
}
#endregion

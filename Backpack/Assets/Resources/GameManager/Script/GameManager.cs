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
    #region 게임매니저 싱글톤
    static GameManager instance;
    public static GameManager GetInstance() { return instance; } // 접근 방법 GetInstance()

    [SerializeField]
    StageList stageList; // 씬 이름이 담긴 데이터
    public InventoryData Inventory { get; set; } = new InventoryData();
    public StageList GetStageList() { return stageList; }
    [SerializeField] MapCreateManager mapCreater;
    Text systemText; // 출력 텍스트  

    [Header("게임 속도")]
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
    [Header("플레이어 오브젝트")]
    [SerializeField] GameObject player;
    public GameObject Player { get { return player; } set { player = value; } }

    #region 진행 데이터
    int stage = 1; // 스테이지
    public int Stage { get { return stage; } set { stage = value; } }
    int category = 0; // 방 유형
    public int Category { get { return category; } set { category = value; } }
    int difficulty = 0; // 난이도
    public int Difficulty { get { return difficulty; } set { difficulty = value; } }
    #endregion

    void Awake() // 싱글톤 할당
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
    #region 기능    
    public void SceneProgress(string sceneName) // 게임 진행 데이터
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
    public bool SetInformation(string Input) // 게임 데이터 최신화
    {
        switch (Input)
        {
            case ("PlayerUpdate"): // 플레이어 정보 최신화
                break;
            case ("ItemUpdate"): // 아이템 리스트 최신화
                break;
            case ("StageClear"): // 스테이지 클리어
                mapCreater.MapCreated = false;
                stage++;
                break;
        }
        return true;
    }
    public void GameOver() { } // 게임 오버
    public void ReStart() { } // 재시작
    public void TextPrint(Text systemText) // 시스템 텍스트 출력
    {
        Console.WriteLine(systemText);
    }
    #endregion
    #region 이벤트
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
#region 인터페이스(Interface)
public interface Save // 저장
{
    void UpdateData(); // 데이터 최신화
    void SaveData(); // 데이터 저장
    void DeleteData(); // 데이터 삭제
}
public interface Character // 캐릭터
{
    string name { get; set; } // 캐릭터 이름
    void Ability(); // 캐릭터 특성
}
public interface Item // 게임 아이템
{
    string Name { get; set; } // 아이템 이름
    int Capacity { get; set; } // 차지하는 슬롯 갯수
    int Row { get; set; } // 행
    int Column { get; set; } // 열
    bool[,] Slot { get; set; } // 아이템 슬롯 형태
    SlotArray[] SlotArray { get; set; } // 아이템 슬롯 형태(인스펙터)
    float Damage { get; set; } // 공
    float Armor { get; set; } // 방
    float Speed { get; set; } // 속
    bool Drag { get; set; } // 아이템 상호작용 변수
    bool TakeSlot { get; set; } // 아이템 배치 변수
    PlayerInventory Inventory { get; set; } // 인벤토리 함수 호출용
    Vector3 OwnPos { get; set; } // 아이템 위치
    void ItemSize(); // 아이템의 모양(부피)
    void ActiveItem(); // 아이템 효과
    void DestoyItem(); // 아이템 제거
    void OnPointerClick(PointerEventData eventData); // 아이템 클릭
}
public interface Inventory // 인벤토리
{
    int MaxSlot { get; set; } // 슬롯 최대크기
    int SlotCount { get; set; } // 현재 슬롯 크기
    bool[,] SlotBase { get; set; } // 현재 슬롯 형태
    SerializableDictionary<string, List<Item>> ItemTable { get; set; } // 아이템 데이터
    void ItemInformation(Dictionary<string, Item> itemTable, bool[,] slotBase) { } // 현재 아이템 정보와 배치
    void SaveItem(); // 인벤토리 아이템 정보 저장
    void LoadItem(); // 인벤토리로 아이템 불러오기
    void DeleteItem(Item value, GameObject valueObj); // 존재하지 않는 아이템 제거
    void SearchItem(); // 아이템 데이터 판별
    void AddItem(Item value, GameObject valueObj); // 아이템 추가
    void SellItem(); // 아이템 팔기
    void BuyItem(); // 아이템 사기
    void ItemCombination(); // 아이템 조합 시너지(+ 위치에 따른 효과)
}
#endregion

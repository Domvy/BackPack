using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject player; // 플레이어
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>(); // 적 종류
    List<GameObject> enemyPos = new List<GameObject>(); // 적이 생성될 위치 리스트
    List<GameObject> createdMonster = new List<GameObject>(); // 생성된 몬스터
    Transform playerPos; // 플레이어 생성 위치
    int listCount = 0; // 리스트 숫자
    int monsterCount = 0; // 생성 숫자
    int monsterPosCount = 0; // 몬스터 자리 숫자
    bool chaos; // 생성 몬스터 무작위 OX
    [Header("생성될 적 갯수")]
    [SerializeField]
    [Range(1, 5)]
    int countSlider = 1;
    int min;
    int max;

    private void Awake()
    {
        player = GameManager.GetInstance().Player;
        playerPos = GameObject.FindWithTag("PlayerPos").transform;
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("EnemyPos");
        enemyPos = enemyArray.ToList();
        enemyPos = enemyPos.OrderBy(obj => obj.transform.GetSiblingIndex()).ToList();
        min = 1;
        max = countSlider;
    }
    private void Start()
    {
        listCount = monsterList.Count;
        monsterCount = UnityEngine.Random.Range(min, max + 1);
        chaos = (UnityEngine.Random.value >= 0.5f);
        Instantiate(player, playerPos.position, Quaternion.identity, playerPos); // 플레이어 생성
        CreateMonster();
        StartBattle();
    }
    private void Update()
    {
        for (int i = 0; i < createdMonster.Count; i++) // 몬스터 이동
        {
            if (createdMonster[i] != null)
            { continue; }
            else
            {
                createdMonster.RemoveAt(i);
                MoveMonster();
            }
        }
    }
    private void FixedUpdate() // 전투 연산
    {
        
    }
    private void LateUpdate() // 카메라 이동
    {
        
    }
    private void CreateMonster() // 몬스터 생성
    {
        monsterPosCount = monsterCount;
        Transform createPos = enemyPos[monsterCount - 1].transform;
        GameObject monster = monsterList[UnityEngine.Random.Range(0, listCount)];
        for (int i = 0; i < monsterCount; i++)
        {
            if (chaos && i != 0)
            { monster = monsterList[UnityEngine.Random.Range(0, listCount)]; }
            Instantiate(monster, createPos.GetChild(i).position, Quaternion.identity, createPos.GetChild(i));
            createdMonster.Add(createPos.GetChild(i).GetChild(0).gameObject);
        }
    }
    private void MoveMonster() // 몬스터 숫자 변동 시 자리이동 및 하이라키 위치 변경
    {
        monsterPosCount = createdMonster.Count;
        Transform createPos = enemyPos[monsterPosCount - 1].transform;
        for (int i = 0; i < monsterPosCount; i++)
        {
            createdMonster[i].transform.parent = createPos.GetChild(i);
            StartCoroutine(Move(createdMonster[i], createPos.GetChild(i).position, Time.deltaTime));
        }
    }
    public void DeleteMonster() // 몬스터 삭제
    {
        for (int i = 0; i < createdMonster.Count; i++)
        { Destroy(createdMonster[i].gameObject); }
        createdMonster.Clear();
    }
    private void StartBattle() // 전투 시작
    {

    }
    #region 코루틴
    public IEnumerator Move(GameObject monster, Vector3 destination, float speed) // 몬스터 자리이동 코루틴
    {
        while (monster.transform.position != destination)
        {
            monster.transform.position = Vector3.Lerp(monster.transform.position, destination, speed);
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
}

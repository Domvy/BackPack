using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject player; // �÷��̾�
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>(); // �� ����
    List<GameObject> enemyPos = new List<GameObject>(); // ���� ������ ��ġ ����Ʈ
    List<GameObject> createdMonster = new List<GameObject>(); // ������ ����
    Transform playerPos; // �÷��̾� ���� ��ġ
    int listCount = 0; // ����Ʈ ����
    int monsterCount = 0; // ���� ����
    int monsterPosCount = 0; // ���� �ڸ� ����
    bool chaos; // ���� ���� ������ OX
    [Header("������ �� ����")]
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
        Instantiate(player, playerPos.position, Quaternion.identity, playerPos); // �÷��̾� ����
        CreateMonster();
        StartBattle();
    }
    private void Update()
    {
        for (int i = 0; i < createdMonster.Count; i++) // ���� �̵�
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
    private void FixedUpdate() // ���� ����
    {
        
    }
    private void LateUpdate() // ī�޶� �̵�
    {
        
    }
    private void CreateMonster() // ���� ����
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
    private void MoveMonster() // ���� ���� ���� �� �ڸ��̵� �� ���̶�Ű ��ġ ����
    {
        monsterPosCount = createdMonster.Count;
        Transform createPos = enemyPos[monsterPosCount - 1].transform;
        for (int i = 0; i < monsterPosCount; i++)
        {
            createdMonster[i].transform.parent = createPos.GetChild(i);
            StartCoroutine(Move(createdMonster[i], createPos.GetChild(i).position, Time.deltaTime));
        }
    }
    public void DeleteMonster() // ���� ����
    {
        for (int i = 0; i < createdMonster.Count; i++)
        { Destroy(createdMonster[i].gameObject); }
        createdMonster.Clear();
    }
    private void StartBattle() // ���� ����
    {

    }
    #region �ڷ�ƾ
    public IEnumerator Move(GameObject monster, Vector3 destination, float speed) // ���� �ڸ��̵� �ڷ�ƾ
    {
        while (monster.transform.position != destination)
        {
            monster.transform.position = Vector3.Lerp(monster.transform.position, destination, speed);
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
}

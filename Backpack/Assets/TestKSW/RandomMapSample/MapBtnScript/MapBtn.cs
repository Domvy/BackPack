using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum MapType // �� ����
{
    None,
    CharacterSelect,
    Lobby,
    Shop,
    Event,
    Battle
}
public enum BattleType // ���� ���̵�
{
    None = 0,
    Normal = 1,
    Elite = 2,
    Boss = 3
}
public class MapBtn : MonoBehaviour // �� �̵� ��ư ��ũ��Ʈ
{
    [SerializeField] MapType mapType;
    [SerializeField] BattleType battleType;
    Button btn;
    void Start()
    {
        btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ButtonActive);
    }
    private void ButtonActive() // GameManager �Լ� ����
    {
        GameManager.GetInstance().Difficulty = (int)battleType;
        GameManager.GetInstance().SceneProgress(mapType.ToString());
    }
}


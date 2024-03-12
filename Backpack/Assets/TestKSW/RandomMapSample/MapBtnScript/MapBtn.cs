using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum MapType // 맵 유형
{
    None,
    CharacterSelect,
    Lobby,
    Shop,
    Event,
    Battle
}
public enum BattleType // 전투 난이도
{
    None = 0,
    Normal = 1,
    Elite = 2,
    Boss = 3
}
public class MapBtn : MonoBehaviour // 맵 이동 버튼 스크립트
{
    [SerializeField] MapType mapType;
    [SerializeField] BattleType battleType;
    Button btn;
    void Start()
    {
        btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ButtonActive);
    }
    private void ButtonActive() // GameManager 함수 실행
    {
        GameManager.GetInstance().Difficulty = (int)battleType;
        GameManager.GetInstance().SceneProgress(mapType.ToString());
    }
}


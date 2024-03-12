using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Stage List", menuName = "Scriptable Object/Stage List", order = int.MaxValue)]
public class StageList : ScriptableObject
{
    #region 씬 이름 목록
    [Header("씬 이름 입력")]

    [Header("메인 메뉴")]
    [SerializeField] SceneAsset mainMenu;
    public string MainMenu { get { return mainMenu.name.ToString(); } }
    [Header("캐릭터 선택 씬")]
    [SerializeField] SceneAsset characterSelect;
    [Header("상점 씬")]
    [SerializeField] SceneAsset shop;
    [Header("이벤트 씬")]
    [SerializeField] SceneAsset evnt;
    [Header("로비")]
    [SerializeField] SceneAsset lobby;
    public string CharacterSelect { get { return characterSelect.name.ToString(); } }
    public string Lobby { get { return lobby.name.ToString(); } }
    public string Shop { get { return shop.name.ToString(); } }
    public string Event { get { return evnt.name.ToString(); } }    

    [Header("스테이지 1 전투 씬")]
    [SerializeField] SceneAsset firstNormalBattle;
    public string FirstNormalBattle { get {  return firstNormalBattle.name.ToString(); } }
    [SerializeField] SceneAsset firstEliteBattle;
    public string FirstEliteBattle { get { return firstEliteBattle.name.ToString(); } }
    [SerializeField] SceneAsset firstBossBattle;
    public string FirstBossBattle { get { return firstBossBattle.name.ToString(); } }
    [Header("스테이지 2 전투 씬")]
    [SerializeField] SceneAsset secondNormalBattle;
    public string SecondNormalBattle { get { return secondNormalBattle.name.ToString(); } }
    [SerializeField] SceneAsset secondEliteBattle;
    public string SecondEliteBattle { get { return secondEliteBattle.name.ToString(); } }
    [SerializeField] SceneAsset secondBossBattle;
    public string SecondBossBattle { get { return secondBossBattle.name.ToString(); } }
    [Header("스테이지 3 전투 씬")]
    [SerializeField] SceneAsset thirdNormalBattle;
    public string ThirdNormalBattle { get { return thirdNormalBattle.name.ToString(); } }
    [SerializeField] SceneAsset thirdEliteBattle;
    public string ThirdEliteBattle { get { return thirdEliteBattle.name.ToString();} }
    [SerializeField] SceneAsset thirdBossBattle;
    public string ThirdBossBattle { get { return thirdBossBattle.name.ToString();} }
    #endregion
}

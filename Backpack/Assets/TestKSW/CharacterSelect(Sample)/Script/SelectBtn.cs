using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.UI;

public class SelectBtn : MonoBehaviour
{
    [Header("0 = Left Button, 1 = Right Button")]
    [SerializeField]
    GameObject[] selectButton = new GameObject[2]; // 좌우버튼
    [SerializeField] List<GameObject> characterList = new List<GameObject>(); // 캐릭터 리스트
    [SerializeField] GameObject activeBtn;
    [SerializeField] GameObject nextBtn;
    bool moving = false;
    int moveSpeed = 10;

    [System.Obsolete]
    private void Awake()
    {
        characterList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        { characterList.Add(transform.GetChild(i).gameObject); }
        selectButton[0].GetComponent<Button>().onClick.AddListener(LeftBtnClick);
        selectButton[1].GetComponent<Button>().onClick.AddListener(RightBtnClick);
        foreach (GameObject character in characterList)
        { character.active = character.transform.GetSiblingIndex() == 0 ? true : false; }
        activeBtn = characterList[0];
    }
    private void LeftBtnClick()
    {
        if (!moving)
        {
            nextBtn = activeBtn.transform.GetSiblingIndex() == 0 ?
            characterList[characterList.Count - 1] :
            characterList[activeBtn.transform.GetSiblingIndex() - 1];
            nextBtn.transform.localPosition = new Vector3(-1920, 0, 0);
            StartCoroutine(MoveUI(activeBtn, nextBtn, true));
            activeBtn = nextBtn;
            nextBtn = null;
            moving = true;
        }        
    }
    private void RightBtnClick()
    {
        if (!moving)
        {
            nextBtn = activeBtn.transform.GetSiblingIndex() == characterList.Count - 1 ?
            characterList[0] :
            characterList[activeBtn.transform.GetSiblingIndex() + 1];
            nextBtn.transform.localPosition = new Vector3(1920, 0, 0);
            StartCoroutine(MoveUI(activeBtn, nextBtn, false));
            activeBtn = nextBtn;
            nextBtn = null;
            moving = true;
        }        
    }
    public void PlayerSelect()
    {
        GameManager.GetInstance().Player = Resources.Load<GameObject>("Prefabs/Player/" + GameObject.FindWithTag("Player").name);
    }
    private IEnumerator MoveUI(GameObject activeBtn, GameObject nextBtn, bool direction)
    {
        nextBtn.SetActive(true);
        Vector3 destination1 = direction ? new Vector3(1920, 0, 0) : new Vector3(-1920, 0, 0);
        Vector3 destination2 = new Vector3(0, 0, 0);
        while (Mathf.Abs(nextBtn.transform.localPosition.x) > 1f)
        {
            activeBtn.transform.localPosition = Vector3.Lerp(activeBtn.transform.localPosition, destination1, Time.deltaTime * moveSpeed);
            nextBtn.transform.localPosition = Vector3.Lerp(nextBtn.transform.localPosition, destination2, Time.deltaTime * moveSpeed);
            yield return null;
        }
        activeBtn.SetActive(false);
        moving = false;
    }
}

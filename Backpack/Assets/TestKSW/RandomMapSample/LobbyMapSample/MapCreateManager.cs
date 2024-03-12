using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Data;
using System.Linq;

public class MapCreateManager : MonoBehaviour
{
    [Header("��ư ������")]
    [SerializeField] GameObject normalBtn;
    [SerializeField] GameObject eliteBtn;
    [SerializeField] GameObject bossBtn;
    [SerializeField] GameObject shopBtn;
    [SerializeField] GameObject eventBtn;
    [Space(10f)]
    [Header("�� ������")]
    [Range(1, 10)]
    [SerializeField] int width;
    [Range(1, 10)]
    [SerializeField] int height;
    [SerializeField] int space_X;
    [SerializeField] int space_Y;
    [Space(10f)]
    [Header("Ư�� �� ���� Ȯ��")]
    [Tooltip("��ü Ȯ��(�Ϲݸ�, Ư����)")]
    [Range(0, 100)]
    [SerializeField] int percentage;
    [Space(5f)]
    [Tooltip("���� ����(�ջ� 100)")]
    [Range(0, 100)]
    [SerializeField] int elite;
    [Tooltip("���� ����(�ջ� 100)")]
    [Range(0, 100)]
    [SerializeField] int shop;
    [Tooltip("���� ����(�ջ� 100)")]
    [Range(0, 100)]
    [SerializeField] int evnt;

    [SerializeField] bool mapCreated;
    public bool MapCreated { get { return mapCreated; } set { mapCreated = value; } }
    List<Transform> Line = new List<Transform>();
    GridLayoutGroup layout;
    int childHight;
    int childCount;

    List<int> createdBtnCount = new List<int>();
    List<GameObject> createdList = new List<GameObject>();
    List<GameObject> btnPosList = new List<GameObject>();

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �� �̺�Ʈ �߰�        
        mapCreated = false;
    }    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.GetInstance().GetStageList().Lobby == scene.name)
        {
            MakeLine(width, height);
            if (!mapCreated)
            {                
                RandomMap();
                mapCreated = true;
            }
            else
                CreateMap(createdBtnCount, createdList);
        }
    }
    private void MakeLine(int width, int height)
    {
        Line.Clear();
        Transform map = GameObject.FindWithTag("Map").transform;
        for (int i = 0; i < width; i++)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/Line"), map.position, Quaternion.identity, map);
        }
        if (map != null)
        {
            for (int i = 0; i < map.childCount; i++)
            { Line.Add(map.GetChild(i)); }
        }
    }
    private void RandomMap() // ������ ��(��ư) ����
    {        
        List<int> createBtnCount = new List<int>(); // ���κ� ������ ���� ����Ʈ
        int allBtnCount = 0; // �� ���� ����
        for (int i = 0; i < Line.Count; i++)
        {
            int randomCount = Random.Range(1, height + 1);
            if (i == 0)
            {
                createBtnCount.Add(randomCount);
            }
            else
            {
                while (createBtnCount[i - 1] * 2 < randomCount)
                {
                    randomCount = Random.Range(1, height + 1);
                }
                if (randomCount < createBtnCount[i - 1] - 2)
                { randomCount = createBtnCount[i - 1] - 2; }
                createBtnCount.Add(randomCount);
            }
        }
        createBtnCount[Line.Count - 1] = 1; // �������� �׻� �ϳ�(���� ����)
        foreach (int i in createBtnCount) // �����ջ�
        { allBtnCount += i; }

        List<GameObject> createList = new List<GameObject>(); // ���� ������ ��� ����Ʈ
        for (int i = 0; i < allBtnCount; i++) // Ȯ�� ��� �ݺ���
        {
            int dice = Random.Range(1, 101);
            if (percentage < dice) // Ư�� �� Ȯ�����
            {
                createList.Add(normalBtn);
            }
            else // Ư�� �� Ȯ�� ����(����, �̺�Ʈ)
            {
                int dice2 = Random.Range(1, 101);
                if (dice2 <= elite)
                {
                    createList.Add(eliteBtn);
                }
                else if (dice2 <= elite + shop)
                {
                    createList.Add(shopBtn);
                }
                else if (dice2 <= elite + shop + evnt)
                {
                    createList.Add(eventBtn);
                }
            }
        }
        createList = ShuffleList(createList); // ���� ������
        createList[createList.Count - 1] = bossBtn; // �������� ���� ����

        createdBtnCount = createBtnCount.ToList(); // ������ ���� ����
        createdList = createList.ToList();

        CreateMap(createBtnCount, createList); // �� ����
    }
    private void CreateMap(List<int> createNumber, List<GameObject> createObj)
    {
        int count = 0;
        for (int i = 0; i < createNumber.Count; i++) // ��ư ����
        {
            for (int j = 0; j < createNumber[i]; j++)
            {
                CreateBtn(createObj[count], Line[i]);
                count++;
            }
        }
        LayoutVerticalSort(space_Y); // ������ ��ư ����
        LayoutHorizontalSort(space_X);
        StartCoroutine(CoWaitForPosition());
    }
    private void CreateBtn(GameObject btn, Transform line) // ��ư����
    {
        Instantiate(btn, line.position, Quaternion.identity, line);
    }
    private void LayoutVerticalSort(int space) // ��ư ���� ���� (Grid Layout Group ���)
    {
        for (int i = 0; i < Line.Count; i++)
        {
            layout = Line[i].GetComponent<GridLayoutGroup>();
            if (layout == null)
            {
                Line[i].AddComponent<GridLayoutGroup>();
                layout = Line[i].GetComponent<GridLayoutGroup>();
            }
            childCount = Line[i].transform.childCount;
            childHight = (int)Line[i].GetComponent<RectTransform>().rect.height;
            layout.padding.top = -(childHight * (childCount - 1) / 2 + (space / 2) * (childCount - 1));
            layout.spacing = new Vector2(0, space);
        }
    }
    private void LayoutHorizontalSort(int space) // ��ư ���� ����
    {
        for (int i = 0; i < Line.Count; i++)
        {
            if (i <= Line.Count / 2)
            {
                Line[i].transform.localPosition = -new Vector3((Line.Count / 2 - i) * space, 0, 0);
            }
            else
            {
                Line[i].transform.localPosition = new Vector3((i - Line.Count / 2) * space, 0, 0);
            }
        }
    }
    private void CreateBridge() // ������ ��ư �� �̾����� ���� �߱�
    {
        GameObject bridge = Resources.Load<GameObject>("Prefabs/UI/Bridge");
        List<List<GameObject>> bridgeBtnList = new List<List<GameObject>>();         // ��ư ������ ���� ����Ʈ ����
        foreach (Transform obj in Line) { bridgeBtnList.Add(new List<GameObject>()); }
        int btnNumber = 0;
        for (int i = 0; i < Line.Count; i++)
        {
            for (int j = 0; j < createdBtnCount[i]; j++)
            {
                bridgeBtnList[i].Add(btnPosList[btnNumber].gameObject);                
                btnNumber++;
            }
        }
        for (int i = 0; i < bridgeBtnList.Count; i++) // ������ ��ư ����
        {
            foreach (GameObject bridgeBtnA in bridgeBtnList[i])
            {
                GameObject high, middle, low;
                high = middle = low = null;

                Vector2 A = bridgeBtnA.transform.localPosition;

                if (i == bridgeBtnList.Count - 1) { continue; }
                foreach (GameObject bridgeBtnB in bridgeBtnList[i + 1]) // ����, ����, ���� ��ư ���
                {
                    Vector2 B = bridgeBtnB.transform.localPosition;

                    if (A.y < B.y)
                    {
                        high = bridgeBtnB;
                    }
                    else if (A.y == B.y && middle == null)
                    {
                        middle = bridgeBtnB;
                    }
                    else if (B.y < A.y && low == null)
                    {
                        low = bridgeBtnB;
                    }
                }
                // ++���� �긴�� ����
                if (middle != null) // Y��ǥ�� ������ ��ư�� ���� �� 1�� ����
                {
                    // ���� ��ǥ ���ϱ�
                    float xPos = (middle.transform.position.x + bridgeBtnA.transform.position.x) / 2;
                    float yPos = (middle.transform.position.y + bridgeBtnA.transform.position.y) / 2;
                    Vector3 pos = new Vector3(xPos, yPos, 90);

                    // ���� ȸ�� �� ���ϱ�
                    xPos = middle.transform.position.x - bridgeBtnA.transform.position.x;
                    yPos = middle.transform.position.y - bridgeBtnA.transform.position.y;
                    Vector2 direction = new Vector2(xPos, yPos);
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                    // ����
                    Instantiate(bridge, pos, angleAxis, GameObject.Find("Road").transform);

                    if (bridgeBtnA == bridgeBtnList[i][0] && high != null)
                    {
                        xPos = (high.transform.position.x + bridgeBtnA.transform.position.x) / 2;
                        yPos = (high.transform.position.y + bridgeBtnA.transform.position.y) / 2;
                        Vector3 pos2 = new Vector3(xPos, yPos, 90);

                        xPos = high.transform.position.x - bridgeBtnA.transform.position.x;
                        yPos = high.transform.position.y - bridgeBtnA.transform.position.y;
                        Vector2 direction2 = new Vector2(xPos, yPos);

                        angle = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
                        Quaternion angleAxis2 = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                        Instantiate(bridge, pos2, angleAxis2, GameObject.Find("Road").transform);
                    }
                    if (bridgeBtnA == bridgeBtnList[i][bridgeBtnList[i].Count - 1] && low != null)
                    {
                        xPos = (low.transform.position.x + bridgeBtnA.transform.position.x) / 2;
                        yPos = (low.transform.position.y + bridgeBtnA.transform.position.y) / 2;
                        Vector3 pos2 = new Vector3(xPos, yPos, 90);

                        xPos = low.transform.position.x - bridgeBtnA.transform.position.x;
                        yPos = low.transform.position.y - bridgeBtnA.transform.position.y;
                        Vector2 direction2 = new Vector2(xPos, yPos);

                        angle = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
                        Quaternion angleAxis2 = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                        Instantiate(bridge, pos2, angleAxis2, GameObject.Find("Road").transform);
                    }
                }
                else if (middle == null)// Y��ǥ�� ������ ��ư�� ���� �� ��, �Ʒ� 2�� ����
                {
                    if(high != null)
                    {
                        float xPos = (high.transform.position.x + bridgeBtnA.transform.position.x) / 2;
                        float yPos = (high.transform.position.y + bridgeBtnA.transform.position.y) / 2;
                        Vector3 pos = new Vector3(xPos, yPos, 90);

                        xPos = high.transform.position.x - bridgeBtnA.transform.position.x;
                        yPos = high.transform.position.y - bridgeBtnA.transform.position.y;
                        Vector2 direction = new Vector2(xPos, yPos);
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                        Instantiate(bridge, pos, angleAxis, GameObject.Find("Road").transform);
                    }
                    if (low != null)
                    {
                        float xPos = (low.transform.position.x + bridgeBtnA.transform.position.x) / 2;
                        float yPos = (low.transform.position.y + bridgeBtnA.transform.position.y) / 2;
                        Vector3 pos = new Vector3(xPos, yPos, 90);

                        xPos = low.transform.position.x - bridgeBtnA.transform.position.x;
                        yPos = low.transform.position.y - bridgeBtnA.transform.position.y;
                        Vector2 direction = new Vector2(xPos, yPos);
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                        Instantiate(bridge, pos, angleAxis, GameObject.Find("Road").transform);
                    }
                }
            }
        }
    }
    #region ���
    private List<T> ShuffleList<T>(List<T> list) // ����Ʈ ����
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
    #endregion
    #region �ڷ�ƾ
    IEnumerator CoWaitForPosition() // 1������ �� ��ư ������ ��������
    {
        btnPosList.Clear();
        yield return new WaitForEndOfFrame();
        btnPosList.AddRange(GameObject.FindGameObjectsWithTag("StageBtn"));
        // ��ư ������ �޾ƿ���
        CreateBridge();
    }
    #endregion
}

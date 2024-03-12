using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

 // �����صξ��ٰ� ������ �ٽ� ������ ���Ǵ� ���� �ڿ� save �ּ� �ޱ�
[System.Serializable]
public class GameResolution
{
    public Screen screen;
    public FullScreenMode fullScreenMode;   // save
    public enum ResolutionSize {p360, p480, p720, p1080, p1440, p2160 }
    public ResolutionSize resolutionSize; // save
    public int screenWidth;
    public int screenHeight;

    public GameResolution(ResolutionSize resolutionSize, FullScreenMode fullScreenMode)
    {
        this.resolutionSize = resolutionSize;
        this.fullScreenMode = fullScreenMode;
    }

    public void SetResolution()
    {
        Dictionary<ResolutionSize, (int width, int height)> resolutionScreen = new()
        {
            { ResolutionSize.p360, (640, 360)},
            { ResolutionSize.p480, (854, 480)},
            { ResolutionSize.p720, (1280, 720)},
            { ResolutionSize.p1080, (1920, 1080)},
            { ResolutionSize.p1440, (2560, 1440)},
            { ResolutionSize.p2160, (3840, 2160)}
        };
        
        if(resolutionScreen.TryGetValue(resolutionSize, out var resolution))
        {
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode);
        }
        else
        {
            Debug.LogError("�������� �ʴ� �ػ� ũ���Դϴ�");
        }
    }
}
[System.Serializable]
public class GameVolume
{
    public int setVolume;   // ���� �Ѱ� ���� ���� ��� ����   //save

    public AudioMixer audioMixer; 
    public Slider audioSlider;

    public float masterVolume;  // save
    public float bgmVolume;     // save
    public float sfxVolume;     // save
}

public class GameSettingFunction : MonoBehaviour
{
    public GameResolution gameResolution;
    public GameVolume gameVolume;

    public TMP_Dropdown windowDropdowns;
    string windowDDKey = "WINDOW_DROPDOWN_KEY";
    int windowDDOption = 0;
    public TMP_Dropdown resolutionDropdowns;
    string resolutionDDKey = "RESOLUTION_DROPDOWN_KEY";
    int resolutionDDOption = 0;

    public List<string> dropdownList = new();

    private void Awake()
    {
        if (PlayerPrefs.HasKey(windowDDKey) == false) windowDDOption = 0;
        else windowDDOption = PlayerPrefs.GetInt(windowDDKey);
        if (PlayerPrefs.HasKey(resolutionDDKey) == false) resolutionDDOption = 0;
        else resolutionDDOption = PlayerPrefs.GetInt(resolutionDDKey);
    }

    void Start()
    {
        dropdownList.Add("��ü ȭ��");
        dropdownList.Add("��ü â ���");
        dropdownList.Add("â ���");
        windowDropdowns.AddOptions(dropdownList);

        dropdownList.Clear();

        dropdownList.Add("2160p");
        dropdownList.Add("1440p");
        dropdownList.Add("1080p");
        dropdownList.Add("720p");
        dropdownList.Add("480p");
        dropdownList.Add("320p");
        resolutionDropdowns.AddOptions(dropdownList);

        dropdownList.Clear();
    }

    public void OnResolutionButtonClick(GameResolution.ResolutionSize resolution, FullScreenMode fullScreenMode)
    {
        // GameResolution �ν��Ͻ� ����
        GameResolution gameResolution = new GameResolution(resolution, fullScreenMode);

        // SetResolution �޼��� ȣ��
        gameResolution.SetResolution();
    }

    public void AudioControl()
    {
        float sound;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        { SetSliderValue(ray, "MasterVolumeSlider", gameVolume.masterVolume); }
        else if (Input.GetMouseButtonDown(0))
        { SetSliderValue(ray, "BGMVolumeSlider", gameVolume.bgmVolume); }
        if (Input.GetMouseButtonDown(0))
        { SetSliderValue(ray, "SFXVolumeSlider", gameVolume.sfxVolume); }

    }

    void SetSliderValue(Ray _ray, string volumName, float volumValue)
    {
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit) && _hit.Equals(gameObject.name == volumName))
        {
            volumValue = _hit.transform.gameObject.GetComponentInChildren<Slider>().value;
            //gameVolume.audioMixer.
        }
    }
}

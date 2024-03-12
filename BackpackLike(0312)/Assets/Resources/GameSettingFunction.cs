using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

 // 저장해두었다가 게임을 다시 켰을때 사용되는 변수 뒤에 save 주석 달기
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
            Debug.LogError("지원하지 않는 해상도 크기입니다");
        }
    }
}
[System.Serializable]
public class GameVolume
{
    public int setVolume;   // 사운드 켜고 끄는 역할 담당 변수   //save

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
        dropdownList.Add("전체 화면");
        dropdownList.Add("전체 창 모드");
        dropdownList.Add("창 모드");
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
        // GameResolution 인스턴스 생성
        GameResolution gameResolution = new GameResolution(resolution, fullScreenMode);

        // SetResolution 메서드 호출
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

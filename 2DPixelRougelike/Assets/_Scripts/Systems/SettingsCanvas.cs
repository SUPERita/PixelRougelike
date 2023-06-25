using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvas : Singleton<SettingsCanvas>
{

    float lastTimeScale = 1;
    bool isOpen = false;
    [SerializeField] private CanvasGroup _c;
    [Header("---Audio")]
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private Slider sfxSlider = null;
    private string _sfxVolumeParameter = "SFXVolume";
    [SerializeField] private Slider musicSlider = null;
    private string _musicVolumeParameter = "MusicVolume";
    [Header("---Graphics")]
    [SerializeField] private Toggle fullScreenToggle = null;
    private string _fullScreenParameter = "IsFullScreen";
    [SerializeField] private TMP_Dropdown resolutionDrowdown;
    Resolution[] screenResolutions;
    private string _resolutionParameter = "Resolution";
    [Header("---Prefrences")]
    [SerializeField] private Toggle dmgNumbersToggle = null;
    private string _dmgNumbersParameter = "dmgNumbers";


    #region menu lifecycle
    public void OnPause(InputValue _value)
    {
        if (isOpen) { 
            CloseSettings(); 
        }
        else if(GameStateManager.Instance.GetCurrentGameState()==GameState.GameLoop){ 
            OpenSettings();
        }
    }

    [ButtonGroup]
    [Button]
    private void OpenSettings()
    {
        isOpen = true;
        //
        GameStateManager.Instance.SetState(GameState.Paused);

        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;

        Helpers.ToggleCanvas(_c, true);

        // set selected
        EventSystem.current.SetSelectedGameObject(musicSlider.gameObject);
    
    }
    [ButtonGroup]
    [Button]
    public void CloseSettings()
    {
        isOpen = false;
        GameStateManager.Instance.ReturnToLastState();

        Time.timeScale = lastTimeScale;

        Helpers.ToggleCanvas(_c, false);

        // set selected
        foreach(Button _b in FindObjectsOfType<Button>())
        {
            if (_b.IsInteractable())
            {
                EventSystem.current.SetSelectedGameObject(_b.gameObject);
                break;
            }
        }
            
    }
    [Button]
    private void ToggleCanvas()
    {
        Helpers.ToggleCanvas(_c);
    }

    #endregion

    //saving & loading
    void Start()
    {
        //make the canvas not show at start
        //CloseSettings(); // just toggle it nigga

        //handle resolutions
        HandleResolutions();

        //set listeners
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        resolutionDrowdown.onValueChanged.AddListener(SetResolution);
        dmgNumbersToggle.onValueChanged.AddListener(SetDamageNumbers);//dmg numbers
        //---load prefs
        LoadPrefs();
    }
    private void LoadPrefs()
    {
        //audio
        sfxSlider.value = PlayerPrefs.GetFloat(_sfxVolumeParameter, 1);
        OnSFXVolumeSliderChange(sfxSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(_musicVolumeParameter, 1);
        OnMusicVolumeSliderChange(musicSlider.value);

        //graphics
        SetFullScreen(IntToBool(PlayerPrefs.GetInt(_fullScreenParameter, 1)));
        SetResolution(PlayerPrefs.GetInt(_resolutionParameter, HandleResolutions()));

        //prefrences
        SetDamageNumbers(IntToBool(PlayerPrefs.GetInt(_dmgNumbersParameter, 1))); //dmg numbers

    }
    private void OnDisable()
    {
        //settingsCog.OnClickEvent -= SettingsCog_OnClickEvent;
        //qualityGroup.OnSelectedOption -= QualityGroup_OnSelectedOption;
        //frameGroup.OnSelectedOption -= FrameGroup_OnSelectedOption;
        //showFramesToggle.OnToggleClicked -= ShowFramesToggle_OnToggleClicked;

        //---save prefs
        PlayerPrefs.SetFloat(_sfxVolumeParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(_musicVolumeParameter, musicSlider.value);
        PlayerPrefs.SetInt(_fullScreenParameter, BoolToInt(Screen.fullScreen));
        PlayerPrefs.SetInt(_resolutionParameter, resolutionDrowdown.value);
        PlayerPrefs.SetInt(_dmgNumbersParameter, BoolToInt(showDamageNumbers));//dmg numbers
    }

    #region Volume
    private void OnSFXVolumeSliderChange(float _v)
    {
        SetMixerVolumeNormalized(_v, _sfxVolumeParameter);

        //display
        sfxSlider.SetValueWithoutNotify(_v);
    }
    private void OnMusicVolumeSliderChange(float _v)
    {
        SetMixerVolumeNormalized(_v, _musicVolumeParameter);

        //display
        musicSlider.SetValueWithoutNotify(_v);
    }
    private void SetMixerVolumeNormalized(float _v, string _mixerName)
    {
        //Debug.Log(_v);
        mixer.SetFloat(_mixerName, Mathf.Log10(_v) * 20f);
        if (_v == 0)
        {
            mixer.SetFloat(_mixerName, -80f);
        }
    }

    #endregion

    #region Graphics
    //fullscreen
    private void SetFullScreen(bool _b)
    {
        Screen.fullScreen = _b;

        fullScreenToggle.SetIsOnWithoutNotify(_b);
    }
    //resolutions
    /// <summary>
    /// populate the resolutions array and set the dropdown graphic to the current resolution
    /// </summary>
    /// <returns>current resolution Index</returns>
    private int HandleResolutions()
    {
        //filter resolutions
        screenResolutions = new Resolution[0];
        List<Resolution> _filteredResolutions = new List<Resolution>(); 
        foreach (Resolution _resolution in Screen.resolutions)
        {
            //if aleardy in the list skip it
            if(IsResolutionInList(_resolution, _filteredResolutions.ToArray())){
                continue;
            }

            //add resolution
            _filteredResolutions.Add(_resolution);
        }

        //get resolution
        screenResolutions = _filteredResolutions.ToArray();
        resolutionDrowdown.ClearOptions();
        List<string> _options = new List<string>();

        int _currentResolutionIndex = 0;
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            string _option = screenResolutions[i].width + "x" + screenResolutions[i].height;
            _options.Add(_option);

            if (screenResolutions[i].width == Screen.currentResolution.width &&
                screenResolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }

        //make the dropdown graphic show the current resolution
        resolutionDrowdown.AddOptions(_options);
        resolutionDrowdown.SetValueWithoutNotify(_currentResolutionIndex);
        resolutionDrowdown.RefreshShownValue();

        return _currentResolutionIndex;
    }
    private void SetResolution(int _resolutionIndex)
    {
        // if bug
        if(_resolutionIndex >= screenResolutions.Length) {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
            return;
        }

        Resolution _resolution = screenResolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);

        //update graphics
        resolutionDrowdown.SetValueWithoutNotify(_resolutionIndex);
        resolutionDrowdown.RefreshShownValue();
    }

    #endregion

    #region Prefrences

    //dmg numbers
    public bool showDamageNumbers { get; private set; } = true;
    private void SetDamageNumbers(bool _b)
    {
        showDamageNumbers = _b;

        dmgNumbersToggle.SetIsOnWithoutNotify(_b);
    }

    #endregion



    #region Just happend to be on the pause menu (nonessential)

    

    #endregion

    #region utils

    //utils
    public int BoolToInt(bool _b)
    {
        if (_b) return 1;

        return 0;
    }
    public bool IntToBool(int _i)
    {
        return _i == 1;
    }

    public bool CompareResolution(Resolution _resolution1, Resolution _resolution2)
    {
        return _resolution1.width == _resolution2.width &&
               _resolution1.height == _resolution2.height;
    }
    public bool IsResolutionInList(Resolution _res, Resolution[] _arr)
    {
        foreach(Resolution _i in _arr)
        {
            if(CompareResolution(_res, _i)) return true;
        }
        return false;
    }

    #endregion

}

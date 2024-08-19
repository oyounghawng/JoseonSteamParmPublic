using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    public List<UniversalRenderPipelineAsset> pipelineAssets;

    private Resolution[] resolutions;
    private List<int> frameRateList;

    [Header("Display Setting")]
    public TMP_Dropdown display;
    public TMP_Dropdown quality;
    public TMP_Dropdown frameRate;
    public Toggle vSync;

    [Space(10)]

    [Header("Sound Setting")]

    public Slider bgmSlider;
    public Slider effectSlider;

    public TMP_Text txtBgmVolume;
    public TMP_Text txtEffectVolume;


    private void OnEnable()
    {
        display.onValueChanged.RemoveListener(SetDisplay);
        display.onValueChanged.AddListener(SetDisplay);

        quality.onValueChanged.RemoveListener(SetQuality);
        quality.onValueChanged.AddListener(SetQuality);

        frameRate.onValueChanged.RemoveListener(SetFrameRate);
        frameRate.onValueChanged.AddListener(SetFrameRate);

        vSync.onValueChanged.AddListener(SetVsync);


        bgmSlider.onValueChanged.RemoveListener(SetBgmVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);

        effectSlider.onValueChanged.RemoveListener(SetEffectVolume);
        effectSlider.onValueChanged.AddListener(SetEffectVolume);

        bgmSlider.value = Managers.Sound.BgmVolume;
        effectSlider.value = Managers.Sound.EffectVolume;
    }

    public override void Init()
    {
        base.Init();

        InitializeDisplay();
        InitializeQuality();
        InitializeFrameRate();
    }

    #region Initialize Function

    private void InitializeDisplay()
    {
        resolutions = Screen.resolutions.Reverse().ToArray();

        display.ClearOptions();

        HashSet<string> options = new HashSet<string>();

        int currentDisplay = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height} ({(int)(resolutions[i].refreshRateRatio.numerator / resolutions[i].refreshRateRatio.denominator)}hz)";

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentDisplay = i;
            }
        }

        display.AddOptions(new List<string>(options));
        display.value = currentDisplay;
        display.RefreshShownValue();
    }

    private void InitializeQuality()
    {
        quality.ClearOptions();

        List<string> options = new List<string>();

        options.Add("»ó");
        options.Add("Áß");
        options.Add("ÇÏ");

        quality.AddOptions(options);
    }
    private void InitializeFrameRate()
    {
        frameRateList = new List<int>();

        frameRateList.Add(60);
        frameRateList.Add(30);
    }

    #endregion

    #region Display Option
    private void SetDisplay(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    private void SetQuality(int index)
    {
        if (index < 0)
            return;
        QualitySettings.SetQualityLevel(index);
        QualitySettings.renderPipeline = pipelineAssets[index];
    }

    private void SetFrameRate(int index)
    {
        int targetFrameRate = frameRateList[index];

        Application.targetFrameRate = targetFrameRate;
    }
    private void SetVsync(bool active)
    {
        QualitySettings.vSyncCount = active ? 1 : 0;
    }
    #endregion


    #region Sound Option

    private void SetBgmVolume(float volume)
    {
        txtBgmVolume.text = volume.ToString("F2");
        Managers.Sound.BgmVolume = volume;

    }

    private void SetEffectVolume(float volume)
    {
        txtEffectVolume.text = volume.ToString("F2");
        Managers.Sound.EffectVolume = volume;
    }
    #endregion

}
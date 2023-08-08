using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ControlTowerForRunningAlone : MonoBehaviour
{
    public Toggle avatarToggle;
    public Toggle mapToggle;
    public Toggle effectToggle;
    public Slider slider;
    public Player player;
    public RunningInfo runningInfo;
    public AvatarAlone avatarAlone;
    public Animator avatarAnime;
    public StateBar stateBar;
    public Camera arCamera;
    public PlayButton playButton;
    public LocationModule locacationModule;
    public GPXLogger GPXLogger;
    public TMP_Text timeText;
    public TMP_Text paceText;

    private float runningSpeed;
    private bool avatarToggleValue;
    private bool mapToggleValue;
    private bool effectToggleValue;
    private bool isPaused;
    private bool isPauseButtonPressed;
    private float time;

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;
        isPauseButtonPressed = true;
        SetAvatarAnime();
    }

    void Start()
    {
        avatarToggleValue = true;
        mapToggleValue = true;
        effectToggleValue = true;
        isPaused = true;
        isPauseButtonPressed = false;
        time = 0;
    }

    void Update()
    {
        CheckOption();
        SetAvatarAnime();        
        if (stateBar.GetIsCountDownEnd())
        {
            timeText.text = time.ToString();
            paceText.text = (player.GetTotalDist() / time).ToString();

            if (time == 0)
            {
                ToggleIsPaused();
                GPXLogger.enabled = true;
            }
            if (!isPaused)
                time += Time.deltaTime;
            if (isPauseButtonPressed)
            {
                player.ToggleIsPaused();
                avatarAlone.ToggleIsPaused();
                runningInfo.ToggleIsPaused();
                isPauseButtonPressed = false;
            }
        }
        else
        {            
            avatarAlone.FixAvatar();
            runningInfo.FixInfo();
        }
    }

    public void SetAvatarAnime()
    {
        if (!stateBar.GetIsCountDownEnd())
            return;
        if (isPaused)
        {
            avatarAnime.SetBool("isIdle", true);
            avatarAnime.SetBool("isRun", false);
            avatarAnime.SetBool("isSprint", false);
        }
        else if (runningSpeed >= 10)
        {
            avatarAnime.SetBool("isIdle", false);
            avatarAnime.SetBool("isRun", false);
            avatarAnime.SetBool("isSprint", true);
        }
        else
        {
            avatarAnime.SetBool("isIdle", false);
            avatarAnime.SetBool("isRun", true);
            avatarAnime.SetBool("isSprint", false);
        }         
    }

    public void CheckOption()
    {
        if (runningSpeed != slider.value)
        {
            runningSpeed = slider.value;
            avatarAlone.SetMovePerFrame(slider.value / 180);
        }
        if (avatarToggleValue != avatarToggle.isOn)
        {
            avatarToggleValue = avatarToggle.isOn;
            if (avatarToggleValue == true)
                arCamera.cullingMask |= 1 << LayerMask.NameToLayer("Avatar");
            else
                arCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Avatar"));
        }
        if (mapToggleValue != mapToggle.isOn)
        {
            //mapToggleValue = mapToggle.isOn;
            //miniMap.SetActive(mapToggleValue);
        }
        if (effectToggleValue != effectToggle.isOn)
        {
            effectToggleValue = effectToggle.isOn;
            if (effectToggleValue == true)
                arCamera.cullingMask |= 1 << LayerMask.NameToLayer("Effect");
            else
                arCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Effect"));
        }
    }
}
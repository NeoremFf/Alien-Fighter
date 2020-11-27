using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private Slider musicPowerSlider;
    [SerializeField] private Toggle musicStateCheckBox;

    [SerializeField] private AudioSource sound;
    [SerializeField] private Slider soundPowerSlider;
    [SerializeField] private Toggle soundStateCheckBox;
    [SerializeField] private SoundPlayer soundController;

    private bool isMuteMusic = true;
    private bool isMuteSound = true;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("Music_State");
        //PlayerPrefs.DeleteKey("Sound_State");

        if (!PlayerPrefs.HasKey("Music_State"))
        {
            PlayerPrefs.SetInt("Music_State", 1);
        }
        if (!PlayerPrefs.HasKey("Sound_State"))
        {
            PlayerPrefs.SetInt("Sound_State", 1);
        }
        if (!PlayerPrefs.HasKey("Music_Power"))
        {
            PlayerPrefs.SetFloat("Music_Power", 1);
        }
        if (!PlayerPrefs.HasKey("Sound_Power"))
        {
            PlayerPrefs.SetFloat("Sound_Power", 1);
        }

        Startup();
    }

    private void Startup()
    {
        if (musicStateCheckBox)
        {
            if (PlayerPrefs.GetInt("Music_State") == 1)
            {
                PlayerPrefs.SetInt("Music_State", 0);
                musicStateCheckBox.isOn = false;
            }
            else
            {
                PlayerPrefs.SetInt("Music_State", 1);
                musicStateCheckBox.isOn = true;
                SetMusicState();
            }

             musicPowerSlider.value = PlayerPrefs.GetFloat("Music_Power");
        }

        if (soundStateCheckBox)
        {
            if (PlayerPrefs.GetInt("Sound_State") == 1)
            {
                PlayerPrefs.SetInt("Sound_State", 0);
                soundStateCheckBox.isOn = false;
            }
            else
            {
                PlayerPrefs.SetInt("Sound_State", 1);
                soundStateCheckBox.isOn = true;
                SetSoundState();
            }

            soundPowerSlider.value = PlayerPrefs.GetFloat("Sound_Power");
        }
    }

    #region Music
    public void SetMusicState()
    {
        if (PlayerPrefs.GetInt("Music_State") == 1)
        {
            isMuteMusic = true;
            PlayerPrefs.SetInt("Music_State", 0);
        }
        else
        {
            isMuteMusic = false;
            PlayerPrefs.SetInt("Music_State", 1);
        }

        music.mute = isMuteMusic;
    }

    public void SetMusicPower()
    {
        music.volume = musicPowerSlider.value;

        PlayerPrefs.SetFloat("Music_Power", musicPowerSlider.value);
    }

    public bool GetMusicState()
    {
        return isMuteMusic;
    }
    #endregion

    #region Sound
    public void SetSoundState()
    {
        if (PlayerPrefs.GetInt("Sound_State") == 1)
        {
            isMuteSound = true;
            PlayerPrefs.SetInt("Sound_State", 0);
        }
        else
        {
            isMuteSound = false;
            PlayerPrefs.SetInt("Sound_State", 1);
        }

        soundController.SetSoundActive(!isMuteSound);
    }

    public void SetSoundPower()
    {
        sound.volume = soundPowerSlider.value;

        PlayerPrefs.SetFloat("Sound_Power", soundPowerSlider.value);
    }

    public bool GetSoundState()
    {
        return isMuteSound;
    }
    #endregion
}
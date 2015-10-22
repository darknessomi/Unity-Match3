using UnityEngine;
using System.Collections;

public class SoundMusicButtonControll : MonoBehaviour
{

    public UnityEngine.UI.Image Sound;      // button sound
    public UnityEngine.UI.Image Music;      // button music

    void Start()
    {
        SetButtonState();
    }

    /// <summary>
    /// Set state button music and sound when play game
    /// </summary>
    void SetButtonState()
    {
        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            Music.sprite = ButtonActionController.Click.ButtonSprite[0];
            MusicController.Music.MusicON();
        }
        else
        {
            Music.sprite = ButtonActionController.Click.ButtonSprite[1];
            MusicController.Music.MusicOFF();
        }

        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            Sound.overrideSprite = ButtonActionController.Click.ButtonSprite[2];
            SoundController.Sound.SoundON();
        }
        else
        {
            Sound.overrideSprite = ButtonActionController.Click.ButtonSprite[3];
            SoundController.Sound.SoundOFF();
        }
    }

    /// <summary>
    /// Set and change state of music in game
    /// </summary>
    public void BMusic()
    {
        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            Music.sprite = ButtonActionController.Click.ButtonSprite[1];
            PlayerPrefs.SetInt("MUSIC", 1);
            Debug.Log("MUSIC OFF");
            MusicController.Music.MusicOFF();
        }
        else
        {
            Music.sprite = ButtonActionController.Click.ButtonSprite[0];
            PlayerPrefs.SetInt("MUSIC", 0);
            Debug.Log("MUSIC ON");
            MusicController.Music.MusicON();
        }
        SoundController.Sound.Click();
    }

    /// <summary>
    /// Set and change state of sound background in game
    /// </summary>
    public void BSound()
    {

        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            PlayerPrefs.SetInt("SOUND", 1);
            Sound.overrideSprite = ButtonActionController.Click.ButtonSprite[3];
            SoundController.Sound.SoundOFF();
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
            Sound.overrideSprite = ButtonActionController.Click.ButtonSprite[2];
            SoundController.Sound.SoundON();
        }
        SoundController.Sound.Click();
    }
}

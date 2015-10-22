using UnityEngine;
using System.Collections;

public class ButtonActionController : MonoBehaviour
{

    public static ButtonActionController Click;     // instance of ButtonActionController

    public Sprite[] ButtonSprite;                   //sprite array of buttons
    void Awake()
    {
        if (Click == null)
        {
            DontDestroyOnLoad(gameObject);
            Click = this;
        }
        else if (Click != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// When select classic mode
    /// </summary>
    /// <param name="level">number of level</param>
    public void ClassicScene(int level)
    {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        PLayerInfo.MODE = 0;
        PLayerInfo.MapPlayer = new Player();
        PLayerInfo.MapPlayer.Level = level;
        PLayerInfo.MapPlayer.HightScore = level;
        PLayerInfo.MapPlayer.HightScore = PlayerPrefs.GetInt(PLayerInfo.KEY_CLASSIC_HISCORE, 0);
        Application.LoadLevel("PlayScene");
    }


    /// <summary>
    /// When select arcade mode
    /// </summary>
    /// <param name="player">info of level to play</param>
    public void ArcadeScene(Player player)
    {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        PLayerInfo.MODE = 1;
        PLayerInfo.MapPlayer = player;
        StartCoroutine(GotoScreen("PlayScene"));
    }


    public void SelectMap(int mode)
    {
        SoundController.Sound.Click();
        if (mode == 1)
            Application.LoadLevel("MapScene");
        else
            HomeScene();

        CameraMovement.StarPointMoveIndex = -1;
    }

    /// <summary>
    /// Go to a scene with name
    /// </summary>
    /// <param name="screen">name of the scene to direction</param>
    IEnumerator GotoScreen(string screen)
    {
        yield return new WaitForSeconds(0);
        Application.LoadLevel(screen);
    }

    public void HomeScene()
    {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        Application.LoadLevel("HomeScene");
    }

    /// <summary>
    /// Set and change state of music
    /// </summary>
    /// <param name="button">Image button</param>
    public void BMusic(UnityEngine.UI.Button button)
    {

        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            PlayerPrefs.SetInt("MUSIC", 1); // music off
        }
        else
        {
            PlayerPrefs.SetInt("MUSIC", 0); // music on
        }

    }
    /// <summary>
    /// Set and change state of sound background
    /// </summary>
    /// <param name="button">Image button</param>
    public void BSound(UnityEngine.UI.Image button)
    {
        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            PlayerPrefs.SetInt("SOUND", 1);
            button.overrideSprite = ButtonSprite[3];
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
            button.overrideSprite = ButtonSprite[2];
        }
    }
}

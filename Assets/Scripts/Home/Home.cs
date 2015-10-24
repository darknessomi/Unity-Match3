using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    void Start()
    {
        // hidden banner (banner only show on Game Play scene)
//        GoogleMobileAdsScript.advertise.HideBanner();
        MusicController.Music.BG_menu();
    }

    void Update()
    {
        // Exit game if click Escape key or back on mobile
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitOK();
        }
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void ExitOK()
    {
        Application.Quit();
    }

}

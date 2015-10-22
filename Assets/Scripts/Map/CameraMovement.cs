using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement mcamera;       // camera movement

    public static int StarPointMoveIndex;       // position index

    public RectTransform container;             // container of scroll view

    public GameObject PopUp;                    // popup show when click to item button level

    public GameObject StarPoint;                // position start

    public Sprite[] star;                       // arrays star of item level

    public GameObject fade;                     // fade animation

    float distance = 90.8f / 8680f;

    public static bool movement;

    public static bool setstate;

    public bool isPopup;


    Player map;


    void Awake()
    {
        mcamera = this;
    }

    void Start()
    {
        setLastpos();
        SetPoint();
        GoogleMobileAdsScript.advertise.HideBanner();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPopup)
        {
            UnfreezeMap();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonActionController.Click.HomeScene();
        }
    }

    /// <summary>
    /// set last position of container
    /// </summary>
    void setLastpos()
    {
        float lastp = PlayerPrefs.GetFloat("LASTPOS", 0);
        if (lastp < 0) lastp = 0;
        else if (lastp > 90.8000f) lastp = 90.8f;
        transform.position += new Vector3(0, lastp);
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, -lastp / distance + 4740f);
    }

    void SetPoint()
    {
        float x = PlayerPrefs.GetFloat("LASTPOSX", -0.0045f);
        float y = PlayerPrefs.GetFloat("LASTPOS", -3.587f);
        StarPoint.transform.position = new Vector3(x, y, StarPoint.transform.position.z);

    }

    /// <summary>
    /// Update positio camera when scroller
    /// </summary>
    public void CameraPosUpdate()
    {
        transform.position = new Vector3(transform.position.x, -(container.anchoredPosition.y - 4740f) * distance, transform.position.z);
        if (setstate)
            movement = true;
    }


    /// <summary>
    /// show infomation of level player
    /// </summary>
    /// <param name="_map"></param>
    public void PopUpShow(Player _map)
    {
        isPopup = true;
        CameraMovement.mcamera.FreezeMap();
        map = _map;
        Image[] stars = new Image[3];
        stars[0] = PopUp.transform.GetChild(1).GetComponent<Image>();
        stars[1] = PopUp.transform.GetChild(2).GetComponent<Image>();
        stars[2] = PopUp.transform.GetChild(3).GetComponent<Image>();


        for (int i = 0; i < 3; i++)
        {
            if (i < _map.Stars)
                stars[i].sprite = star[0];
            else
                stars[i].sprite = star[1];
        }
        PopUp.transform.GetChild(4).GetComponent<Text>().text = _map.HightScore.ToString();
        PopUp.transform.GetChild(6).GetComponent<Text>().text = _map.Level.ToString("00");
        Animation am = PopUp.GetComponent<Animation>();
        am.enabled = true;
        PopUp.SetActive(true);
    }

    public void ArcadeScene()
    {
        ButtonActionController.Click.ArcadeScene(map);
    }

    public void FreezeMap()
    {
        DataLoader.enableclick = false;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UnfreezeMap()
    {
        SoundController.Sound.Click();
        PopUp.SetActive(false);
        isPopup = false;
        DataLoader.enableclick = true;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

}

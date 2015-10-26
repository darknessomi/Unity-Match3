using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DataLoader : MonoBehaviour
{
    public static DataLoader Data;                              // instance of  this class

    public static List<Player> MyData = new List<Player>();     // list of Player object

    const string KEY_DATA = "DATA";                             // key PlayerPrefs

    const string KEY_FRISTTIME = "one";                         // key check first app to play 

    public static bool enableclick;

    public const string KEY_MAPPOS = "mappos";

    public GameObject map;

    private GameObject tmp;

    public GameObject mapParent;

    public List<GameObject> listmap1;

    public GameObject[] listmap;

    public Vector2[] mappos = new Vector2[40];

    public UnityEngine.UI.Image processbar;

    public Sprite[] MapSprite;

    bool hold;

    GameObject holdobj;

    const float STARMOVE_TIME = 1f;                             // time of movement icon select level

    void Awake()
    {
        Data = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        listmap = new GameObject[40];
        //PlayerPrefs.DeleteKey(KEY_FRISTTIME);
        if (PlayerPrefs.GetInt(KEY_FRISTTIME, 0) == 0)
        {
            PlayerPrefs.SetString(KEY_DATA, datadefaut);
            PlayerPrefs.SetInt(KEY_FRISTTIME, 1);
        }

        StartCoroutine(MapButtonDrawer());
    }

    GameObject moveobj(Vector3 mouseposition)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            GameObject tmp = Physics2D.OverlapPoint(touchPos).gameObject;
            if (tmp != null && tmp.tag == "map")
            {
                return tmp;
            }
        }
        return null;
    }

    /// <summary>
    /// Draw buttons level on scene
    /// </summary>
    /// <returns></returns>
    IEnumerator MapButtonDrawer()
    {
        DataLoader.enableclick = false;
        MapPosD();
        processbar.fillAmount = 0.3f;
        yield return new WaitForSeconds(0.3f);
        Debug.Log("1");
        PlayerUtils pu = new PlayerUtils();
        MyData = pu.Load();
        processbar.fillAmount = 0.5f;
        Debug.Log("2");
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 20; i++)
        {
            processbar.fillAmount += 0.0016834016834017f * 3;
            insmap(mappos[i], i);
            insmap(mappos[i + 20], i + 20);
            yield return null;
        }
        Debug.Log("3");
        processbar.transform.parent.gameObject.SetActive(false);
        DataLoader.enableclick = true;
        if (CameraMovement.StarPointMoveIndex != -1 && CameraMovement.StarPointMoveIndex != 40)
        {
            StarPointMove();
            yield return new WaitForSeconds(STARMOVE_TIME);
            CameraMovement.mcamera.PopUpShow(MyData[CameraMovement.StarPointMoveIndex]);

            PlayerPrefs.SetFloat("LASTPOS", listmap[CameraMovement.StarPointMoveIndex].transform.position.y);
            PlayerPrefs.SetFloat("LASTPOSX", listmap[CameraMovement.StarPointMoveIndex].transform.position.x);

        }
        else
        {
            CameraMovement.mcamera.StarPoint.transform.GetChild(0).GetComponent<Animation>().Play("StarPoint");
        }
    }



    Vector3 StringToVector3(string s)
    {
        Vector3 vt = Vector3.zero;
        string[] p = s.Split(',');
        vt = new Vector3(float.Parse(p[0]), float.Parse(p[1]));
        return vt;
    }

    void insmap(Vector3 pos, int index)
    {

        tmp = (GameObject)Instantiate(map);
        tmp.transform.position = new Vector3(pos.x, pos.y);
        tmp.transform.SetParent(mapParent.transform, false);
        listmap[index] = tmp;
        tmp.transform.GetChild(1).GetComponent<TextMesh>().text = (index + 1).ToString();
        tmp.name = (index + 1).ToString();
        Map m = tmp.GetComponent<Map>();
        m.map = MyData[index];
        m.SetMapInfo();
    }

    void StarPointMove()
    {
        DataLoader.enableclick = false;
        Vector3 newpos = listmap[CameraMovement.StarPointMoveIndex].transform.position + new Vector3(-0.05f, -0.2f, -0.3f);
        Ulti.MoveTo(CameraMovement.mcamera.StarPoint, newpos, STARMOVE_TIME, newpos.z);
        StartCoroutine(stopanimation());
    }
    IEnumerator stopanimation()
    {
        CameraMovement.mcamera.StarPoint.transform.GetChild(0).transform.localPosition = new Vector3(0, 0.619f, 0);
        yield return new WaitForSeconds(STARMOVE_TIME);
        CameraMovement.mcamera.StarPoint.transform.GetChild(0).GetComponent<Animation>().Play("StarPoint");
    }

    #region
	string datadefaut = "False,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,3,True,0,0,3,True,0,0,3,";
    /// <summary>
    /// position of buttons level is fixed
    /// </summary>
    void MapPosD()
    {

        mappos[0] = new Vector2(-0.66f, -1.06f);

        mappos[1] = new Vector2(0.42f, -0.67f);

        mappos[2] = new Vector2(-0.53f, 0.57f);

        mappos[3] = new Vector2(0.42f, 0.94f);

        mappos[4] = new Vector2(-0.66f, 2.08f);

        mappos[5] = new Vector2(0.16f, 2.52f);

        mappos[6] = new Vector2(-0.94f, 5.02f);

        mappos[7] = new Vector2(0.13f, 5.29f);

        mappos[8] = new Vector2(-0.84f, 6.51f);

        mappos[9] = new Vector2(0.63f, 7.32f);

        mappos[10] = new Vector2(-0.02f, 8.66f);

        mappos[11] = new Vector2(-0.02f, 10.59f);

        mappos[12] = new Vector2(0.98f, 11f);

        mappos[13] = new Vector2(-1.01f, 13.49f);

        mappos[14] = new Vector2(0.62f, 13.88f);

        mappos[15] = new Vector2(-0.69f, 15.09f);

        mappos[16] = new Vector2(0.16f, 15.53f);

        mappos[17] = new Vector2(-0.11f, 17.07f);

        mappos[18] = new Vector2(0.72f, 17.33f);

        mappos[19] = new Vector2(-0.73f, 21.15f);

        mappos[20] = new Vector2(0f, 21.18f);

        mappos[21] = new Vector2(-0.73f, 22.61f);

        mappos[22] = new Vector2(0.3f, 22.90f);

        mappos[23] = new Vector2(-0.26f, 24.54f);

        mappos[24] = new Vector2(0.3f, 24.6f);

        mappos[25] = new Vector2(1.4f, 25.29f);

        mappos[26] = new Vector2(0.88f, 27.09f);

        mappos[27] = new Vector2(0.2f, 26.88f);

        mappos[28] = new Vector2(-0.7f, 29.08f);

        mappos[29] = new Vector2(0.24f, 29.54f);

        mappos[30] = new Vector2(-0.48f, 30.86f);

        mappos[31] = new Vector2(0.12f, 31f);

        mappos[32] = new Vector2(-0.4f, 32.5f);

        mappos[33] = new Vector2(0.63f, 33.13f);

        mappos[34] = new Vector2(0.72f, 34.84f);

        mappos[35] = new Vector2(0f, 34.5f);

        mappos[36] = new Vector2(0.63f, 37.67f);

        mappos[37] = new Vector2(-0.2f, 37.3f);

        mappos[38] = new Vector2(-0.12f, 39.1f);

        mappos[39] = new Vector2(0.63f, 39.22f);

    }
    #endregion

}


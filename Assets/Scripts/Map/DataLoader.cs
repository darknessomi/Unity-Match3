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

    public Vector2[] mappos = new Vector2[50];

    public UnityEngine.UI.Image processbar;

    public GameObject fade;

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
        listmap = new GameObject[50];
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

        for (int i = 0; i < 25; i++)
        {
            processbar.fillAmount += 0.0016835016835017f * 3;
            insmap(mappos[i], i);
            insmap(mappos[i + 25], i + 25);
            yield return null;
        }
        Debug.Log("3");
        processbar.transform.parent.gameObject.SetActive(false);
        DataLoader.enableclick = true;
        if (CameraMovement.StarPointMoveIndex != -1 && CameraMovement.StarPointMoveIndex != 50)
        {
            StarPointMove();
            yield return new WaitForSeconds(STARMOVE_TIME);
            CameraMovement.mcamera.PopUpShow(MyData[CameraMovement.StarPointMoveIndex]);

            PlayerPrefs.SetFloat("LASTPOS", listmap[CameraMovement.StarPointMoveIndex].transform.position.y);
            PlayerPrefs.SetFloat("LASTPOSX", listmap[CameraMovement.StarPointMoveIndex].transform.position.x);

        }
        else
        {
            fade.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
        Vector3 newpos = listmap[CameraMovement.StarPointMoveIndex].transform.position + new Vector3(0, 0, -0.3f);
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

        mappos[0] = new Vector2(-0.004228723f, -3.587f);

        mappos[1] = new Vector2(0.695f, -3.232f);

        mappos[2] = new Vector2(-0.09021276f, -3.03f);

        mappos[3] = new Vector2(-0.925f, -2.859967f);

        mappos[4] = new Vector2(-0.188883f, -2.489f);

        mappos[5] = new Vector2(0.6173936f, -2.367f);

        mappos[6] = new Vector2(1.342f, -2f);

        mappos[7] = new Vector2(0.5240414f, -1.590361f);

        mappos[8] = new Vector2(-0.2140727f, -1.613735f);

        mappos[9] = new Vector2(-1.06f, -1.636f);

        mappos[10] = new Vector2(-1.579f, -1.265327f);

        mappos[11] = new Vector2(-1.274f, -0.7695142f);

        mappos[12] = new Vector2(-0.8071734f, 0.4705882f);

        mappos[13] = new Vector2(-0.9679077f, 1f);

        mappos[14] = new Vector2(-1.731092f, 1.497479f);

        mappos[15] = new Vector2(-0.9021276f, 1.77f);

        mappos[16] = new Vector2(-0.05638298f, 1.736f);

        mappos[17] = new Vector2(0.7893617f, 1.752f);

        mappos[18] = new Vector2(1.557f, 1.98f);

        mappos[19] = new Vector2(1.307f, 2.653f);

        mappos[20] = new Vector2(0.439f, 2.86f);

        mappos[21] = new Vector2(-0.271578f, 2.98f);

        mappos[22] = new Vector2(-0.9669681f, 3.106f);

        mappos[23] = new Vector2(-1.39f, 3.66f);

        mappos[24] = new Vector2(-0.8239896f, 4.221f);

        mappos[25] = new Vector2(-1f, 4.758f);

        mappos[26] = new Vector2(-1.562f, 5.279f);

        mappos[27] = new Vector2(-0.996f, 5.581f);

        mappos[28] = new Vector2(-0.2349291f, 5.59f);

        mappos[29] = new Vector2(0.4792553f, 5.6f);

        mappos[30] = new Vector2(1.184042f, 5.72f);

        mappos[31] = new Vector2(1.631163f, 6.270145f);

        mappos[32] = new Vector2(1.090071f, 6.87f);

        mappos[33] = new Vector2(0.3523936f, 7.128f);

        mappos[34] = new Vector2(-0.3636702f, 7.196f);

        mappos[35] = new Vector2(-1.067f, 7.494f);

        mappos[36] = new Vector2(-0.963f, 8.204f);

        mappos[37] = new Vector2(-0.601f, 8.72f);

        mappos[38] = new Vector2(0.017f, 9.103f);

        mappos[39] = new Vector2(0.5887378f, 9.264387f);

        mappos[40] = new Vector2(1.207f, 9.45f);

        mappos[41] = new Vector2(1.583f, 9.921f);

        mappos[42] = new Vector2(0.9122202f, 10.28887f);

        mappos[43] = new Vector2(0.3170127f, 10.39903f);

        mappos[44] = new Vector2(-0.3274053f, 10.45336f);

        mappos[45] = new Vector2(-0.9444384f, 10.69456f);

        mappos[46] = new Vector2(-1.259f, 11.225f);

        mappos[47] = new Vector2(-1.013f, 11.708f);

        mappos[48] = new Vector2(-0.62f, 12.499f);

        mappos[49] = new Vector2(-0.173f, 12.927f);

    }
    #endregion

}


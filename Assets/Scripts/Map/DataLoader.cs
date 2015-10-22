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

    public Vector2[] mappos = new Vector2[297];

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
        listmap = new GameObject[297];
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

        for (int i = 0; i < 99; i++)
        {
            processbar.fillAmount += 0.0016835016835017f * 3;
            insmap(mappos[i], i);
            insmap(mappos[i + 99], i + 99);
            insmap(mappos[i + 198], i + 198);

            yield return null;
        }
        Debug.Log("3");
        processbar.transform.parent.gameObject.SetActive(false);
        DataLoader.enableclick = true;
        if (CameraMovement.StarPointMoveIndex != -1 && CameraMovement.StarPointMoveIndex != 297)
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
    string datadefaut = "False,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,1,True,0,0,1,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,";
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

        mappos[50] = new Vector2(0.2264377f, 13.25734f);

        mappos[51] = new Vector2(0.6922522f, 13.51896f);

        mappos[52] = new Vector2(1.184f, 13.786f);

        mappos[53] = new Vector2(1.528f, 14.277f);

        mappos[54] = new Vector2(1.381f, 14.842f);

        mappos[55] = new Vector2(0.818f, 15.127f);

        mappos[56] = new Vector2(0.2264377f, 15.20605f);

        mappos[57] = new Vector2(-0.329952f, 15.26113f);

        mappos[58] = new Vector2(-0.9251596f, 15.40536f);

        mappos[59] = new Vector2(-1.41f, 15.694f);

        mappos[60] = new Vector2(-1.674f, 16.34f);

        mappos[61] = new Vector2(-1.663f, 16.935f);

        mappos[62] = new Vector2(-1.254f, 17.392f);

        mappos[63] = new Vector2(-0.615f, 17.391f);

        mappos[64] = new Vector2(0.03234824f, 17.3409f);

        mappos[65] = new Vector2(0.7051916f, 17.40975f);

        mappos[66] = new Vector2(1.337f, 17.79f);

        mappos[67] = new Vector2(1.428f, 18.401f);

        mappos[68] = new Vector2(1.139f, 18.952f);

        mappos[69] = new Vector2(0.698f, 19.339f);

        mappos[70] = new Vector2(0.218f, 19.65f);

        mappos[71] = new Vector2(-0.3040734f, 19.95054f);

        mappos[72] = new Vector2(-0.595f, 20.795f);

        mappos[73] = new Vector2(-1.171f, 21.165f);

        mappos[74] = new Vector2(-1.759f, 21.565f);

        mappos[75] = new Vector2(-0.848f, 21.87f);

        mappos[76] = new Vector2(-0.1488019f, 21.88109f);

        mappos[77] = new Vector2(0.6146165f, 21.9224f);

        mappos[78] = new Vector2(1.257f, 22.087f);

        mappos[79] = new Vector2(1.636f, 22.677f);

        mappos[80] = new Vector2(1.002796f, 23.1341f);

        mappos[81] = new Vector2(0.2911341f, 23.24426f);

        mappos[82] = new Vector2(-0.4205271f, 23.28556f);

        mappos[83] = new Vector2(-1.067491f, 23.36818f);

        mappos[84] = new Vector2(-1.568f, 23.924f);

        mappos[85] = new Vector2(-0.769888f, 24.30333f);

        mappos[86] = new Vector2(0.006469648f, 24.34464f);

        mappos[87] = new Vector2(0.848f, 24.354f);

        mappos[88] = new Vector2(1.634f, 24.472f);

        mappos[89] = new Vector2(2.097f, 25.069f);

        mappos[90] = new Vector2(1.583f, 25.679f);

        mappos[91] = new Vector2(0.796f, 25.85f);

        mappos[92] = new Vector2(-0.05822683f, 25.80419f);

        mappos[93] = new Vector2(-0.8345845f, 25.68026f);

        mappos[94] = new Vector2(-1.657f, 25.697f);

        mappos[95] = new Vector2(-1.737f, 26.462f);

        mappos[96] = new Vector2(-1.155f, 27.123f);

        mappos[97] = new Vector2(-0.213f, 27.34f);

        mappos[98] = new Vector2(0.429f, 27.742f);

        mappos[99] = new Vector2(0.892f, 28.338f);

        mappos[100] = new Vector2(0.986f, 29.143f);

        mappos[101] = new Vector2(0.455f, 29.713f);

        mappos[102] = new Vector2(-0.1876198f, 30.10146f);

        mappos[103] = new Vector2(-0.8863418f, 30.37684f);

        mappos[104] = new Vector2(-1.542f, 30.722f);

        mappos[105] = new Vector2(-1.79f, 31.43f);

        mappos[106] = new Vector2(-1.293f, 31.832f);

        mappos[107] = new Vector2(-0.537f, 31.743f);

        mappos[108] = new Vector2(0.493f, 31.705f);

        mappos[109] = new Vector2(1.339217f, 31.8842f);

        mappos[110] = new Vector2(2.045f, 32.401f);

        mappos[111] = new Vector2(1.5f, 33.043f);

        mappos[112] = new Vector2(0.7569487f, 33.12344f);

        mappos[113] = new Vector2(-0.03234824f, 33.09591f);

        mappos[114] = new Vector2(-0.7569487f, 33.09591f);

        mappos[115] = new Vector2(-1.442731f, 33.26114f);

        mappos[116] = new Vector2(-1.326278f, 34.01845f);

        mappos[117] = new Vector2(-0.5887378f, 34.21122f);

        mappos[118] = new Vector2(0.09704469f, 34.23876f);

        mappos[119] = new Vector2(0.8216451f, 34.32138f);

        mappos[120] = new Vector2(1.563f, 34.803f);

        mappos[121] = new Vector2(1.054552f, 35.4792f);

        mappos[122] = new Vector2(0.2523163f, 35.56182f);

        mappos[123] = new Vector2(-0.55f, 35.563f);

        mappos[124] = new Vector2(-1.371f, 35.665f);

        mappos[125] = new Vector2(-1.954f, 36.421f);

        mappos[126] = new Vector2(-1.927f, 37.132f);

        mappos[127] = new Vector2(-1.365096f, 37.64099f);

        mappos[128] = new Vector2(-0.5757986f, 37.66853f);

        mappos[129] = new Vector2(0.2264377f, 37.29676f);

        mappos[130] = new Vector2(1.05f, 37.046f);

        mappos[131] = new Vector2(1.589f, 37.553f);

        mappos[132] = new Vector2(1.451f, 38.275f);

        mappos[133] = new Vector2(0.705f, 38.834f);

        mappos[134] = new Vector2(0.03234824f, 39.11264f);

        mappos[135] = new Vector2(-0.375f, 39.774f);

        mappos[136] = new Vector2(0.2134984f, 40.287f);

        mappos[137] = new Vector2(0.95f, 40.561f);

        mappos[138] = new Vector2(1.352f, 41.178f);

        mappos[139] = new Vector2(0.7957666f, 41.69f);

        mappos[140] = new Vector2(-0.07116612f, 41.865f);

        mappos[141] = new Vector2(-0.8345845f, 42.07f);

        mappos[142] = new Vector2(-1.391f, 42.64414f);

        mappos[143] = new Vector2(-0.8475238f, 43.2008f);

        mappos[144] = new Vector2(0.05822683f, 43.40195f);

        mappos[145] = new Vector2(0.874f, 43.678f);

        mappos[146] = new Vector2(1.15f, 44.36f);

        mappos[147] = new Vector2(0.54992f, 44.88013f);

        mappos[148] = new Vector2(-0.2134984f, 44.918f);

        mappos[149] = new Vector2(-0.9251596f, 45.02f);

        mappos[150] = new Vector2(-1.631f, 45.40336f);

        mappos[151] = new Vector2(-1.69f, 46.1f);

        mappos[152] = new Vector2(-1.132188f, 46.68477f);

        mappos[153] = new Vector2(-0.4205271f, 46.89131f);

        mappos[154] = new Vector2(0.2781948f, 47.08408f);

        mappos[155] = new Vector2(0.982f, 47.572f);

        mappos[156] = new Vector2(1.212f, 48.299f);

        mappos[157] = new Vector2(0.7828273f, 48.911f);

        mappos[158] = new Vector2(0.109984f, 49.03011f);

        mappos[159] = new Vector2(-0.6275558f, 49.04388f);

        mappos[160] = new Vector2(-1.379f, 49.142f);

        mappos[161] = new Vector2(-1.592f, 49.856f);

        mappos[162] = new Vector2(-0.8863418f, 50.31066f);

        mappos[163] = new Vector2(-0.1876198f, 50.442f);

        mappos[164] = new Vector2(0.6146165f, 50.44f);

        mappos[165] = new Vector2(1.264f, 50.744f);

        mappos[166] = new Vector2(0.866f, 51.208f);

        mappos[167] = new Vector2(0.2393769f, 51.482f);

        mappos[168] = new Vector2(-0.382f, 52.011f);

        mappos[169] = new Vector2(0.1229233f, 52.654f);

        mappos[170] = new Vector2(0.848f, 52.865f);

        mappos[171] = new Vector2(1.449f, 53.399f);

        mappos[172] = new Vector2(0.9122202f, 53.93117f);

        mappos[173] = new Vector2(0.1746805f, 54.11017f);

        mappos[174] = new Vector2(-0.6016772f, 54.31671f);

        mappos[175] = new Vector2(-1.346f, 54.684f);

        mappos[176] = new Vector2(-1.632f, 55.268f);

        mappos[177] = new Vector2(-0.731f, 55.509f);

        mappos[178] = new Vector2(0.05822683f, 55.50087f);

        mappos[179] = new Vector2(0.860463f, 55.45956f);

        mappos[180] = new Vector2(1.39f, 56.074f);

        mappos[181] = new Vector2(0.8345845f, 56.79091f);

        mappos[182] = new Vector2(0.1229233f, 57.0663f);

        mappos[183] = new Vector2(-0.6016772f, 57.159f);

        mappos[184] = new Vector2(-1.291f, 57.354f);

        mappos[185] = new Vector2(-1.478f, 57.954f);

        mappos[186] = new Vector2(-1.041613f, 58.51208f);

        mappos[187] = new Vector2(-0.3428913f, 58.7737f);

        mappos[188] = new Vector2(0.4334664f, 58.7737f);

        mappos[189] = new Vector2(1.095f, 58.771f);

        mappos[190] = new Vector2(1.654f, 59.268f);

        mappos[191] = new Vector2(1.698f, 60.006f);

        mappos[192] = new Vector2(0.2523163f, 61.01823f);

        mappos[193] = new Vector2(-0.4981629f, 61.01926f);

        mappos[194] = new Vector2(-1.18f, 61.044f);

        mappos[195] = new Vector2(-1.764f, 61.535f);

        mappos[196] = new Vector2(-1.27452f, 61.99689f);

        mappos[197] = new Vector2(-0.64f, 62.125f);

        mappos[198] = new Vector2(0.07116612f, 62.09327f);

        mappos[199] = new Vector2(0.8216451f, 62.07951f);

        mappos[200] = new Vector2(1.577f, 62.14f);

        mappos[201] = new Vector2(1.507428f, 62.802f);

        mappos[202] = new Vector2(0.860463f, 63.042f);

        mappos[203] = new Vector2(0.1876198f, 63.20859f);

        mappos[204] = new Vector2(-0.554f, 63.559f);

        mappos[205] = new Vector2(-0.679313f, 64.07061f);

        mappos[206] = new Vector2(-0.006469648f, 64.26888f);

        mappos[207] = new Vector2(0.679313f, 64.3515f);

        mappos[208] = new Vector2(1.331f, 64.598f);

        mappos[209] = new Vector2(1.733f, 65.177f);

        mappos[210] = new Vector2(1.416f, 65.765f);

        mappos[211] = new Vector2(0.7957666f, 66.01759f);

        mappos[212] = new Vector2(0.1617411f, 66.18282f);

        mappos[213] = new Vector2(-0.4852235f, 66.34806f);

        mappos[214] = new Vector2(-1.118f, 66.678f);

        mappos[215] = new Vector2(-1.479f, 67.221f);

        mappos[216] = new Vector2(-1.387f, 67.987f);

        mappos[217] = new Vector2(-1.241f, 68.757f);

        mappos[218] = new Vector2(-0.8345845f, 69.316f);

        mappos[219] = new Vector2(-0.1617411f, 69.34177f);

        mappos[220] = new Vector2(0.4722843f, 69.35554f);

        mappos[221] = new Vector2(1.09337f, 69.45193f);

        mappos[222] = new Vector2(1.652f, 69.92f);

        mappos[223] = new Vector2(1.46861f, 70.59f);

        mappos[224] = new Vector2(0.8345845f, 70.81509f);

        mappos[225] = new Vector2(0.1488019f, 70.98032f);

        mappos[226] = new Vector2(-0.534f, 71.172f);

        mappos[227] = new Vector2(-1.156f, 71.48979f);

        mappos[228] = new Vector2(-1.551f, 72.155f);

        mappos[229] = new Vector2(-1.028674f, 72.745f);

        mappos[230] = new Vector2(-0.2781948f, 72.78366f);

        mappos[231] = new Vector2(0.4464056f, 72.639f);

        mappos[232] = new Vector2(1.113f, 72.598f);

        mappos[233] = new Vector2(1.525f, 73.121f);

        mappos[234] = new Vector2(1.506f, 73.837f);

        mappos[235] = new Vector2(1.011f, 74.29829f);

        mappos[236] = new Vector2(0.3946485f, 74.577f);

        mappos[237] = new Vector2(-0.2264377f, 74.80776f);

        mappos[238] = new Vector2(-0.861f, 75.08315f);

        mappos[239] = new Vector2(-1.428f, 75.544f);

        mappos[240] = new Vector2(-1.464f, 76.365f);

        mappos[241] = new Vector2(-0.7569487f, 76.68608f);

        mappos[242] = new Vector2(-0.07116612f, 76.72739f);

        mappos[243] = new Vector2(0.6404951f, 76.762f);

        mappos[244] = new Vector2(1.371f, 76.925f);

        mappos[245] = new Vector2(1.544f, 77.566f);

        mappos[246] = new Vector2(0.951038f, 78.024f);

        mappos[247] = new Vector2(0.2781948f, 78.231f);

        mappos[248] = new Vector2(-0.446f, 78.376f);

        mappos[249] = new Vector2(-1.101f, 78.629f);

        mappos[250] = new Vector2(-1.63f, 79.149f);

        mappos[251] = new Vector2(-1.609f, 79.814f);

        mappos[252] = new Vector2(-1.025f, 80.375f);

        mappos[253] = new Vector2(-0.2393769f, 80.52411f);

        mappos[254] = new Vector2(0.3817092f, 80.60673f);

        mappos[255] = new Vector2(0.936f, 80.96473f);

        mappos[256] = new Vector2(1.248642f, 81.57059f);

        mappos[257] = new Vector2(1.171006f, 82.25905f);

        mappos[258] = new Vector2(0.5369807f, 82.43806f);

        mappos[259] = new Vector2(-0.1358626f, 82.43806f);

        mappos[260] = new Vector2(-0.7569487f, 82.45182f);

        mappos[261] = new Vector2(-1.329f, 82.676f);

        mappos[262] = new Vector2(-1.651f, 83.315f);

        mappos[263] = new Vector2(-1.723f, 83.929f);

        mappos[264] = new Vector2(-1.331f, 84.601f);

        mappos[265] = new Vector2(-0.7181308f, 84.98347f);

        mappos[266] = new Vector2(-0.07116612f, 84.88708f);

        mappos[267] = new Vector2(0.6146165f, 84.74938f);

        mappos[268] = new Vector2(1.261581f, 84.74938f);

        mappos[269] = new Vector2(1.38f, 85.375f);

        mappos[270] = new Vector2(0.941f, 85.829f);

        mappos[271] = new Vector2(0.2781948f, 86.16763f);

        mappos[272] = new Vector2(-0.4075879f, 86.27779f);

        mappos[273] = new Vector2(-1.052f, 86.4f);

        mappos[274] = new Vector2(-1.624f, 86.823f);

        mappos[275] = new Vector2(-1.746f, 87.543f);

        mappos[276] = new Vector2(-1.217f, 87.962f);

        mappos[277] = new Vector2(-0.413f, 87.571f);

        mappos[278] = new Vector2(0.3170127f, 87.69275f);

        mappos[279] = new Vector2(0.8087059f, 88.20222f);

        mappos[280] = new Vector2(1.653f, 88.344f);

        mappos[281] = new Vector2(1.891f, 88.914f);

        mappos[282] = new Vector2(1.64976f, 89.57915f);

        mappos[283] = new Vector2(1.080431f, 89.84077f);

        mappos[284] = new Vector2(0.3946485f, 89.68931f);

        mappos[285] = new Vector2(-0.200559f, 89.40015f);

        mappos[286] = new Vector2(-0.8216451f, 89.20738f);

        mappos[287] = new Vector2(-1.503f, 89.506f);

        mappos[288] = new Vector2(-1.736f, 90.223f);

        mappos[289] = new Vector2(-1.506f, 91.0685f);

        mappos[290] = new Vector2(-0.7440094f, 91.38519f);

        mappos[291] = new Vector2(0.122f, 91.242f);

        mappos[292] = new Vector2(0.93f, 91.686f);

        mappos[293] = new Vector2(1.492f, 92.37659f);

        mappos[294] = new Vector2(0.769888f, 92.56936f);

        mappos[295] = new Vector2(-0.03234824f, 92.5969f);

        mappos[296] = new Vector2(-0.956f, 92.74f);

    }
    #endregion

}


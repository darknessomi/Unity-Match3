using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class JewelSpawner : MonoBehaviour
{

    public static JewelSpawner spawn;

    public GameObject[,] JewelGrib;

    public JewelObj[,] JewelGribScript;

    public GameObject JewelParent;

    public GameObject JewelObject;

    public Sprite[] JewelSprite;

    public GameObject Star;

    public GameObject StarEffect;

    public GameObject JewelColor;

    public GameObject NoSelect;

    private const float BaseDistance = 1f;

    private GameObject ObjTmp;

    private JewelObj JewelScript;

    public List<GameObject>[] prespawnlist = new List<GameObject>[7];

    void Awake()
    {
        spawn = this;

        for (int i = 0; i < 7; i++)
        {
            prespawnlist[i] = new List<GameObject>();
        }
    }

    public void JewelMapCreate(int[,] Map)
    {
        JewelGrib = new GameObject[7, 9];

        JewelGribScript = new JewelObj[7, 9];

        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 4)
                    s = y;
            }
            for (int y = s; y < 9; y++)
            {
                if (Map[x, y] > 0)
                {
                    RJewelInstantiate(x, y);
                }
            }
        }

        while (!Supporter.sp.isNoMoreMove())
        {
            RemakeGrib();
            JewelMapCreate(Map);
        }
    }
    void RemakeGrib()
    {
        for (int x = 0; x < 7; x++)
            for (int y = 0; y < 9; y++)
                if (JewelGrib[x, y] != null && JewelGribScript[x, y] != GameController.action.JewelStar)
                {
                    Destroy(JewelGrib[x, y]);
                    JewelGribScript[x, y] = null;
                }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < prespawnlist[i].Count; j++)
            {
                if (prespawnlist[i][j] != null)
                    Destroy(prespawnlist[i][j]);
            }
            prespawnlist[i].Clear();
        }

    }

    public IEnumerator Respawn()
    {
    loop:
        RemakeGrib();

        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 4)
                    s = y;
            }
            for (int y = s; y < 9; y++)
            {
                if (GribManager.cell.Map[x, y] > 0 && JewelGribScript[x, y] == null)
                {
                    RJewelInstantiate(x, y);
                }
            }
        }

        while (!Supporter.sp.isNoMoreMove())
        {
            goto loop;
        }

        EnableAllJewel();
        yield return new WaitForSeconds(0.75f);
        Timer.timer.NoSelect.SetActive(false);
        Timer.timer.Nomove.SetActive(false);
    }

    public void EnableAllJewel()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (JewelGribScript[x, y] != null && JewelGribScript[x, y] != GameController.action.JewelStar)
                    JewelGribScript[x, y].JewelEnable();
            }
        }
    }

    public GameObject JewelInstantiate(int x, int y)
    {
        ObjTmp = (GameObject)Instantiate(JewelObject);
        JewelScript = ObjTmp.GetComponent<JewelObj>();
        ObjTmp.transform.SetParent(JewelParent.transform, false);
        ObjTmp.transform.localPosition = new Vector3(ObjTmp.transform.localPosition.x + x * BaseDistance, ObjTmp.transform.localPosition.y + y * BaseDistance);
        JewelGrib[x, y] = ObjTmp;
        JewelGribScript[x, y] = JewelScript;
        int r = 0;

        if (PLayerInfo.MODE == 1)
            r = Random.Range(0, 6);
        else
            r = Random.Range(0, 7);

        JewelScript.render.sprite = JewelSprite[r];
        JewelScript.jewel.JewelPosition = new Vector2(x, y);
        JewelScript.jewel.JewelType = r;

        return ObjTmp;
    }

    public GameObject JewelInstantiatebt(int x, int y)
    {
        GameObject tmp;
        tmp = (GameObject)Instantiate(JewelObject);
        JewelScript = tmp.GetComponent<JewelObj>();

        tmp.transform.SetParent(JewelParent.transform, false);
        JewelScript.render.enabled = true;
        JewelGrib[x, y] = ObjTmp;
        JewelGribScript[x, y] = JewelScript;

        int r = 0;

        if (PLayerInfo.MODE == 1)
            r = Random.Range(0, 6);
        else
            r = Random.Range(0, 7);
        JewelScript.render.sprite = JewelSprite[r];
        JewelScript.jewel.JewelPosition = new Vector2(x, 9);
        JewelScript.jewel.JewelType = r;
        JewelScript.jewel.JewelPower = 0;
        return tmp;
    }

    GameObject remakeJewel(GameObject obj, int x)
    {
        GameObject o = obj;
        Animation anim = ObjTmp.GetComponent<Animation>();
        if (anim.GetClipCount() > 0)
            anim.RemoveClip("Moveto");

        prespawnlist[x].RemoveAt(0);


        o.transform.GetChild(0).transform.localPosition = Vector3.zero;

        if (o.transform.GetChild(0).gameObject.transform.childCount > 0)
            Destroy(o.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        return o;
    }

    GameObject RJewelInstantiate(int x, int y)
    {
        ObjTmp = (GameObject)Instantiate(JewelObject);
        JewelScript = ObjTmp.GetComponent<JewelObj>();
        ObjTmp.transform.SetParent(JewelParent.transform, false);
        ObjTmp.transform.localPosition = new Vector3(x, y);
        ObjTmp.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0, 0, 1);
        JewelGrib[x, y] = ObjTmp;
        JewelGribScript[x, y] = JewelScript;

        int r = randomjewel(x, y);

        JewelScript.render.sprite = JewelSprite[r];
        JewelScript.jewel.JewelPosition = new Vector2(x, y);
        JewelScript.jewel.JewelType = r;

        return ObjTmp;
    }

    int randomjewel(int x, int y)
    {
        int r = -1;
        int dem = 0;
        while (true)
        {
            if (PLayerInfo.MODE == 1)
                r = Random.Range(0, 6);
            else
                r = Random.Range(0, 7);

            if (x < 2 || JewelGribScript[x - 1, y] == null | JewelGribScript[x - 2, y] == null || r != JewelGribScript[x - 1, y].jewel.JewelType || r != JewelGribScript[x - 2, y].jewel.JewelType)
            {
                if (y < 2 || JewelGribScript[x, y - 1] == null || JewelGribScript[x, y - 2] == null || JewelGribScript[x, y - 1].jewel.JewelType != r || JewelGribScript[x, y - 2].jewel.JewelType != r)
                {
                    return r;
                }
            }
            dem++;
            if (dem > 100)
                return 0;
        }
    }

    public IEnumerator remakeGrib()
    {
        NoSelect.SetActive(true);
        yield return new WaitForSeconds(1f);
    }

    public GameObject SpawnJewelPower(int type, int power, Vector3 pos)
    {
        GameObject tmp;
        int x = (int)pos.x;
        int y = (int)pos.y;
        if (JewelGrib[x, y] != null)
            Destroy(JewelGrib[x, y]);
        if (type == 8)
        {
            tmp = (GameObject)Instantiate(JewelColor);
        }
        else
        {
            tmp = (GameObject)Instantiate(JewelObject);
        }

        JewelScript = tmp.GetComponent<JewelObj>();
        JewelScript.render.enabled = true;
        tmp.transform.SetParent(JewelParent.transform, false);
        tmp.transform.localPosition = new Vector3(x, y, pos.z);
        JewelGrib[x, y] = tmp;
        JewelGribScript[x, y] = JewelScript;
        if (type != 8)
            JewelScript.render.sprite = JewelSprite[type];
        JewelScript.jewel.JewelPosition = new Vector2(x, y);
        JewelScript.jewel.JewelType = type;
        JewelScript.jewel.JewelPower = power;
        tmp.GetComponent<Collider2D>().enabled = false;
        if (power == (int)GameController.Power.BOOM)
            EffectSpawner.effect.Enchant(tmp.transform.GetChild(0).gameObject);
        return tmp;
    }

    public void SpawnStar(Vector2 pos)
    {
        if (JewelGribScript[(int)pos.x, (int)pos.y] != null)
            Destroy(JewelGrib[(int)pos.x, (int)pos.y]);

        GameObject tmp = (GameObject)Instantiate(Star);
        tmp.name = "JewelStar";
        tmp.transform.SetParent(JewelParent.transform, false);
        tmp.transform.localPosition = new Vector3(pos.x, pos.y);
        tmp.transform.GetChild(0).gameObject.SetActive(false);
        JewelScript = tmp.GetComponent<JewelObj>();
        JewelScript.jewel.JewelPosition = pos;
        JewelGribScript[(int)pos.x, (int)pos.y] = JewelScript;
        JewelGrib[(int)pos.x, (int)pos.y] = tmp;
        GameController.action.JewelStar = JewelScript;

        StarEffect.SetActive(true);
    }

}

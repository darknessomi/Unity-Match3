using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public static GameController action;

    public static float DROP_SPEED = 8;
    public static float DROP_DELAY = 0.5f;

    public int GameState;

    public int CellNotEmpty;

    public GameObject Selector;
    public enum Power
    {
        BOOM = 1,
        ROW_LIGHTING = 2,
        COLLUMN_LIGHTING = 3,
        MAGIC = 8,
        TIME = 4,
    }

    public SpawnController drop;

    public GameObject NoSelect;

    public JewelObj JewelStar;

    public bool isStar;

    public bool isShowStar;

    public bool isAddPower;

    public Animation StartAnim;

    private JewelObj JewelScript;
    private JewelObj JewelScript1;

    private GameObject Pointer;

    private GameObject Selected;

    bool ishold;
    void Awake()
    {
        action = this;
    }

    IEnumerator Start()
    {

        if (PLayerInfo.MODE == 1)
            StartCoroutine(GribManager.cell.GribMapCreate(PLayerInfo.MapPlayer.Name));
        else
            StartCoroutine(GribManager.cell.GribMapCreate("classic"));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(EffectSpawner.effect.ComboTick());
        Timer.timer.TimeTick(true);
        GameState = (int)Timer.GameState.PLAYING;
        NoSelect.SetActive(false);
    }

    void Update()
    {
        JewelSelecter();
        backpress();
    }
    //process click action
    void JewelSelecter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ishold = true;

            if (Pointer == null)
            {
                Pointer = JewelTouchChecker(Input.mousePosition);
            }

            Supporter.sp.StopSuggestionAnim();
            if (Pointer != null && !Pointer.name.Contains("Jewel"))
                Pointer = null;
        }
        else if (Input.GetMouseButton(0) && ishold)
        {
            if (Pointer != null)
            {
                EnableSelector(Pointer.transform.position);
                Selected = JewelTouchChecker(Input.mousePosition);
                if (Selected != null && Pointer != Selected && Selected.name.Contains("Jewel"))
                {
                    if (DistanceChecker(Pointer, Selected))
                    {
                        RuleChecker(Pointer, Selected);
                        Pointer = null;
                        Selected = null;
                        Selector.SetActive(false);
                    }
                    else
                    {
                        Pointer = Selected;
                        Selected = null;
                        EnableSelector(Pointer.transform.position);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ishold = false;
        }
    }
    //check distance between 2 object
    bool DistanceChecker(GameObject obj1, GameObject obj2)
    {
        Vector2 v1 = obj1.GetComponent<JewelObj>().jewel.JewelPosition;
        Vector2 v2 = obj2.GetComponent<JewelObj>().jewel.JewelPosition;
        if (Vector2.Distance(v1, v2) <= 1)
        {
            return true;
        }

        return false;
    }
    //check logic game
    public void RuleChecker(GameObject obj1, GameObject obj2)
    {
        JewelObj Jewel1 = obj1.GetComponent<JewelObj>();
        JewelObj Jewel2 = obj2.GetComponent<JewelObj>();
        List<JewelObj> NeiObj1 = Ulti.ListPlus(Jewel1.GetCollumn(Jewel2.jewel.JewelPosition, Jewel1.jewel.JewelType, null),
                                         Jewel1.GetRow(Jewel2.jewel.JewelPosition, Jewel1.jewel.JewelType, null), Jewel1);
        List<JewelObj> NeiObj2 = Ulti.ListPlus(Jewel2.GetCollumn(Jewel1.jewel.JewelPosition, Jewel2.jewel.JewelType, null),
                                         Jewel2.GetRow(Jewel1.jewel.JewelPosition, Jewel2.jewel.JewelType, null), Jewel2);



        if (Jewel1.jewel.JewelType == 99 || Jewel2.jewel.JewelType == 99)
            if (Jewel1.jewel.JewelType == 8 || Jewel2.jewel.JewelType == 8)
            {
                Jewel1.SetBackAnimation(obj2);
                Jewel2.SetBackAnimation(obj1);
                return;
            }

        if (NeiObj1.Count >= 3 || NeiObj2.Count >= 3 || Jewel1.jewel.JewelType == 8 || Jewel2.jewel.JewelType == 8)
        {
            Ulti.MoveTo(obj1, obj2.transform.localPosition, 0.2f);
            Ulti.MoveTo(obj2, obj1.transform.localPosition, 0.2f);
            SwapJewelPosition(obj1, obj2);
            JewelProcess(NeiObj1, NeiObj2, obj1, obj2);
        }
        else
        {
            Jewel1.SetBackAnimation(obj2);
            Jewel2.SetBackAnimation(obj1);
        }
    }

    void backpress()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameState == (int)Timer.GameState.PLAYING)
        {
            Timer.timer.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GameState == (int)Timer.GameState.PAUSE)
        {
            Timer.timer.Resume();
        }
    }
    void JewelProcess(List<JewelObj> list1, List<JewelObj> list2, GameObject obj1, GameObject obj2)
    {
        int c1 = list1.Count;
        int c2 = list2.Count;
        if (c1 > 2)
        {
            ListProcess(list1, obj2, obj1, obj1.GetComponent<JewelObj>().jewel.JewelType);
        }
        else if (obj1.GetComponent<JewelObj>().jewel.JewelType == 8)
        {
            obj2.GetComponent<JewelObj>().Destroy();
            PDestroyType(obj2.GetComponent<JewelObj>().jewel.JewelType, obj2.transform.position);
            obj1.GetComponent<JewelObj>().Destroy();
        }

        if (c2 > 2)
        {
            ListProcess(list2, obj1, obj2, obj2.GetComponent<JewelObj>().jewel.JewelType);
        }
        else if (obj2.GetComponent<JewelObj>().jewel.JewelType == 8)
        {
            obj1.GetComponent<JewelObj>().Destroy();
            PDestroyType(obj1.GetComponent<JewelObj>().jewel.JewelType, obj1.transform.position);
            obj2.GetComponent<JewelObj>().Destroy();
        }

    }
    public void JewelProcess(List<JewelObj> list1, GameObject obj1)
    {
        int c1 = list1.Count;
        if (c1 > 2)
        {
            ListProcess(list1, obj1, null, obj1.GetComponent<JewelObj>().jewel.JewelType);
        }

    }

    bool ListProcess(List<JewelObj> list, GameObject obj, GameObject obj1, int type)
    {
        Vector3 v;

        if (obj1 != null)
        {
            JewelScript = obj1.GetComponent<JewelObj>();
            v = new Vector3(JewelScript.jewel.JewelPosition.x, JewelScript.jewel.JewelPosition.y);
        }
        else
        {
            JewelScript = obj.GetComponent<JewelObj>();
            v = new Vector3(JewelScript.jewel.JewelPosition.x, JewelScript.jewel.JewelPosition.y);
        }

        int c = list.Count;
        if (c == 3)
        {
            DestroyJewel(list);
            EffectSpawner.effect.ComBoInc();
            dropjewel();
            return false;
        }
        else if (c == 4)
        {
            ReGroup(list, type, (int)Power.BOOM, v);
            DestroyRandom();
            EffectSpawner.effect.ComBoInc();
            dropjewel();
        }
        else if (c >= 5)
        {
            ReGroup(list, 8, (int)Power.MAGIC, v);
            EffectSpawner.effect.ComBoInc();
            DestroyRandom();
            DestroyRandom();
            dropjewel();
        }

        return true;
    }
    void dropjewel()
    {
        drop.DELAY = DROP_DELAY;
        drop.enabled = true;
    }
    void DestroyJewel(List<JewelObj> list)
    {
        SoundController.Sound.JewelCrash();
        foreach (var item in list)
        {
            item.Destroy();
        }
    }
    void ReGroup(List<JewelObj> list, int type, int power, Vector2 pos)
    {
        SoundController.Sound.JewelCrash();
        foreach (var item in list)
        {
            item.ReGroup(pos);
        }
        StartCoroutine(SpawnJewelPower(type, power, pos));
    }

    GameObject JewelTouchChecker(Vector3 mouseposition)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            return Physics2D.OverlapPoint(touchPos).gameObject;
        }
        return null;
    }
    //swap map jewel position
    void SwapJewelPosition(GameObject jewel1, GameObject jewel2)
    {
        JewelObj tmp1 = jewel1.GetComponent<JewelObj>();
        JewelObj tmp2 = jewel2.GetComponent<JewelObj>();

        Vector2 tmp = tmp1.jewel.JewelPosition;
        tmp1.jewel.JewelPosition = tmp2.jewel.JewelPosition;
        tmp2.jewel.JewelPosition = tmp;

        GameObject Objtmp = JewelSpawner.spawn.JewelGrib[(int)tmp1.jewel.JewelPosition.x, (int)tmp1.jewel.JewelPosition.y];
        JewelSpawner.spawn.JewelGrib[(int)tmp1.jewel.JewelPosition.x, (int)tmp1.jewel.JewelPosition.y] = jewel2;
        JewelSpawner.spawn.JewelGrib[(int)tmp2.jewel.JewelPosition.x, (int)tmp2.jewel.JewelPosition.y] = Objtmp;

        JewelObj scripttmp = tmp1;
        JewelSpawner.spawn.JewelGribScript[(int)tmp2.jewel.JewelPosition.x, (int)tmp2.jewel.JewelPosition.y] = tmp2;
        JewelSpawner.spawn.JewelGribScript[(int)tmp1.jewel.JewelPosition.x, (int)tmp1.jewel.JewelPosition.y] = scripttmp;
        if (tmp1.jewel.JewelType == 99 || tmp2.jewel.JewelType == 99)
            WinChecker();

    }
    IEnumerator SpawnJewelPower(int type, int power, Vector2 pos)
    {

        yield return new WaitForSeconds(0.4f);
        GameObject tmp = JewelSpawner.spawn.SpawnJewelPower(type, power, pos);
        yield return new WaitForSeconds(0.2f);
        tmp.GetComponent<Collider2D>().enabled = true;
    }

    public void CellRemoveEffect(int x, int y)
    {
        if (x - 1 >= 0 && GribManager.cell.GribCellObj[x - 1, y] != null)
            GribManager.cell.GribCellObj[x - 1, y].RemoveEffect();

        if (x + 1 < 7 && GribManager.cell.GribCellObj[x + 1, y] != null)
            GribManager.cell.GribCellObj[x + 1, y].RemoveEffect();

        if (y - 1 >= 0 && GribManager.cell.GribCellObj[x, y - 1] != null)
            GribManager.cell.GribCellObj[x, y - 1].RemoveEffect();

        if (y + 1 < 9 && GribManager.cell.GribCellObj[x, y + 1] != null)
            GribManager.cell.GribCellObj[x, y + 1].RemoveEffect();


    }

    public void PDestroyRow(int _x, int y)
    {
        dropjewel();
        SoundController.Sound.Fire();
        List<CellObj> celleffect = new List<CellObj>();
        List<JewelObj> jeweldes = new List<JewelObj>();
        for (int x = 0; x < 7; x++)
        {
            if (_x != x)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect > 0)
                    celleffect.Add(GribManager.cell.GribCellObj[x, y]);
                if (JewelSpawner.spawn.JewelGribScript[x, y] != null && JewelSpawner.spawn.JewelGribScript[x, y].jewel.JewelType != 99 && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 0)
                    jeweldes.Add(JewelSpawner.spawn.JewelGribScript[x, y]);
            }
        }
        foreach (CellObj item in celleffect)
        {
            item.RemoveEffect();
        }
        foreach (JewelObj item in jeweldes)
        {
            item.Destroy();
        }
    }
    public void PDestroyCollumn(int x, int _y)
    {
        dropjewel();
        SoundController.Sound.Fire();
        List<CellObj> celleffect = new List<CellObj>();
        List<JewelObj> jeweldes = new List<JewelObj>();
        for (int y = 0; y < 9; y++)
        {
            if (_y != y)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect > 0)
                    celleffect.Add(GribManager.cell.GribCellObj[x, y]);
                if (JewelSpawner.spawn.JewelGribScript[x, y] != null && JewelSpawner.spawn.JewelGribScript[x, y].jewel.JewelType != 99 && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 0)
                    jeweldes.Add(JewelSpawner.spawn.JewelGribScript[x, y]);
            }
        }
        foreach (CellObj item in celleffect)
        {
            item.RemoveEffect();
        }
        foreach (JewelObj item in jeweldes)
        {
            item.Destroy();
        }
    }
    public void PBoom(int x, int y)
    {
        dropjewel();
        for (int i = x - 1; i <= x + 1; i++)
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i != x || j != y)
                    if (i >= 0 && i < 7 && j >= 0 && j < 9 && JewelSpawner.spawn.JewelGribScript[i, j] != null && JewelSpawner.spawn.JewelGribScript[i, j].jewel.JewelType != 99)
                        JewelSpawner.spawn.JewelGribScript[i, j].Destroy();
            }
    }

    public void PDestroyType(int type, Vector3 pos)
    {
        StartCoroutine(DestroyType(type, pos));
    }

    IEnumerator DestroyType(int type, Vector3 pos)
    {
        NoSelect.SetActive(true);
        dropjewel();
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                JewelObj tmp = JewelSpawner.spawn.JewelGribScript[x, y];
                if (tmp != null && tmp.jewel.JewelType == type)
                {
                    EffectSpawner.effect.MGE(pos, JewelSpawner.spawn.JewelGrib[x, y].transform.position);
                    tmp.Destroy();

                }

            }
        }
        yield return new WaitForSeconds(0.2f);
        NoSelect.SetActive(false);
    }
    public void PBonusTime()
    {
        StartCoroutine(TimeInc());
    }

    public void DestroyRandom()
    {
        //uu tien destroy ganh
        dropjewel();
        if (PLayerInfo.MODE == 1)
        {
            if (!isStar)
            {
                List<CellObj> listeff = getListCellEffect();

                if (listeff.Count > 0)
                {
                    CellObj tmp = listeff[Random.Range(0, listeff.Count)];
                    tmp.RemoveEffect();
                    EffectSpawner.effect.Thunder(GribManager.cell.GribCell[(int)tmp.cell.CellPosition.x, (int)tmp.cell.CellPosition.y].transform.position);
                }
                else
                {
                    destroynotempty();
                }

            }
            else
            {
                Vector2 vtmp = posUnderStar();
                JewelObj tmp = JewelSpawner.spawn.JewelGribScript[(int)vtmp.x, (int)vtmp.y];
                if (tmp != null && tmp != JewelStar)
                {
                    tmp.Destroy();
                    EffectSpawner.effect.Thunder(GribManager.cell.GribCell[(int)tmp.jewel.JewelPosition.x, (int)tmp.jewel.JewelPosition.y].transform.position);
                }
            }
        }
    }
    private List<CellObj> getListCellEffect()
    {
        List<CellObj> tmp = new List<CellObj>();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect > 0)
                {
                    tmp.Add(GribManager.cell.GribCellObj[x, y]);
                }
            }
        }
        return tmp;
    }
    private List<CellObj> getListNotEmpty()
    {
        List<CellObj> tmp = new List<CellObj>();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellType > 1)
                {
                    if (JewelSpawner.spawn.JewelGribScript[x, y] != null)
                        tmp.Add(GribManager.cell.GribCellObj[x, y]);
                }
            }
        }
        return tmp;
    }
    private Vector2 posUnderStar()
    {
        List<Vector2> under = new List<Vector2>();
        int x = (int)JewelStar.jewel.JewelPosition.x;
        int y = (int)JewelStar.jewel.JewelPosition.y;
        for (int i = 0; i < y; i++)
        {
            if (JewelSpawner.spawn.JewelGribScript[x, i] != null)
                under.Add(JewelSpawner.spawn.JewelGribScript[x, i].jewel.JewelPosition);
        }
        if (under.Count > 0)
            return under[Random.Range(0, under.Count)];
        else return new Vector2(x, y);
    }
    private void destroynotempty()
    {
        try
        {
            List<CellObj> listnotempty = getListNotEmpty();
            if (listnotempty.Count > 0)
            {
                Vector2 tmp = listnotempty[Random.Range(0, listnotempty.Count)].cell.CellPosition;
                if (JewelSpawner.spawn.JewelGribScript[(int)tmp.x, (int)tmp.y] != null)
                {
                    JewelSpawner.spawn.JewelGribScript[(int)tmp.x, (int)tmp.y].Destroy();
                    EffectSpawner.effect.Thunder(GribManager.cell.GribCell[(int)tmp.x, (int)tmp.y].transform.position);
                }
            }
        }
        catch
        {
        }
    }

    IEnumerator TimeInc()
    {
        int dem = 0;
        int t = 22;
        while (t > 0)
        {
            dem++;
            Timer.timer.GameTime += 1;
            if (Timer.timer.GameTime >= 270f)
            {
                Timer.timer.GameTime = 270f;
                break;
            }
            t -= 1;
            yield return null;
            if (dem >= 270) break;

        }
    }

    public void AddBonusPower()
    {
        int dem = 0;
        while (true)
        {
            dem++;
            if (dem >= 63)
                return;
            int x = Random.Range(0, 7);
            int y = Random.Range(0, 9);
            JewelObj tmp = JewelSpawner.spawn.JewelGribScript[x, y];
            if (tmp != null && tmp.jewel.JewelType != 8 && tmp.jewel.JewelPower == 0 && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 0)
            {
                int r = Random.Range(2, 4);
                tmp.jewel.JewelPower = r;
                EffectSpawner.effect.ThunderRow(JewelSpawner.spawn.JewelGrib[x, y], r);
                return;
            }
        }
    }

    public void ShowStar()
    {
        List<Vector2> listpos = new List<Vector2>();
        Vector2 pos;
        for (int y = 9 - 1; y >= 0; y--)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null)
                    listpos.Add(new Vector2(x, y));
            }
            if (listpos.Count > 0)
                break;
        }
        pos = listpos[Random.Range(0, listpos.Count)];
        JewelSpawner.spawn.SpawnStar(pos);
        SoundController.Sound.StarIn();
    }

    public void WinChecker()
    {
        int Min = 0;
        for (int y = 0; y < 9; y++)
        {
            if (GribManager.cell.GribCellObj[(int)JewelStar.jewel.JewelPosition.x, y] != null)
            {
                Min = y;
                break;
            }
        }

        if ((int)JewelStar.jewel.JewelPosition.y == Min)
        {
            Timer.timer.Win();
            Destroy(JewelStar.gameObject);
        }
    }

    void EnableSelector(Vector3 pos)
    {
        Selector.transform.position = pos;
        Selector.SetActive(true);
    }
}

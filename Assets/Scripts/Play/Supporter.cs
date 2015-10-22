using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Supporter : MonoBehaviour
{

    public static Supporter sp;

    public bool isNomove;

    public Vector2[] AvaiableMove;

    JewelObj[] AvaiableObj = new JewelObj[2];

    private float SP_DELAY = 5f;

    List<Vector2> vtmplist;

    JewelObj obj;

    void Awake()
    {
        sp = this;
    }

    void Update()
    {
        if (SP_DELAY > 0 && GameController.action.GameState == (int)Timer.GameState.PLAYING && !isNomove)
        {
            SP_DELAY -= Time.deltaTime;
        }
        else if (!isNomove && GameController.action.GameState == (int)Timer.GameState.PLAYING)
        {
            RefreshTime();
            isNoMoreMove();
            PlaySuggestionAnim();
        }
    }

    public bool isNoMoreMove()
    {
        StopSuggestionAnim();
        AvaiableMove = new Vector2[2];
        AvaiableObj = new JewelObj[2];

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (JewelSpawner.spawn.JewelGribScript[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 0)
                {
                    obj = JewelSpawner.spawn.JewelGribScript[x, y];
                    JewelObj obj1 = MoveChecker(x, y, obj);
                    if (obj1 != null)
                    {
                        AvaiableMove[0] = obj.jewel.JewelPosition;
                        AvaiableObj[0] = JewelSpawner.spawn.JewelGribScript[(int)AvaiableMove[0].x, (int)AvaiableMove[0].y];
                        AvaiableMove[1] = obj1.jewel.JewelPosition;
                        AvaiableObj[1] = JewelSpawner.spawn.JewelGribScript[(int)AvaiableMove[1].x, (int)AvaiableMove[1].y];
                        isNomove = false;
                        return true;
                    }

                }
            }
        }
        isNomove = true;
        return false;
    }

    public void RefreshTime()
    {
        SP_DELAY = 5f;
    }

    JewelObj MoveChecker(int x, int y, JewelObj obj)
    {
        vtmplist = getListPos(x, y);
        foreach (Vector2 item in vtmplist)
        {
            if (JewelSpawner.spawn.JewelGribScript[(int)item.x, (int)item.y] != null && JewelSpawner.spawn.JewelGribScript[(int)item.x, (int)item.y].jewel.JewelType == 8)
                return JewelSpawner.spawn.JewelGribScript[(int)item.x, (int)item.y];
            else
            {
                List<JewelObj> NeiObj1 = Ulti.ListPlus(obj.GetCollumn(item, obj.jewel.JewelType, null),
                                                       obj.GetRow(item, obj.jewel.JewelType, null), obj);
                if (NeiObj1.Count >= 3)
                    return JewelSpawner.spawn.JewelGribScript[(int)item.x, (int)item.y];
            }
        }

        return null;
    }


    List<Vector2> getListPos(int x, int y)
    {
        vtmplist = new List<Vector2>();
        if (y + 1 < 9 && GribManager.cell.GribCellObj[x, y + 1] != null && GribManager.cell.GribCellObj[x, y + 1].cell.CellEffect == 0)
            vtmplist.Add(new Vector2(x, y + 1));
        if (y - 1 >= 0 && GribManager.cell.GribCellObj[x, y - 1] != null && GribManager.cell.GribCellObj[x, y - 1].cell.CellEffect == 0)
            vtmplist.Add(new Vector2(x, y - 1));
        if (x + 1 < 7 && GribManager.cell.GribCellObj[x + 1, y] != null && GribManager.cell.GribCellObj[x + 1, y].cell.CellEffect == 0)
            vtmplist.Add(new Vector2(x + 1, y));
        if (x - 1 >= 0 && GribManager.cell.GribCellObj[x - 1, y] != null && GribManager.cell.GribCellObj[x - 1, y].cell.CellEffect == 0)
            vtmplist.Add(new Vector2(x - 1, y));
        return vtmplist;
    }

    public void PlaySuggestionAnim()
    {
        if (AvaiableObj[0] != null && AvaiableObj[1] != null)
        {
            AvaiableObj[0].JewelSuggesttion();
            AvaiableObj[1].JewelSuggesttion();
        }
    }
    public void StopSuggestionAnim()
    {
        if (AvaiableObj[0] != null)
        {
            AvaiableObj[0].JewelStopSuggesttion();
        }
        if (AvaiableObj[1] != null)
        {
            AvaiableObj[1].JewelStopSuggesttion();
        }
    }
}

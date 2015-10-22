using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{

    public float DELAY;

    void Update()
    {
        DELAY -= Time.deltaTime;
        if (DELAY <= 0)
        {
            StartCoroutine(DropAndSpawn());
            this.enabled = false;
        }
    }

    IEnumerator DropAndSpawn()
    {
        Drop();
        yield return new WaitForEndOfFrame();
        Spawn();
        BonusPower();
        ShowStar();
    }

    void Drop()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (JewelSpawner.spawn.JewelGribScript[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect != 4)
                    JewelSpawner.spawn.JewelGribScript[x, y].getNewPosition();
            }
        }
    }
    void Spawn()
    {
        int[] h = new int[7];
        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GribManager.cell.GribCellObj[x, y] != null && GribManager.cell.GribCellObj[x, y].cell.CellEffect == 4)
                    s = y + 1;
            }
            for (int y = s; y < 9; y++)
            {
                if (GameController.action.GameState == (int)Timer.GameState.PLAYING)
                    if (GribManager.cell.GribCellObj[x, y] != null && JewelSpawner.spawn.JewelGribScript[x, y] == null)
                    {

                        GameObject tmp = JewelSpawner.spawn.JewelInstantiate(x, y);
                        if (PLayerInfo.MODE == 1 && Random.value > 0.99f)
                        {
                            tmp.GetComponent<JewelObj>().jewel.JewelPower = 4;
                            EffectSpawner.effect.Clock(tmp);
                        }
                        tmp.transform.localPosition = new Vector3(tmp.transform.localPosition.x, 10 + h[x]);
                        h[x]++;
                        StartCoroutine(Ulti.IEDrop(tmp, new Vector2(x, y), GameController.DROP_SPEED));
                        JewelObj script = tmp.GetComponent<JewelObj>();
                        script.render.enabled = true;
                    }
            }
        }
        StartCoroutine(checkNomoremove());
    }

    /// <summary>
    /// check no more move
    /// </summary>
    /// <returns></returns>
    IEnumerator checkNomoremove()
    {
        yield return new WaitForSeconds(0.5f);
        if (!Supporter.sp.isNoMoreMove())
        {
            if (PLayerInfo.MODE == 1)
            {
                Timer.timer.NoSelect.SetActive(true);
                StartCoroutine(ReSpawnGrib());
            }
            else if (true)
            {
                Timer.timer.NoSelect.SetActive(true);
                Timer.timer.Lost();
            }
        }
    }

    IEnumerator ReSpawnGrib()
    {
        Timer.timer.Nomove.SetActive(true);
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (JewelSpawner.spawn.JewelGribScript[x, y] != null && JewelSpawner.spawn.JewelGribScript[x, y].jewel.JewelType != 99)
                    JewelSpawner.spawn.JewelGribScript[x, y].JewelDisable();
            }
        }
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(JewelSpawner.spawn.Respawn());
    }
    void BonusPower()
    {
        if (GameController.action.isAddPower)
        {
            GameController.action.AddBonusPower();
            GameController.action.isAddPower = false;
        }
    }

    /// <summary>
    /// display star
    /// </summary>
    void ShowStar()
    {
        if (GameController.action.isShowStar)
        {
            GameController.action.isShowStar = false;
            GameController.action.ShowStar();
            GameController.action.isStar = true;
        }
    }
}

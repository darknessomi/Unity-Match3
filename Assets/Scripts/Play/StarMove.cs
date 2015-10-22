using UnityEngine;
using System.Collections;

public class StarMove : MonoBehaviour
{

    float X = -1;
    float Y = -1;
    bool actived = false;

    IEnumerator Start()
    {
        X = 0;
        Y = 0;
        yield return new WaitForSeconds(1f);
        actived = true;
        yield return new WaitForSeconds(0.75f);
        GameObject.Find("JewelStar").transform.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject);
    }

    void Update()
    {

        if (actived)
        {
            X = JewelSpawner.spawn.JewelGrib[(int)GameController.action.JewelStar.jewel.JewelPosition.x, (int)GameController.action.JewelStar.jewel.JewelPosition.y].transform.position.x;
            Y = JewelSpawner.spawn.JewelGrib[(int)GameController.action.JewelStar.jewel.JewelPosition.x, (int)GameController.action.JewelStar.jewel.JewelPosition.y].transform.position.y;
        }

        if (X != -1 && X != transform.localPosition.x)
            MoveToX(X);
        if (Y != -1 && Y != transform.localPosition.y)
            MoveToY(Y);

    }
    void MoveToX(float x)
    {
        if (Mathf.Abs(x - transform.localPosition.x) > 0.15)
        {
            if (transform.localPosition.x > x)
                transform.localPosition -= new Vector3(Time.smoothDeltaTime * 6f, 0, 0);
            else if (transform.localPosition.x < x)
                transform.localPosition += new Vector3(Time.smoothDeltaTime * 6f, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            X = -1;
        }
    }
    void MoveToY(float y)
    {
        if (Mathf.Abs(y - transform.localPosition.y) > 0.15)
        {
            if (transform.localPosition.y > y)
                transform.localPosition -= new Vector3(0, Time.smoothDeltaTime * 8f, 0);
            else if (transform.localPosition.y < y)
                transform.localPosition += new Vector3(0, Time.smoothDeltaTime * 8f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            Y = -1;

        }
    }
}

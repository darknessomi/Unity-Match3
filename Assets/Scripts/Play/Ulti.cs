using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ulti : MonoBehaviour
{

    public static List<JewelObj> ListPlus(List<JewelObj> l1, List<JewelObj> l2, JewelObj bonus)
    {
        List<JewelObj> tmp = new List<JewelObj>();
        for (int i = l1.Count - 1; i >= 0; i--)
        {
            tmp.Add(l1[i]);
        }
        if (bonus != null)
            tmp.Add(bonus);

        for (int i = 0; i < l2.Count; i++)
        {
            tmp.Add(l2[i]);
        }

        return tmp;
    }

    public static void MoveTo(GameObject obj, Vector2 NewPos, float duration)
    {
        obj.SetActive(true);
        Animation anim = obj.GetComponent<Animation>();
        //if (anim.GetClipCount() > 1)
        //{
        //    Destroy(anim.GetClip("Moveto"));
        //}
        anim.enabled = true;
        AnimationClip animclip = new AnimationClip();
#if UNITY_5
            animclip.legacy = true;
#endif
        AnimationCurve curvex = AnimationCurve.Linear(0, obj.transform.localPosition.x, duration, NewPos.x);
        AnimationCurve curvey = AnimationCurve.Linear(0, obj.transform.localPosition.y, duration, NewPos.y);
        AnimationCurve curvenable = AnimationCurve.Linear(0, 1, duration, 0);

        animclip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        animclip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        animclip.SetCurve("", typeof(Animation), "m_Enabled", curvenable);

        anim.AddClip(animclip, "Moveto");
        anim.Play("Moveto");
        Destroy(animclip, duration);
    }

    public static void MoveTo(GameObject obj, Vector2 NewPos, float duration, float z)
    {
        Animation anim = obj.GetComponent<Animation>();
        //if (anim.GetClipCount() > 1)
        //{
        //    anim.RemoveClip("Moveto");
        //}
        anim.enabled = true;
        AnimationClip animclip = new AnimationClip();
#if UNITY_5
            animclip.legacy = true;
#endif
        AnimationCurve curvex = AnimationCurve.Linear(0, obj.transform.localPosition.x, duration, NewPos.x);
        AnimationCurve curvey = AnimationCurve.Linear(0, obj.transform.localPosition.y, duration, NewPos.y);
        AnimationCurve curvez = AnimationCurve.Linear(0, z, duration, z);
        AnimationCurve curvenable = AnimationCurve.Linear(0, 1, duration, 0);

        animclip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        animclip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        animclip.SetCurve("", typeof(Transform), "localPosition.z", curvez);
        animclip.SetCurve("", typeof(Animation), "m_Enabled", curvenable);

        anim.AddClip(animclip, "Moveto");
        anim.Play("Moveto");
        Destroy(animclip, duration);
    }
    public static void MoveTo(GameObject obj, Vector2 startpos, Vector2 NewPos, float duration, float z)
    {
        Animation anim = obj.GetComponent<Animation>();
        //if (anim.GetClipCount() > 1)
        //{
        //    anim.RemoveClip("Moveto");
        //}
        anim.enabled = true;
        AnimationClip animclip = new AnimationClip();
#if UNITY_5
                animclip.legacy = true;
#endif
        AnimationCurve curvex = AnimationCurve.Linear(0, startpos.x, duration, NewPos.x);
        AnimationCurve curvey = AnimationCurve.Linear(0, startpos.y, duration, NewPos.y);
        AnimationCurve curvez = AnimationCurve.Linear(0, z, duration, z);
        AnimationCurve curvenable = AnimationCurve.Linear(0, 1, duration, 0);

        animclip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        animclip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        animclip.SetCurve("", typeof(Transform), "localPosition.z", curvez);
        animclip.SetCurve("", typeof(Animation), "m_Enabled", curvenable);

        anim.AddClip(animclip, "Moveto");
        anim.Play("Moveto");
        Destroy(animclip, duration);
    }
    public static IEnumerator IEDrop(GameObject obj, Vector2 NewPos, float speed)
    {
        JewelObj script = obj.GetComponent<JewelObj>();
        Collider2D coll = obj.GetComponent<Collider2D>();
        if (obj != null)
        {
            Transform _tranform = obj.transform;
            coll.enabled = false;
            script.isMove = true;
            while (_tranform != null && _tranform.localPosition.y - NewPos.y > 0.1f)
            {
                _tranform.localPosition -= new Vector3(0, Time.smoothDeltaTime * speed);
                yield return null;
            }
            if (_tranform != null)
            {
                _tranform.localPosition = new Vector3(NewPos.x, NewPos.y);

                script.Bounce();
                script.RuleChecker();
                yield return new WaitForSeconds(0.2f);
                if (coll != null)
                {
                    coll.enabled = true;
                    script.isMove = false;
                }
            }
        }
    }

}